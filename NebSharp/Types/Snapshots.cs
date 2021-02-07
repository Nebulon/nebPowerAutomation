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

using System;

namespace NebSharp.Types
{
    /// <summary>
    /// Defines the snapshot consistency level for snapshots
    /// </summary>
    public enum SnapshotConsistencyLevel
    {
        /// <summary>
        /// Consistency is only guaranteed for a single volume
        /// </summary>
        VV,

        /// <summary>
        /// Consistency is only quaranteed for volumes on the same SPU
        /// </summary>
        SPU,

        /// <summary>
        /// Consistency is guaranteed for all volumes in the same nPod
        /// </summary>
        Pod,
    }

    /// <summary>
    /// An input object to create a volume clone
    ///
    /// <para>
    /// Allows the creation of a volume clone from a base volume or snapshot.
    /// Clones are read and writeable copies of another volume. Clones can be
    /// used to quickly instantiate copies of data and data for recovery
    /// purposes when applications require read/write access for copy
    /// operations.
    /// </para>
    /// </summary>
    public sealed class CreateCloneInput
    {
        /// <summary>
        /// Name for the volume clone
        /// </summary>
        [JsonPath("$.cloneVolumeName", true)]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the volume or snapshot to clone
        /// </summary>
        [JsonPath("$.originVolumeUUID", true)]
        public Guid VolumeGuid { get; set; }
    }

    /// <summary>
    /// An input object to create a snapshot schedule template
    ///
    /// <para>
    /// Allows the creation of snapshot schedule templates.Snapshot schedule
    /// templates are used to consistently provision snapshot schedules across
    /// nPods. They are referenced in nPod templates and are provisioned when a
    /// nPod is formed from such a template.
    /// </para>
    /// </summary>
    public sealed class CreateSnapshotScheduleTemplateInput
    {
        /// <summary>
        /// A time in seconds when snapshots will be automatically deleted.
        /// If not specified, snapshots will not be deleted automatically
        /// (not recommended).
        /// </summary>
        [JsonPath("$.expirationSec", false)]
        public long? ExpirationSeconds { get; set; }

        /// <summary>
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </summary>
        [JsonPath("$.ignoreBootLUNs", false)]
        public bool? IgnoreBootVolume { get; set; }

        /// <summary>
        /// Human readable name for the snapshot schedule template
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// A naming pattern for volume snapshot names when snapshots are
        /// automatically created. Available variables for the format
        /// string are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </summary>
        [JsonPath("$.namePattern", true)]
        public string NamePattern { get; set; }

        /// <summary>
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </summary>
        [JsonPath("$.retentionSec", false)]
        public long? RetentionSeconds { get; set; }

        /// <summary>
        /// The schedule by which volume snapshots will be created
        /// </summary>
        [JsonPath("$.schedule", true)]
        public ScheduleInput Schedule { get; set; }
    }

    /// <summary>
    /// A snapshot schedule that is defined for an entire nPod
    ///
    /// <para>
    /// nPod snapshot schedules are defined for the entire nPod vs. for
    /// individual volumes. They are centrally configured through a template or
    /// on-demand and apply to every volume in an nPod.
    /// </para>
    /// </summary>
    public sealed class NPodSnapshotSchedule
    {
        /// <summary>
        /// Defines the consistency level when snapshotting multiple volumes
        /// </summary>
        [JsonPath("$.consistencyLevel", true)]
        public SnapshotConsistencyLevel ConsistencyLevel { get; set; }

        /// <summary>
        /// The days in the month when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfMonth", true)]
        public long[] DayOfMonth { get; set; }

        /// <summary>
        /// The days of the week when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfWeek", true)]
        public long[] DayOfWeek { get; set; }

        /// <summary>
        /// A time in seconds when snapshots will be automatically deleted.
        /// If not specified, snapshots will not be deleted automatically
        /// (not recommended).
        /// </summary>
        [JsonPath("$.expirationSec", true)]
        public long ExpirationSeconds { get; set; }

        /// <summary>
        /// The unique identifier for the nPod snapshot schedule
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// The hours of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.hour", true)]
        public long[] Hour { get; set; }

        /// <summary>
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </summary>
        [JsonPath("$.ignoreBootLUNs", true)]
        public bool IgnoreBootVolume { get; set; }

