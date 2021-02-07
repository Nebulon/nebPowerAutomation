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

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Adds an unregistered SPU to the organization
        ///
        /// <para>
        /// SPUs need to be claimed by an organization before they can be used
        /// for nPod creation.While the nPod creation command will perform an
        /// implicit claim, this method allows registering SPUs with an
        /// organization without creating an nPod.
        /// </para>
        /// <para>
        /// Once an SPU was claimed, it will become visible in the <c>GetSpus</c>
        /// query and in the nebulon ON web user interface.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU to register with an organization.
        /// </param>
        /// <returns>If the claim process was successful</returns>
        public bool ClaimSpu(string spuSerial)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("serial", spuSerial, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"claimSPU",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows deletion of SPU information in nebulon ON
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <returns></returns>
        public bool DeleteSpuInformation(string spuSerial)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("serial", spuSerial, true);

            return RunMutation<bool>(
                @"delSPUInfo",
                parameters
            );
        }

        /// <summary>
        /// Retrieves a list of custom diagnostic command requests
        ///
        /// <para>
        /// Custom diagnostic command requests are used by customer satisfaction
        /// teams to run arbitrary troubleshooting commands on SPUs. These
        /// require user confirmation.
        /// </para>
        ///
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number for which to query for custom diagnostic command
        /// requests
        /// </param>
        /// <returns></returns>
        public SpuCustomDiagnostic[] GetSpuCustomDiagnostics(string spuSerial)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, true);

            return RunQueryMany<SpuCustomDiagnostic>(
                @"spuCustomDiagnostics",
                parameters
            );
        }

        /// <summary>
        /// Retrieves a list of SPUs
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the SPUs on the server. If omitted, the
        /// server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the SPU objects on supported
        /// properties. If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns></returns>
        public SpuList GetSpus(PageInput page = null, SpuFilter filter = null, SpuSort sort = null)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<SpuList>(@"getSPUs", parameters);
        }

        /// <summary>
        /// Turns on the locate LED pattern of the SPU
        ///
        /// <para>
        /// Allows identification of an SPU in the servers by turning on the
        /// locate LED pattern for the SPU. Please consult the Cloud-Defined
        /// Storage manual for the LED blink patterns.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <returns>If the process was successful</returns>
        public bool LocateSpu(string spuSerial)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"pingSpu",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Removes an SPU from an organization
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <returns></returns>
        public bool ReleaseSpu(string spuSerial)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, false);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"releaseSPU",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows replacing an SPU
        ///
        /// <para>
        /// The replace services processing unit (SPU) operation is used to
        /// transition the configuration of an old, likely failed, SPU to a new
        /// replacement unit and allows modifying the configuration during the
        /// process.
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod of the old SPU that is being
        /// replaced
        /// </param>
        /// <param name="previousSpuSerial">
        /// The serial number of the old SPU that is being replaced
        /// </param>
        /// <param name="newSpuInfo">
        /// Configuration information for the new SPU
        /// </param>
        /// <param name="sSetGuid">
        /// The storage set information for the existing SPU. This information
        /// can be obtained from the active replacement alert and only used to
        /// verify that the correct SPU is selected.
        /// </param>
        /// <returns></returns>
        public bool ReplaceSpu(
            Guid nPodGuid,
            string previousSpuSerial,
            NPodSpuInput newSpuInfo,
            Guid sSetGuid)
        {
            ReplaceSpuInput input = new ReplaceSpuInput();
            input.NewSpuInfo = newSpuInfo;
            input.NPodGuid = nPodGuid;
            input.PreviousSpuSerial = previousSpuSerial;
            input.SsetGuid = sSetGuid;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"replaceSPU",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows running custom diagnostic commands
        ///
        /// <para>
        /// SPU custom diagnostics requests allows customers to run arbitrary
        /// diagnostic commands on the services processing units as part of
        /// troubleshooting issues during a support case.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU on which to run diagnostic
        /// </param>
        /// <param name="npodGuid">
        /// The unique identifier of the nPod on which to run diagnostic
        /// </param>
        /// <param name="diagnosticName">
        /// The name of the diagnostic to run
        /// </param>
        /// <param name="requestGuid">
        /// The unique identifier of the custom diagnostic request to run
        /// </param>
        /// <returns></returns>
        public bool RunCustomDiagnostics(
            string spuSerial = null,
            Guid? npodGuid = null,
            string diagnosticName = null,
            Guid? requestGuid = null)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, true);
            parameters.Add("podUID", npodGuid, true);
            parameters.Add("diagnosticName", diagnosticName, true);
            parameters.Add("requestUID", requestGuid, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"runCustomDiagnostic",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows to secure-erase data on a services processing unit (SPU)
        ///
        /// <para>
        /// The secure erase functionality allows a deep-erase of data stored
        /// on the physical drives attached to the SPU.Only SPUs that are not
        /// part of a nPod can be secure-erased.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU to secure-erase
        /// </param>
        /// <returns></returns>
        public bool SecureEraseSpu(string spuSerial)
        {
            SecureEraseSpuInput input = new SecureEraseSpuInput();
            input.SpuSerial = spuSerial;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"secureEraseSPU",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows submitting additional debugging information to nebulon ON
        ///
        /// <para>
        /// Used for customers to send additional debug information to nebulon
        /// ON for troubleshooting and resolve issues.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <param name="note">
        /// An optional note to attach to the debug information
        /// </param>
        /// <returns></returns>
        public bool SendSpuDebugInfo(string spuSerial, string note)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, true);
            parameters.Add("note", note, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"sendDebugInfo",
               parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Allows configuring a proxy server for an SPU
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <param name="proxy">
        /// The proxy server IP address
        /// </param>
        /// <returns></returns>
        public bool SetSpuProxy(string spuSerial, string proxy)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("spuSerial", spuSerial, false);
            parameters.Add("proxy", proxy, false);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
               @"setProxy",
               parameters
            );

            return DeliverToken(tokenResponse);
        }
    }
}