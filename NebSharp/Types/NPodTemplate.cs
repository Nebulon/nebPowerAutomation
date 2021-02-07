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
    /// An input object to create a new nPod template
    ///
    /// <para>
    /// nPod templates are used during nPod creation and are application
    /// specific. The template defines the anticipated data storage savings and
    /// the expected storage artifacts. Architects would compile nPod templates
    /// and users would consume templates during self-service infrastructure
    /// provisioning.
    /// </para>
    /// </summary>
    public sealed class CreateNPodTemplateInput
    {
        /// <summary>
        /// The name of the application that will be running on the nPod.
        /// </summary>
        [JsonPath("$.app", false)]
        public string Application { get; set; }

        /// <summary>
        /// Allows specifying an HTTP(s) URL for a boot image that is applied
        /// to the boot volume when an nPod is created.
        /// </summary>
        [JsonPath("$.bootImageURL", false)]
        public string BootImageURL { get; set; }

        /// <summary>
        /// If set to <c>true</c> nebulon ON will provision a boot volume for
        /// the server's operating system. If set, the parameter
        /// <c>BootVolumeSizeBytes</c>must also be specified.
        /// </summary>
        [JsonPath("$.bootVolume", false)]
        public bool? BootVolume { get; set; }

        /// <summary>
        /// The size of the boot volume to create in bytes. This value is only
        /// considered when the parameter <c>BootVolume</c> is set to <c>true</c>.
        /// </summary>
        [JsonPath("$.bootVolumeSizeBytes", false)]
        public long? BootVolumeSizeBytes { get; set; }

        /// <summary>
        /// Specifies if volumes shall be mirrored for high availability. If
        /// set to <c>true</c> two copies of the same volume will be created in
        /// an nPod on different SPUs for high availability.
        /// </summary>
        [JsonPath("$.mirroredVolume", false)]
        public bool? MirroredVolume { get; set; }

        /// <summary>
        /// The name of the nPod template to update. The name cannot
        /// be changed. If users want to change the name of a nPod template they
        /// should clone the template with a new name and delete the old record
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the nPod template
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// The name of the operating system that will be installed on servers
        /// in the nPod.
        /// </summary>
        [JsonPath("$.os", false)]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// The anticipated saving factor for the specified application after
        /// data compression and data deduplication. Allowed values are between
        /// <c>1.0</c> and <c>10.0</c>. nebulon ON will use this assumption for
        /// provisioning storage volumes.
        /// </summary>
        [JsonPath("$.savingFactor", false)]
        public double? SavingFactor { get; set; }

        /// <summary>
        /// Allows configuring volume export options. If set to <c>true</c> all
        /// volumes except boot volumes will be made available to each host /
        /// server in the nPod for read and write access. If set to <c>false</c>
        /// volumes will only be made available to the local host of every SPU.
        /// By default volumes are created as shared volumes.
        /// </summary>
        [JsonPath("$.sharedLUN", false)]
        public bool? SharedLun { get; set; }

        /// <summary>
        /// Allows specifying snapshot schedule templates that will be
        /// automatically created for any derived nPods after nPod creation.
        /// </summary>
        [JsonPath("$.snapshotScheduleTemplatesUUIDs", false)]
        public Guid[] SnapshotScheduleTemplateGuids { get; set; }

        /// <summary>
        /// Allows specifying a volume count. This option is only allowed when
        /// <c>SharedVolume</c> is set to <c>false</c> and allows creating a
        /// specific number of volumes per host / server. This is useful when
        /// the size of the volume does not matter but the number of volumes is
        /// important.
        /// </summary>
        [JsonPath("$.volumeCount", false)]
        public long? VolumeCount { get; set; }

        /// <summary>
        /// The size of volumes to create in bytes. Either volume size or
        /// volume count must be present.
        /// </summary>
        [JsonPath("$.volumeSizeBytes", false)]
        public long? VolumeSizeBytes { get; set; }
    }

    /// <summary>
    /// Defines a nebulon Pod (nPod) template
    ///
    /// <para>
    /// nPod templates are used during nPod creation and are application
    /// specific. The template defines the anticipated data storage savings and
    /// the expected storage artifacts. Architects would compile nPod templates
    /// and users would consume templates during self-service infrastructure
    /// provisioning.
    /// </para>
    /// </summary>
    public sealed class NPodTemplate
    {
        /// <summary>
        /// Name of the application running on the hosts in the nPod
        /// </summary>
        [JsonPath("$.app", true)]
        public string Application { get; set; }

        /// <summary>
        /// If true, the template will provision a boot volume that is exported
        /// to the local host with LUN ID 0
        /// </summary>
        [JsonPath("$.bootVolume", true)]
        public bool BootVolume { get; set; }

        /// <summary>
        /// Allows specifying a URL to an O/S image for the boot volume
        /// </summary>
        [JsonPath("$.bootImageURL", true)]
        public string BootVolumeImageUrl { get; set; }

        /// <summary>
        /// Indicates the boot volume size in bytes
        /// </summary>
        [JsonPath("$.bootVolumeSizeBytes", true)]
        public long BootVolumeSizeBytes { get; set; }

        /// <summary>
        /// The unique identifier of the template if versioned
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// If true, data volumes will be mirrored within the nPod. If false,
        /// no high availability is provided in case an SPU fails
        /// </summary>
        [JsonPath("$.mirroredVolume", true)]
        public bool MirroredVolume { get; set; }

        /// <summary>
        /// Indicates if the template can be modified
        /// </summary>
        [JsonPath("$.mutable", true)]
        public bool Mutable { get; set; }

        /// <summary>
        /// The unique name of the nPod template
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// If true, the template is provided to customers by nebulon. If false,
        /// the template is custom and created by the customer
        /// </summary>
        [JsonPath("$.nebulonTemplate", true)]
        public bool NebulonTemplate { get; set; }

        /// <summary>
        /// An optional note for the nPod template
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Name of the operating system running on the hosts in the nPod
        /// </summary>
        [JsonPath("$.os", true)]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// The unique identifier of the original template if versioned
        /// </summary>
        [JsonPath("$.parentUUID", true)]
        public Guid ParentGuid { get; set; }

        /// <summary>
        /// Anticipated data saving factor after compression and deduplication
        /// </summary>
        [JsonPath("$.savingFactor", true)]
        public double SavingFactor { get; set; }

        /// <summary>
        /// If true, data volumes will be presented to all hosts in the nPod.
        /// If this value is set to false, volumes will only be presented to
        /// the local hosts of each SPU
        /// </summary>
        [JsonPath("$.sharedLun", true)]
        public bool SharedLun { get; set; }

        /// <summary>
        /// List of associated snapshot schedule templates contained in this
        /// template
        /// </summary>
        [JsonPath("$.snapshotScheduleTemplates[*].uuid", true)]
        public Guid[] SnapshotScheduleTemplateGuids { get; set; }

        /// <summary>
        /// The version of the template. Every update creates a new version
        /// </summary>
        [JsonPath("$.version", true)]
        public long Version { get; set; }

        /// <summary>
        /// Indicates how many volumes shall be provisioned by the template
        /// </summary>
        [JsonPath("$.volumeCount", true)]
        public long VolumeCount { get; set; }

        /// <summary>
        /// Volume size in bytes
        /// </summary>
        [JsonPath("$.volumeSizeBytes", true)]
        public long VolumeSizeBytes { get; set; }
    }

    /// <summary>
    /// A filter object to filter nPod templates.
    ///
    /// <para>
    /// Allows filtering for specific nPod templates in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class NPodTemplateFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public NPodTemplateFilter And { get; set; }

        /// <summary>
        /// Filter based on nPod template application name
        /// </summary>
        [JsonPath("$.app", false)]
        public StringFilter Application { get; set; }

        /// <summary>
        /// Filter based on nPod template unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on nPod template name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Filter nPod templates for nebulon templates or custom templates
        /// </summary>
        [JsonPath("$.nebulonTemplate", false)]
        public bool? NebulonTempalte { get; set; }

        /// <summary>
        /// Filter nPod tempaltes for only the latest versions
        /// </summary>
        [JsonPath("$.onlyLastVersion", false)]
        public bool? OnlyLastVersion { get; set; }

        /// <summary>
        /// Filter based on nPod template operating system name
        /// </summary>
        [JsonPath("$.os", false)]
        public StringFilter OperatingSystem { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public NPodTemplateFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated nPod template list object
    ///
    /// <para>
    /// Contains a list of nPod template objects and information for pagination.
    /// By default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class NPodTemplateList : PageList<NPodTemplate>
    {
    }

    /// <summary>
    /// A sort object for nPod templates
    ///
    /// <para>
    /// Allows sorting nPod templates on common properties.The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class NPodTemplateSort
    {
        /// <summary>
        /// Sort direction for the <c>Application</c> property
        /// </summary>
        [JsonPath("$.app", false)]
        public SortDirection Application { get; set; }

        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }

        /// <summary>
        /// Sort direction for the <c>OperatingSystem</c> property
        /// </summary>
        [JsonPath("$.os", false)]
        public SortDirection OperatingSystem { get; set; }
    }

    /// <summary>
    /// An input object to update nPod template properties
    ///
    /// <para>
    /// Every change to a nPod template will create a new version of the
    /// template and generate a new unique identifier (Guid). The parent /
    /// original nPod template is accessible via the nPod template
    /// <c>ParentGuid</c> property.
    /// </para>
    /// </summary>
    public sealed class UpdateNPodTemplateInput
    {
        /// <summary>
        /// The name of the application that will be running on the nPod.
        /// </summary>
        [JsonPath("$.app", false)]
        public string Application { get; set; }

        /// <summary>
        /// Allows specifying an HTTP(s) URL for a boot image that is applied
        /// to the boot volume when an nPod is created.
        /// </summary>
        [JsonPath("$.bootImageURL", false)]
        public string BootImageURL { get; set; }

        /// <summary>
        /// If set to <c>true</c> nebulon ON will provision a boot volume for
        /// the server's operating system. If set, the parameter
        /// <c>BootVolumeSizeBytes</c>must also be specified.
        /// </summary>
        [JsonPath("$.bootVolume", false)]
        public bool? BootVolume { get; set; }

        /// <summary>
        /// The size of the boot volume to create in bytes. This value is only
        /// considered when the parameter <c>BootVolume</c> is set to <c>true</c>.
        /// </summary>
        [JsonPath("$.bootVolumeSizeBytes", false)]
        public long? BootVolumeSizeBytes { get; set; }

        /// <summary>
        /// Specifies if volumes shall be mirrored for high availability. If
        /// set to <c>true</c> two copies of the same volume will be created in
        /// an nPod on different SPUs for high availability.
        /// </summary>
        [JsonPath("$.mirroredVolume", false)]
        public bool? MirroredVolume { get; set; }

        /// <summary>
        /// The name of the nPod template to update. The name cannot
        /// be changed. If users want to change the name of a nPod template they
        /// should clone the template with a new name and delete the old record
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the nPod template
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// The name of the operating system that will be installed on servers
        /// in the nPod.
        /// </summary>
        [JsonPath("$.os", false)]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// The anticipated saving factor for the specified application after
        /// data compression and data deduplication. Allowed values are between
        /// <c>1.0</c> and <c>10.0</c>. nebulon ON will use this assumption for
        /// provisioning storage volumes.
        /// </summary>
        [JsonPath("$.savingFactor", false)]
        public double? SavingFactor { get; set; }

        /// <summary>
        /// Allows configuring volume export options. If set to <c>true</c> all
        /// volumes except boot volumes will be made available to each host /
        /// server in the nPod for read and write access. If set to <c>false</c>
        /// volumes will only be made available to the local host of every SPU.
        /// By default volumes are created as shared volumes.
        /// </summary>
        [JsonPath("$.sharedLUN", false)]
        public bool? SharedLun { get; set; }

        /// <summary>
        /// Allows specifying snapshot schedule templates that will be
        /// automatically created for any derived nPods after nPod creation.
        /// </summary>
        [JsonPath("$.snapshotScheduleTemplatesUUIDs", false)]
        public Guid[] SnapshotScheduleTemplateGuids { get; set; }

        /// <summary>
        /// Allows specifying a volume count. This option is only allowed when
        /// <c>SharedVolume</c> is set to <c>false</c> and allows creating a
        /// specific number of volumes per host / server. This is useful when
        /// the size of the volume does not matter but the number of volumes is
        /// important.
        /// </summary>
        [JsonPath("$.volumeCount", false)]
        public long? VolumeCount { get; set; }

        /// <summary>
        /// The size of volumes to create in bytes. Either volume size or
        /// volume count must be present.
        /// </summary>
        [JsonPath("$.volumeSizeBytes", false)]
        public long? VolumeSizeBytes { get; set; }
    }
}