        /// <summary>
        /// The minutes of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.minute", true)]
        public long[] Minute { get; set; }

        /// <summary>
        /// The months in the year when an operation should be executed
        /// </summary>
        [JsonPath("$.month", true)]
        public long[] Month { get; set; }

        /// <summary>
        /// A naming pattern for volume snapshot names when snapshots are
        /// automatically created. Available variables for the format
        /// string are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </summary>
        [JsonPath("$.namePattern", true)]
        public string NamePattern { get; set; }

        /// <summary>
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </summary>
        [JsonPath("$.retentionSec", true)]
        public long RetentionSeconds { get; set; }

        /// <summary>
        /// The number of nPod templates that make use of this template
        /// </summary>
        [JsonPath("$.snapshotCount", true)]
        public long SnapshotCount { get; set; }

        /// <summary>
        /// The number of snapshots created from this schedule
        /// </summary>
        [JsonPath("$.snapshots[*].uid", true)]
        public Guid[] SnapshotGuids { get; set; }

        /// <summary>
        /// Unique identifiers of the templates that created this schedule
        /// </summary>
        [JsonPath("$.snapScheduleTemplate[*].templateUUID", true)]
        public Guid[] SnapshotScheduleTemplateGuids { get; set; }

        /// <summary>
        /// The SPU serial number on which the schedule is defined
        /// </summary>
        [JsonPath("$.spuSerial", true)]
        public string SpuSerial { get; set; }
    }

    /// <summary>
    /// A schedule object
    ///
    /// <para>
    /// Schedules are used to perform operations automatically. The schedule
    /// defines when and how often the operations are executed.
    /// </para>
    /// </summary>
    public sealed class Schedule
    {
        /// <summary>
        /// The days in the month when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfMonth", true)]
        public long[] DayOfMonth { get; set; }

        /// <summary>
        /// The days of the week when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfWeek", true)]
        public long[] DayOfWeek { get; set; }

        /// <summary>
        /// The hours of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.hour", true)]
        public long[] Hour { get; set; }

        /// <summary>
        /// The minutes of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.minute", true)]
        public long[] Minute { get; set; }

        /// <summary>
        /// The months in the year when an operation should be executed
        /// </summary>
        [JsonPath("$.month", true)]
        public long[] Month { get; set; }
    }

    /// <summary>
    /// An input object to create a schedule
    ///
    /// <para>
    /// Schedules are used to perform operations automatically. The schedule
    /// defines when and how often the operations are executed.
    /// </para>
    /// <para>
    /// Multiple values for <c>Minute</c>, <c>Hour</c>, <c>DayOfWeek</c>,
    /// <c>DayOfMonth</c>, and <c>Month</c> can be specified when the operation
    /// should be executed multiple times in the respective time frame.
    /// </para>
    /// </summary>
    public sealed class ScheduleInput
    {
        /// <summary>
        /// The days in the month when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfMonth", false)]
        public long[] DayOfMonth { get; set; }

        /// <summary>
        /// The days of the week when an operation should be executed
        /// </summary>
        [JsonPath("$.dayOfWeek", false)]
        public long[] DayOfWeek { get; set; }

        /// <summary>
        /// The hours of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.hour", false)]
        public long[] Hour { get; set; }

        /// <summary>
        /// The minutes of the time when an operation should be executed
        /// </summary>
        [JsonPath("$.minute", false)]
        public long[] Minute { get; set; }

        /// <summary>
        /// The months in the year when an operation should be executed
        /// </summary>
        [JsonPath("$.month", false)]
        public long[] Month { get; set; }
    }

    /// <summary>
    /// A snapshot schedule template object
    ///
    /// <para>
    /// Snapshot schedule templates are used to consistently provision snapshot
    /// schedules across nPods. They are referenced in nPod templates and are
    /// provisioned when a nPod is formed from such a template.
    /// </para>
    /// </summary>
    public sealed class SnapshotScheduleTemplate
    {
        /// <summary>
        /// The number of nPod templates that make use of this template
        /// </summary>
        [JsonPath("$.associatedNPodTemplateCount", true)]
        public long AssociatedNPodTempalteCount { get; set; }

        /// <summary>
        /// The number of provisioned snapshot schedules from this template
        /// </summary>
        [JsonPath("$.associatedScheduleCount", true)]
        public long AssociatedScheduleCount { get; set; }

