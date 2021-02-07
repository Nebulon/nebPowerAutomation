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
        /// Retrieves a list of physical drive objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the physical drives on the server. If
        /// omitted, the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the physical drive objects on
        /// supported properties. If omitted objects are returned in the order
        /// as they were created in.
        /// </param>
        /// <returns></returns>
        public PhysicalDriveList GetPhysicalDrives(
            PageInput page,
            PhysicalDriveFilter filter,
            PhysicalDriveSort sort)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<PhysicalDriveList>(
                @"getPhysicalDrives",
                parameters
            );
        }

        /// <summary>
        /// Retrieves a list of physical drive update objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the physical drive updates on the server.
        /// If omitted, the server will return all objects as a paginated
        /// response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the physical drive update objects
        /// on supported properties. If omitted objects are returned in the order
        /// as they were created in.
        /// </param>
        /// <returns></returns>
        public PhysicalDriveUpdatesList GetPhysicalDriveUpdates(
            PageInput page,
            PhysicalDriveUpdatesFilter filter,
            PhysicalDriveUpdatesSort sort)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<PhysicalDriveUpdatesList>(
                @"getPhysicalDriveUpdates",
                parameters
            );
        }

        /// <summary>
        /// Turn on the locate LED of a physical drive
        /// </summary>
        /// <param name="wwn">
        /// The world-wide name of the physical drive
        /// </param>
        /// <param name="durationSeconds">
        /// The number of seconds after which the locate LED will automatically
        /// be turned off again
        /// </param>
        /// <returns></returns>
        public bool LocatePhysicalDrive(string wwn, long durationSeconds)
        {
            LocatePhysicalDriveInput input = new LocatePhysicalDriveInput();
            input.DurationSeconds = durationSeconds;
            input.Wwn = wwn;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, true);

            // run mutation and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"locatePhysicalDrive",
                parameters
            );

            return DeliverToken(tokenResponse);
        }

        /// <summary>
        /// Update the firmware of physical drives.
        ///
        /// <para>
        /// Either <c>nPodGuid</c> or <c>spuSerial</c> must be specified
        /// </para>
        /// </summary>
        /// <param name="nPodGuid">
        /// The nPod unique identifier.
        /// </param>
        /// <param name="spuSerial">
        /// The SPU serial number
        /// </param>
        /// <param name="acceptEula">
        /// Specify <c>true</c> if you accept the physical drive end user
        /// license agreement. If not specified or set to <c>false</c> the
        /// update will fail.
        /// </param>
        /// <returns></returns>
        public bool UpdatePhysicalDriveFirmware(
            Guid? nPodGuid,
            string spuSerial,
            bool acceptEula)
        {
            UpdatePhysicalDriveFirmwareInput input = new UpdatePhysicalDriveFirmwareInput();
            input.AcceptEula = acceptEula;
            input.SpuSerial = spuSerial;
            input.NPodGuid = nPodGuid;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, true);

            // run mutation and deliver token
            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"updatePhysicalDriveFirmware",
                parameters
            );

            return DeliverToken(tokenResponse);
        }
    }
}