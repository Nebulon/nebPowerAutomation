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
        /// Total wait time in seconds until a snapshot is created
        /// </summary>
        private const int VOLUME_CREATE_WAITTIME_SEC = 60 * 2;

        /// <summary>
        /// Allows creation of a new volume
        /// </summary>
        /// <param name="input">
        /// Configuration definition for the new volume
        /// </param>
        /// <returns>The created volume</returns>
        public Volume CreateVolume(CreateVolumeInput input)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"createVolumeV3", parameters);

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
                Thread.Sleep(1000);

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
                    string error = string.Concat("volume creation failed", recipe.Status);
                    throw new Exception(error);
                }

                // execution completed
                if (recipe.State == RecipeState.Completed)
                {
                    VolumeFilter volumeFilter = new VolumeFilter();
                    volumeFilter.Guid = new GuidFilter();
                    volumeFilter.Guid.MustEqual = tokenResponse.WaitOn;

                    VolumeList list = GetVolumes(null, volumeFilter, null);

                    if (list.FilteredCount >= 0)
                        return list.Items[0];
                }

                // still ongoing
                double duration = (DateTime.UtcNow - start).TotalSeconds;
                double timeRemaining = NPOD_CREATE_WAITTIME_SEC - duration;

                if (timeRemaining <= 0)
                    throw new Exception("Snapshot creation timed out");
            }
        }

        /// <summary>
        /// Allows deletion of a volume
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the volume or snapshot to delete
        /// </param>
        /// <returns>If the request was successful</returns>
        public bool DeleteVolume(Guid guid)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"deleteVolume",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Returns a list of configured <b>NebVolume</b> instances based on the
        /// provided page and filter information.
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the volumes on the server. If omitted,
        /// the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the volume objects on supported
        /// properties. If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns>A paginated list of volumes</returns>
        public VolumeList GetVolumes(
            PageInput page = null,
            VolumeFilter filter = null,
            VolumeSort sort = null)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            // this should return exactly one item.
            return RunQuery<VolumeList>(@"getVolumes", parameters);
        }
    }
}