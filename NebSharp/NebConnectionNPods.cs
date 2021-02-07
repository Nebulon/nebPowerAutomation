/*
 *
 * Copyright 2021 Nebulon, Inc.
 * All Rights Reserved.
 *
 * DISCLAIMER: THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
 * OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *
 */

using NebSharp.Types;
using System;
using System.Threading;

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Total wait time in seconds until a nPod is created
        /// </summary>
        private const int NPOD_CREATE_WAITTIME_SEC = 60 * 45;

        /// <summary>
        /// Allows creation of a new nPod
        ///
        /// <para>
        /// A nPod is a collection of network-connected application servers
        /// with SPUs installed that form an application cluster. Together, the
        /// SPUs in a nPod serve shared or local storage to the servers in the
        /// application cluster, e.g.a hypervisor cluster, container platform,
        /// or clustered bare metal application.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// Name of the new nPod
        /// </param>
        /// <param name="nPodGroupGuid">
        /// The unique identifier of the nPod group this nPod will be added to
        /// </param>
        /// <param name="nPodSpuInputs">
        /// List of SPU configuration information that will be used in the new
        /// nPod
        /// </param>
        /// <param name="templateGuid">
        /// The unique identifier of the nPod template to use for the new nPod
        /// </param>
        /// <param name="note">
        /// An optional note for the new nPod
        /// </param>
        /// <param name="timezone">
        /// The timezone to be configured for all SPUs in the nPod
        /// </param>
        /// <param name="ignoreWarnings">
        /// If specified and set to <c>true</c> the nPod creation will proceed
        /// even if nebulon ON reports warnings. It is advised to not ignore
        /// warnings. Consequently, the default behavior is that the nPod
        /// creation will fail when nebulon ON reports validation errors.
        /// </param>
        /// <returns>The new nPod</returns>
        public NPod CreateNPod(
            string name,
            Guid nPodGroupGuid,
            NPodSpuInput[] nPodSpuInputs,
            Guid templateGuid,
            string note,
            string timezone,
            bool ignoreWarnings)
        {
            // check for potential issues that nebulon ON predicts
            Issues issues = GetNewNPodIssues(nPodSpuInputs);
            issues.AssertNoIssues(ignoreWarnings);

            CreateNPodInput input = new CreateNPodInput();
            input.Name = name;
            input.NPodGroupGuid = nPodGroupGuid;
            input.Spus = nPodSpuInputs;
            input.NPodTemplateGuid = templateGuid;
            input.Note = note;
            input.Timezone = timezone;

            // setup parameters for nPod creation
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, true);

            // make the request and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"createNPod",
                parameters
            );

            RecipeRecordIdentifier identifier = DeliverTokenV2(tokenResponse);

            if (identifier == null)
                throw new Exception("Uncomprehensive information returned from server");

            // wait for recipe completion
            DateTime start = DateTime.UtcNow;

            // recipe filter
            NPodRecipeFilter filter = new NPodRecipeFilter();
            filter.NPodGuid = identifier.NPodGuid;
            filter.RecipeGuid = identifier.RecipeGuid;

            while (true)
            {
                Thread.Sleep(5000);

                // query for recipes
                RecipeRecordList recipes = GetNPodRecipes(filter);

                // if there is no record in the cloud wait a few more seconds
                // this case should not exist. TODO: Remove in next version.
                if (recipes.Items.Length == 0)
                    continue;

                // based on the query there should be exactly one
                RecipeRecord recipe = recipes.Items[0];

                // execution failed
                if (recipe.State == RecipeState.Failed)
                {
                    string error = string.Concat("nPod creation failed", recipe.Status);
                    throw new Exception(error);
                }

                // execution completed
                if (recipe.State == RecipeState.Completed)
                {
                    NPodFilter nPodFilter = new NPodFilter();
                    nPodFilter.NPodGuid = new GuidFilter();
                    nPodFilter.NPodGuid.MustEqual = identifier.NPodGuid;

                    NPodList nPods = GetNebNPods(
                        PageInput.First,
                        nPodFilter,
                        null
                    );

                    return nPods.Items[0];
                }

                // still ongoing
                double duration = (DateTime.UtcNow - start).TotalSeconds;
                double timeRemaining = NPOD_CREATE_WAITTIME_SEC - duration;

                if (timeRemaining <= 0)
                    throw new Exception("nPod creation timed out");
            }
        }

        /// <summary>
        /// Delete an existing nPod
        /// <para>
        /// Deletes an nPod and erases all stored data.During nPod deletion the
        /// configuration of SPUs in an nPod is wiped and data encryption keys
        /// are erased. This renders all data in the nPod unrecoverable. This
        /// operation is irreversible.
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod to delete
        /// </param>
        /// <param name="secureErase">
        /// Forces a secure wipe of the nPod. While this is not required in
        /// most cases as nPod deletion will destroy the encryption keys and
        /// render data unreadable, it allows to explicitly overwrite data on
        /// server SSDs. Only use this flag when decommissioning storage as the
        /// secure_wipe procedure will take some time.
        /// </param>
        /// <returns></returns>
        public bool DeleteNPod(Guid nPodGuid, bool secureErase)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uid", nPodGuid, false);
            parameters.Add("secureErase", secureErase, true);

            // make the request and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"delPod",
                parameters
            );
            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Retrieve a list of provisioned nPods
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the nPods on the server. If omitted, the
        /// server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the nPod objects on supported
        /// properties. If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns>
        /// A paginated list of nPods
        /// </returns>
        public NPodList GetNebNPods(
            PageInput page,
            NPodFilter filter,
            NebNpodSort sort)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<NPodList>(@"getNPods", parameters);
        }

        /// <summary>
        /// Allows sending verbose diagnostic information to nebulon ON
        ///
        /// <para>
        /// In cases where more in-depth diagnostic information is required to
        /// resolve customer issues, this method allows capturing and uploading
        /// verbose diagnostic information from SPUs in the specified nPod
        /// through a secure channel to nebulon ON.
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the affected nPod.
        /// </param>
        /// <param name="note">
        /// Allows specifying additional textual information that is
        /// added to the diagnostic bundle.This allows a user to provide
        /// accompanying information, e.g.to describe an observed issue.
        /// </param>
        /// <returns></returns>
        public bool SendNPodDebugInfo(Guid nPodGuid, string note)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", nPodGuid, true);
            parameters.Add("note", note, true);

            // make the request and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"sendDebugInfo",
                parameters
            );
            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows setting the timezone for all SPUs in an nPod
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod that is being modified
        /// </param>
        /// <param name="timezone">
        /// The target timezone for the nPod. Refer to the <c>Timezone</c>
        /// enumeration for available options. By default <c>UTC</c> is used
        /// for the timezone.
        /// </param>
        /// <returns></returns>
        public bool SetNPodTimezone(Guid nPodGuid, string timezone)
        {
            SetNPodTimeZoneInput input = new SetNPodTimeZoneInput();
            input.Timezone = timezone;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", nPodGuid, false);
            parameters.Add("input", input, false);

            // make the request and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"setNPodTimeZone",
                parameters
            );
            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Internal method that checks for issues during nPod creation.
        /// </summary>
        /// <param name="nPodSpuInputs"></param>
        /// <returns></returns>
        private Issues GetNewNPodIssues(NPodSpuInput[] nPodSpuInputs)
        {
            // current API expects a list of spu serial numbers
            string[] spuSerials = new string[nPodSpuInputs.Length];

            for (int i = 0; i < spuSerials.Length; i++)
            {
                spuSerials[i] = nPodSpuInputs[i].Serial;
            }

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerials", spuSerials);

            return RunQuery<Issues>(@"newPodIssues", parameters);
        }
    }
}