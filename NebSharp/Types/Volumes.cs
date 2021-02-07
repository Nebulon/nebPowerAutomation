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
    /// Represents volume sync status for mirrored volumes
    /// </summary>
    public enum VolumeSyncState
    {
        /// <summary>
        /// The volume is not mirrored
        /// </summary>
        NotMirrored,

        /// <summary>
        /// The volume is healthy and all data is in-sync
        /// </summary>
        InSync,

        /// <summary>
        /// The volume is unhealthy and data is currently synchronizing
        /// </summary>
        Syncing,

        /// <summary>
        /// The volume is unhealthy and data is currently not synchronizing
        /// </summary>
        Unsynced,

        /// <summary>
        /// The volume sync status is unavailable
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// An input object to create a new volume
    ///
    /// <para>
    /// One of <c>nPodGuid</c> or <c>ownerSpuSerial</c> must be provided. If
    /// <c>ownerSpuSerial</c> or <c>backupSpuSerial</c> are not provided,
    /// nebulon ON will automatically determine where the volume will be
    /// provisioned.
    /// </para>
    /// </summary>
    public sealed class CreateVolumeInput
    {
        /// <summary>
        /// Serial number of the services processing unit that should be the
        /// backup for this volume
        /// </summary>
        [JsonPath("$.backupSPUSerial", false)]
        public string BackupSpuSerial { get; set; }

        /// <summary>
        /// If set to true, warnings will be ignored during the creation of the
        /// volume. As an example, if there is not enough capacity available
        /// for the volume, this may yield in a warning that can be overwritten
        /// using this parameter.
        /// </summary>
        [JsonPath("$.force", false)]
        public bool Force { get; set; }

        /// <summary>
        /// If set to true, the volume will be setup with a mirrored
        /// configuration. This requires two or more services processing units
        /// in an nPod
        /// </summary>
        [JsonPath("$.mirrored", false)]
        public bool Mirrored { get; set; }

        /// <summary>
        /// User friendly name for the volume
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the nPod for this volume. If only this parameter
        /// is provide (no SPU serial numbers), nebulon ON will determine the
        /// best placement for the volume amongst all SPUs.
        /// </summary>
        [JsonPath("$.nPodUUID", true)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Services processing unit serial number that should be the primary
        /// owner of the volume. If the volume is not created with mirroring
        /// enabled, this is the SPU where the volume will be created on.
        /// </summary>
        [JsonPath("$.ownerSPUSerial", false)]
        public string OwnerSpuSerial { get; set; }

        /// <summary>
        /// Size in bytes for the volume. Minimum size for the volume is 1 GiB
        /// and the maximum enforced size is 64 GiB.
        /// </summary>
        [JsonPath("$.sizeBytes", true)]
        public long SizeBytes { get; set; }
    }

    /// <summary>
    /// A volume
    /// </summary>
    public sealed class Volume
    {
        /// <summary>
        /// List of host / server uuids that have access to the volume
        /// </summary>
        [JsonPath("$.accessibleByHosts[*].uuid", false)]
        public Guid[] AccessibleByHostGuids { get; set; }

        /// <summary>
        /// Indicates if the volume is a boot volume
        /// </summary>
        [JsonPath("$.boot", true)]
        public bool Boot { get; set; }

        /// <summary>
        /// Date and time when the volume was created
        /// </summary>
        [JsonPath("$.creationTime", true)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Unique identifier of the host that currently owns this volume
        /// </summary>
        [JsonPath("$.currentOwnerHost.uuid", false)]
        public Guid? CurrentOwnerHostGuid { get; set; }

        /// <summary>
        /// Date and time when this volume will be automatically deleted
        /// </summary>
        [JsonPath("$.expirationTime", false)]
        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Unique identifier for this volume
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// List of uuids for all LUNs created for the volume
        /// </summary>
        [JsonPath("$.luns[*].uuid", false)]
        public Guid[] LunGuids { get; set; }

        /// <summary>
        /// Human readable name for the volume
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the host that is normally the backup for
        /// this volume
        /// </summary>
        [JsonPath("$.naturalBackupHost.uuid", false)]
        public Guid? NaturalBackupHostGuid { get; set; }

        /// <summary>
        /// Serial number of the SPU that is normally the backup for
        /// this volume
        /// </summary>
        [JsonPath("$.naturalBackupSPU.serial", false)]
        public string NaturalBackupSpuSerial { get; set; }

        /// <summary>
        /// Unique identifier of the host that is normally the owner for
        /// this volume
        /// </summary>
        [JsonPath("$.naturalOwnerHost.uuid", false)]
        public Guid? NaturalOwnerHostGuid { get; set; }

        /// <summary>
        /// Serial number of the SPU that is normally the owner for
        /// this volume
        /// </summary>
        [JsonPath("$.naturalOwnerSPU.serial", false)]
        public string NaturalOwnerSpuSerial { get; set; }

        /// <summary>
        /// Unique identifier of the nPod that this volume is part of
        /// </summary>
        [JsonPath("$.nPod.uuid", false)]
        public Guid? NPodGuid { get; set; }

        /// <summary>
        /// If true, this volume is a read-only snapshot instead of a base
        /// volume.
        /// </summary>
        [JsonPath("$.readOnlySnapshot", true)]
        public bool ReadOnlySnapshot { get; set; }

        /// <summary>
        /// Size of the volume
        /// </summary>
        [JsonPath("$.sizeBytes", true)]
        public long SizeBytes { get; set; }

        /// <summary>
        /// If the volume has snapshots, this references all snapshots that
        /// are based on this volume
        /// </summary>
        [JsonPath("$.snapshots[*].uuid", false)]
        public Guid[] SnapshotGuids { get; set; }

        /// <summary>
        /// If the volume is a snapshot, this references the parent volume's
        /// unique identifier
        /// </summary>
        [JsonPath("$.snapshotParent.uuid", false)]
        public Guid? SnapshotParentGuid { get; set; }

        /// <summary>
        /// Indicates the health and sync state of the volume
        /// </summary>
        [JsonPath("$.syncState", false)]
        public VolumeSyncState? VolumeSyncState { get; set; }

        /// <summary>
        /// The world wide name for this volume
        /// </summary>
        [JsonPath("$.wwn", true)]
        public string Wwn { get; set; }
    }

    /// <summary>
    /// A filter object to filter volumes
    ///
    /// <para>
    /// Allows filtering for specific volumes registered in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on
    /// multiple properties is needed, use the <c>And</c> and <c>Or</c> options
    /// to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class VolumeFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public VolumeFilter And { get; set; }

        /// <summary>
        /// Filter for only base volumes
        /// </summary>
        [JsonPath("$.baseOnly", false)]
        public bool? BaseOnly { get; set; }

        /// <summary>
        /// Filter based on creation time
        /// </summary>
        [JsonPath("$.creationTime", false)]
        public IntFilter CreationTime { get; set; }

        /// <summary>
        /// Filter based on creation time
        /// </summary>
        [JsonPath("$.expirationTime", false)]
        public IntFilter ExpirationTime { get; set; }

        /// <summary>
        /// Filter by unique Id
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on volume name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Filter based on nPod unique identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public GuidFilter NPodGuid { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public VolumeFilter Or { get; set; }

        /// <summary>
        /// Filter based on volume parent uuid
        /// </summary>
        [JsonPath("$.parentUUID", false)]
        public GuidFilter ParentGuid { get; set; }

        /// <summary>
        /// Filter based on volume size
        /// </summary>
        [JsonPath("$.sizeBytes", false)]
        public IntFilter SizeBytes { get; set; }

        /// <summary>
        /// Filter for only snapshots
        /// </summary>
        [JsonPath("$.snapshotsOnly", false)]
        public bool? SnapshotsOnly { get; set; }

        /// <summary>
        /// Filter based on volume WWN
        /// </summary>
        [JsonPath("$.wwn", false)]
        public StringFilter Wwn { get; set; }
    }

    /// <summary>
    /// Paginated volume list
    ///
    /// <para>
    /// Contains a list of volume objects and information for pagination. By
    /// default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class VolumeList : PageList<Volume>
    {
    }

    /// <summary>
    /// A sort object for volumes
    ///
    /// <para>
    /// Allows sorting volumes on common properties. The sort object allows
    /// only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class VolumeSort
    {
        /// <summary>
        /// Sort direction for the <c>CreationTime</c> property
        /// </summary>
        [JsonPath("$.creationTime", false)]
        public SortDirection CreationTime { get; set; }

        /// <summary>
        /// Sort direction for the <c>ExpirationTime</c> property
        /// </summary>
        [JsonPath("$.expirationTime", false)]
        public SortDirection ExpirationTime { get; set; }

        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }

        /// <summary>
        /// Sort direction for the <c>SizeBytes</c> property
        /// </summary>
        [JsonPath("$.sizeBytes", false)]
        public SortDirection SizeBytes { get; set; }

        /// <summary>
        /// Sort direction for the <c>Wwn</c> property
        /// </summary>
        [JsonPath("$.wwn", false)]
        public SortDirection Wwn { get; set; }
    }
}