        /// <summary>
        /// Snapshot consistency level. Always set to <c>Volume</c>
        /// </summary>
        [JsonPath("$.consistencyLevel", true)]
        public SnapshotConsistencyLevel ConsistencyLevel { get; set; }

        /// <summary>
        /// A time in seconds when snapshots will be automatically deleted.
        /// If not specified, snapshots will not be deleted automatically
        /// (not recommended).
        /// </summary>
        [JsonPath("$.expirationSec", true)]
        public long ExpirationSeconds { get; set; }

        /// <summary>
        /// The unique identifier of the snapshot schedule template
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </summary>
        [JsonPath("$.ignoreBootLUNs", true)]
        public bool IgnoreBootVolume { get; set; }

        /// <summary>
        /// Human readable name for the snapshot schedule template
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// A naming pattern for volume snapshot names when snapshots are
        /// automatically created. Available variables for the format
        /// string are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </summary>
        [JsonPath("$.namePattern", true)]
        public string NamePattern { get; set; }

        /// <summary>
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </summary>
        [JsonPath("$.retentionSec", true)]
        public long RetentionSeconds { get; set; }

        /// <summary>
        /// The schedule by which volume snapshots will be created
        /// </summary>
        [JsonPath("$.schedule", true)]
        public ScheduleInput Schedule { get; set; }
    }

    /// <summary>
    /// A filter object to filter snapshot schedule templates
    ///
    /// <para>
    /// Allows filtering for specific snapshot schedule templates in nebulon ON.
    /// The filter allows only one property to be specified. If filtering on
    /// multiple properties is needed, use the <c>And</c> and <c>Or</c> options
    /// to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class SnapshotScheduleTemplateFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public SnapshotScheduleTemplateFilter And { get; set; }

        /// <summary>
        /// Filter based on snapshot schedule template unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on snapshot schedule template name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public SnapshotScheduleTemplateFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated snapshot schedule template list
    ///
    /// <para>
    /// Contains a list of snapshot schedule template objects and information
    /// for pagination. By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class SnapshotScheduleTemplateList : PageList<SnapshotScheduleTemplate>
    {
    }

    /// <summary>
    /// A sort object for snapshot schedule templates
    ///
    /// <para>
    /// Allows sorting snapshot schedule templates on common properties. The
    /// sort object allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class SnapshotScheduleTemplateSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update snapshot schedule template properties
    ///
    /// <para>
    /// Allows updating of snapshot schedule template properties. Snapshot
    /// schedule templates are used to consistently provision snapshot
    /// schedules across nPods. They are referenced in nPod templates and are
    /// provisioned when a nPod is formed from such a template.
    /// </para>
    /// </summary>
    public sealed class UpdateSnapshotScheduleTemplateInput
    {
        /// <summary>
        /// A time in seconds when snapshots will be automatically deleted.
        /// If not specified, snapshots will not be deleted automatically
        /// (not recommended).
        /// </summary>
        [JsonPath("$.expirationSec", false)]
        public long? ExpirationSeconds { get; set; }

        /// <summary>
        /// Allows specifying if boot volumes shall be included when doing
        /// snapshots (<c>true</c>) or if they shall be ignored (<c>false</c>).
        /// By default, all volumes are included.
        /// </summary>
        [JsonPath("$.ignoreBootLUNs", false)]
        public bool? IgnoreBootVolume { get; set; }

        /// <summary>
        /// Human readable name for the snapshot schedule template
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// A naming pattern for volume snapshot names when snapshots are
        /// automatically created. Available variables for the format
        /// string are from the standard <c>strftime</c> function. Additionally
        /// <c>%v</c> is used for the base volume name.
        /// </summary>
        [JsonPath("$.namePattern", false)]
        public string NamePattern { get; set; }

        /// <summary>
        /// A time in seconds that prevents users from deleting snapshots. If
        /// not specified, snapshots can be immediately deleted.
        /// </summary>
        [JsonPath("$.retentionSec", false)]
        public long? RetentionSeconds { get; set; }

        /// <summary>
        /// The schedule by which volume snapshots will be created
        /// </summary>
        [JsonPath("$.schedule", false)]
        public ScheduleInput Schedule { get; set; }
    }
}