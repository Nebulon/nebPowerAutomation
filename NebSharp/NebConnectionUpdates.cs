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
        /// Abort an ongoing firmware update
        ///
        /// <para>
        /// Either <c>spuSerial</c> or <c>nPodGuid</c> must be specified.
        /// </para>
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU
        /// </param>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod
        /// </param>
        /// <returns></returns>
        public bool AbortFirmwareUpdate(string spuSerial, Guid? nPodGuid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"serial", spuSerial, true);
            parameters.Add(@"podUID", nPodGuid, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"abortUpdateSPUFirmware",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Retrieves a list of update packages
        /// </summary>
        /// <returns>
        /// An object describing latest, available, and recommended nebOS
        /// software packages
        /// </returns>
        public UpdatePackages GetUpdatePackages()
        {
            return RunQuery<UpdatePackages>(@"updatePackages");
        }

        /// <summary>
        /// Retrieves a list of active updates
        ///
        /// <para>
        /// Allows querying for currently ongoing updates and their status
        /// information.
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// Filter active updates for a specific nPod by providing its unique
        /// identifier
        /// </param>
        /// <param name="updateGuid">
        /// The unique identifier of an ongoing update
        /// </param>
        /// <returns></returns>
        public UpdateStateSpu[] GetUpdateState(
            Guid? nPodGuid,
            Guid? updateGuid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"podUID", nPodGuid, true);
            parameters.Add(@"updateID", updateGuid, true);

            return RunQueryMany<UpdateStateSpu>(
                @"spuCustomDiagnostics",
                parameters
            );
        }

        /// <summary>
        /// Update nPod firmware to specified package
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod to update
        /// </param>
        /// <param name="packageName">
        /// The package name to install
        /// </param>
        /// <param name="scheduleAt">
        /// Allows scheduling the installation of a package at the specified
        /// date and time. If omitted, the package will be installed immediately
        /// </param>
        /// <param name="ignoreWarnings">
        /// If specified warnings that are discovered during the update
        /// pre-check are ignored (not recommended). If omitted or set to
        /// <c>false</c> will cause the update to stop.
        /// </param>
        /// <returns></returns>
        public bool UpdateNPodFirmware(
            Guid nPodGuid,
            string packageName,
            DateTime? scheduleAt = null,
            bool ignoreWarnings = false)
        {
            Issues issues = RunUpdatePreCheck(nPodGuid, packageName);
            issues.AssertNoIssues(ignoreWarnings);

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"podUID", nPodGuid, false);
            parameters.Add(@"packageName", packageName, false);
            parameters.Add(@"scheduled", scheduleAt, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"updatePodFirmware",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Update nebOS of an SPU to a specific package
        /// </summary>
        /// <param name="spuSerial">
        /// The serial number of the SPU to update
        /// </param>
        /// <param name="packageName">
        /// The name of the update package to install
        /// </param>
        /// <param name="force">
        /// If set to <c>true</c> the update will bypass any safeguards
        /// </param>
        /// <returns></returns>
        public bool UpdateSpuFirmware(string spuSerial, string packageName, bool force)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"serial", spuSerial, false);
            parameters.Add(@"packageName", packageName, false);
            parameters.Add(@"force", force, true);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"updateSPUFirmware",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Runs diagnostics before an update is installed
        ///
        /// <para>
        /// Ensures that an nPod is in a healthy state and that there are no
        /// issues preventing a successful update.
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod for which the update pre-check
        /// is performed
        /// </param>
        /// <param name="packageName">
        /// The package name to be installed
        /// </param>
        /// <returns>
        /// A list of warnings and errors discovered during the pre-check
        /// </returns>
        private Issues RunUpdatePreCheck(Guid nPodGuid, string packageName)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"podUID", nPodGuid, false);
            parameters.Add(@"packageName", packageName, false);

            return RunQuery<Issues>(@"updatePrecheck", parameters);
        }
    }
}