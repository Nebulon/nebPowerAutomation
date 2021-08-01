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
using System.Collections.Generic;
using System.Threading;

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Total wait time in seconds until a snapshot is created
        /// </summary>
        private const int SNAPSHOT_CREATE_WAITTIME_SEC = 60 * 2;

        /// <summary>
        /// Allows creating a read/writeable clone of a volume or snapshot
        /// <para>
        /// Allows the creation of a volume clone from a base volume or
        /// snapshot. Clones are read and writeable copies of another volume.
        /// Clones can be used to quickly instantiate copies of data and data
        /// for recovery purposes when applications require read/write access
        /// for copy operations.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// The human readable name for the volume clone
        /// </param>
        /// <param name="volumeGuid">
        /// The unique identifier for the volume or snapshot from which to
        /// create the clone.
        /// </param>
        /// <returns></returns>
        public Volume CreateClone(string name, Guid volumeGuid)
        {
            CreateCloneInput input = new CreateCloneInput();
            input.Name = name;
            input.VolumeGuid = volumeGuid;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
              @"createClone", parameters);
            bool deliverySuccess = DeliverToken(tokenResponse);

            if (!deliverySuccess)
                throw new Exception("Clone creation failed");

            // query for the clone
            VolumeFilter filter = new VolumeFilter();
            filter.Name = new StringFilter();
            filter.Name.MustEqual = name;

            DateTime start = DateTime.UtcNow;

            while (true)
            {
                Thread.Sleep(2000);

                VolumeList list = GetVolumes(null, filter, null);

                if (list.FilteredCount > 0)
                    return list.Items[0];

                // check if we should time out.
                double duration = (DateTime.UtcNow - start).TotalSeconds;
                double remaining = SNAPSHOT_CREATE_WAITTIME_SEC - duration;

                if (remaining <= 0)
                    throw new Exception("Clone creation timed out");
            }
        }

        /// <summary>
        /// Allows creation of an on-demand snapshot of volumes
        /// <para>
        /// If multiple volumes are provided, multiple name patterns are
        /// required, where the index of the list of items are related. For
        /// example, the name pattern at index <c>3</c> of the
        /// <c>namePatterns</c> parameter will be applied to the volume
        /// specified at index <c>3</c> of the <c>parentVolumeGuids</c> list.
        /// </para>
        /// </summary>
        /// <param name="parentVolumeGuids">
        /// List of unique identifiers for all volumes for which to create a
        /// snapshot
        /// </param>
        /// <param name="namePatterns">
        /// List of naming patterns for volume snapshots. Options of the
        /// <c>strftime</c> function are available to format time and the
        /// variable <c>%v</c> that will be translated to the volume name.
        /// </param>
        /// <param name="expirationSeconds">
        /// The number of seconds after snapshot creation when the snapshots
        /// will be automatically deleted
        /// </param>
        /// <param name="retentionSeconds">
        /// The number of seconds before a user can delete the snapshots.
        /// </param>
        /// <returns></returns>
        public Volume[] CreateSnapshot(
            Guid[] parentVolumeGuids,
            string[] namePatterns,
            long? expirationSeconds = null,
            long? retentionSeconds = null)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("parentVvUID", parentVolumeGuids, false);
            parameters.Add("snapNamePattern", namePatterns, false);
            parameters.Add("consistencyLevel", SnapshotConsistencyLevel.VV, false);
            parameters.Add("roSnap", true, false);
            parameters.Add("expirationSec", expirationSeconds, false);
            parameters.Add("retentionSec", retentionSeconds, false);

            DateTime creationTime = DateTime.UtcNow;

            TokenResponse tokenResponse = RunMutation<TokenResponse>(
                @"createSnap",
                parameters
            );

            bool deliverySuccess = DeliverToken(tokenResponse);

            if (!deliverySuccess)
                throw new Exception("Snapshot creation failed");

            // query for the volumes (and indirectly get their snapshots)
            VolumeFilter filter = new VolumeFilter();
            filter.Guid = new GuidFilter();
            filter.Guid.In = parentVolumeGuids;

            DateTime start = DateTime.UtcNow;

            VolumeList volumes = GetVolumes(null, filter, null);

            while (true)
            {
                Thread.Sleep(2000);

                // candidates for the snapshots we just created
                List<Guid> candidates = new List<Guid>();

                // there should always be at least one item in the list
                foreach(Volume volume in volumes.Items)
                {
                    foreach(Guid snapshotGuid in volume.SnapshotGuids)
                        candidates.Add(snapshotGuid);
                }

                if (candidates.Count > 0)
                {
                    filter = new VolumeFilter();
                    filter.SnapshotsOnly = true;
                    filter.And = new VolumeFilter();
                    filter.And.CreationTime = new IntFilter();
                    filter.And.CreationTime.GreaterThan = ((DateTimeOffset)creationTime).ToUnixTimeSeconds();

                    VolumeList list = GetVolumes(null, filter, null);

                    if (list.FilteredCount > 0)
                        return list.Items;
                }
                    
                // check if we should time out.
                double duration = (DateTime.UtcNow - start).TotalSeconds;
                double remaining = SNAPSHOT_CREATE_WAITTIME_SEC - duration;

                if (remaining <= 0)
                    throw new Exception("Snapshot creation timed out");
            }
        }

        /// <summary>
        /// Allows creation of a new snapshot schedule template
        /// <para>
        /// Allows the creation of snapshot schedule templates. Snapshot
        /// schedule templates are used to consistently provision snapshot
        /// schedules across nPods. They are referenced in nPod templates and
        /// are provisioned when a nPod is formed from such a template.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// Human readable name for the snapshot schedule template
        /// </param>
        /// <param name="namePattern">
        /// A naming pattern for volume snapshot names when they are
        /// automatically created. Available variables for the format string
        /// are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </param>
        /// <param name="schedule">
        /// The schedule by which volume snapshots will be created
        /// </param>
        /// <param name="expirationSeconds">
        /// A time in seconds when snapshots will be automatically deleted. If
        /// not specified, snapshots will not be deleted automatically (not
        /// recommended)
        /// </param>
        /// <param name="retentionSeconds">
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </param>
        /// <param name="ignoreBootVolumes">
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </param>
        /// <returns></returns>
        public SnapshotScheduleTemplate CreateSnapshotScheduleTemplate(
            string name,
            string namePattern,
            ScheduleInput schedule,
            long? expirationSeconds = null,
            long? retentionSeconds = null,
            bool? ignoreBootVolumes = null)
        {
            CreateSnapshotScheduleTemplateInput input = new CreateSnapshotScheduleTemplateInput();
            input.Name = name;
            input.NamePattern = namePattern;
            input.Schedule = schedule;
            input.ExpirationSeconds = expirationSeconds;
            input.RetentionSeconds = retentionSeconds;
            input.IgnoreBootVolume = ignoreBootVolumes;

            // perpare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<SnapshotScheduleTemplate>(
                @"createSnapshotScheduleTemplate",
                parameters
            );
        }

        /// <summary>
        /// Allows deletion of an existing snapshot schedule template
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the snapshot schedule template to delete
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteSnapshotScheduleTemplate(Guid guid)
        {
            // perpare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);

            return RunMutation<bool>(
                @"deleteSnapshotScheduleTemplate",
                parameters
            );
        }

        /// <summary>
        /// Retrieves a list of provisioned snapshot schedules on an nPod
        /// </summary>
        /// <param name="nPodGuid">
        /// The unique identifier of the nPod from which the snapshot schedules
        /// shall be retrieved.
        /// </param>
        /// <returns>A list of snapshot schedules</returns>
        public NPodSnapshotSchedule[] GetSnapshotSchedules(Guid nPodGuid)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("pod", nPodGuid, true);

            return RunQueryMany<NPodSnapshotSchedule>(
                @"PodSnapshotSchedules",
                parameters
            );
        }

        /// <summary>
        /// Retrieves a list of snapshot schedule template objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the snapshot schedule template objects
        /// on the server. If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the snapshot schedule template
        /// objects on supported properties. If omitted objects are returned in
        /// the order as they were created in.
        /// </param>
        /// <returns></returns>
        public SnapshotScheduleTemplateList GetSnapshotScheduleTemplates(
            PageInput page = null,
            SnapshotScheduleTemplateFilter filter = null,
            SnapshotScheduleTemplateSort sort = null)
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<SnapshotScheduleTemplateList>(
                @"getSnapshotScheduleTemplates",
                parameters
            );
        }

        /// <summary>
        /// Allows updating the properties of a snapshot schedule template
        ///
        /// <para>
        /// Allows updating of snapshot schedule template properties. Snapshot
        /// schedule templates are used to consistently provision snapshot
        /// schedules across nPods. They are referenced in nPod templates and
        /// are provisioned when a nPod is formed from such a template.
        /// </para>
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the snapshot schedule template to update
        /// </param>
        /// <param name="name">
        /// Human readable name for the snapshot schedule template
        /// </param>
        /// <param name="namePattern">
        /// A naming pattern for volume snapshot names when they are
        /// automatically created. Available variables for the format string
        /// are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </param>
        /// <param name="schedule">
        /// The schedule by which volume snapshots will be created
        /// </param>
        /// <param name="expirationSeconds">
        /// A time in seconds when snapshots will be automatically deleted. If
        /// not specified, snapshots will not be deleted automatically (not
        /// recommended)
        /// </param>
        /// <param name="retentionSeconds">
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </param>
        /// <param name="ignoreBootVolumes">
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </param>
        /// <returns></returns>
        public SnapshotScheduleTemplate UpdateSnapshotScheduleTemplate(
            Guid guid,
            string name = null,
            string namePattern = null,
            ScheduleInput schedule = null,
            long? expirationSeconds = null,
            long? retentionSeconds = null,
            bool? ignoreBootVolumes = null)
        {
            UpdateSnapshotScheduleTemplateInput input = new UpdateSnapshotScheduleTemplateInput();
            input.Name = name;
            input.NamePattern = namePattern;
            input.Schedule = schedule;
            input.ExpirationSeconds = expirationSeconds;
            input.RetentionSeconds = retentionSeconds;
            input.IgnoreBootVolume = ignoreBootVolumes;

            // perpare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);
            parameters.Add("input", input, false);

            return RunMutation<SnapshotScheduleTemplate>(
                @"updateSnapshotScheduleTemplate",
                parameters
            );
        }
    }
}