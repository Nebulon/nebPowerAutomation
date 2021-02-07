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
    /// Link aggregation transmit rate for LCAP
    /// </summary>
    public enum BondLACPTransmitRate
    {
        /// <summary>
        /// Transmit rate slow
        /// </summary>
        LACPTransmitRateSlow,

        /// <summary>
        /// Transmit rate fast
        /// </summary>
        LACPTransmitRateFast,
    }

    /// <summary>
    /// Transmit has policy used by link aggregation
    /// </summary>
    public enum BondTransmitHashPolicy
    {
        /// <summary>
        /// Hash policy layer 2
        /// </summary>
        TransmitHashPolicyLayer2,

        /// <summary>
        /// Hash policy layer 3 and 4
        /// </summary>
        TransmitHashPolicyLayer34,

        /// <summary>
        /// Hash policy layer 2 and 3
        /// </summary>
        TransmitHashPolicyLayer23,
    }

    /// <summary>
    /// Link aggregation type for data ports
    /// </summary>
    public enum BondType
    {
        /// <summary>
        /// No link aggregation is used
        /// </summary>
        BondModeNone,

        /// <summary>
        /// Link aggregation IEEE 802.3ad
        /// </summary>
        BondMode8023ad,

        /// <summary>
        /// Link aggregation Balance ALB
        /// </summary>
        BondModeBalanceALB,
    }

    /// <summary>
    /// An input object to create a new nPod
    /// <para>
    /// A nPod is a collection of network-connected application servers with
    /// SPUs installed that form an application cluster.Together, the SPUs in a
    /// nPod serve shared or local storage to the servers in the application
    /// cluster, e.g.a hypervisor cluster, container platform, or clustered
    /// bare metal application.
    /// </para>
    /// </summary>
    public sealed class CreateNPodInput
    {
        /// <summary>
        /// Name of the new nPod
        /// </summary>
        [JsonPath("$.nPodName", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the new nPod
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// The unique identifier of the nPod group this nPod will be added to
        /// </summary>
        [JsonPath("$.nPodGroupUUID", true)]
        public Guid NPodGroupGuid { get; set; }

        /// <summary>
        /// The unique identifier of the nPod template to use for the new nPod.
        /// </summary>
        [JsonPath("$.nPodTemplateUUID", true)]
        public Guid NPodTemplateGuid { get; set; }

        /// <summary>
        /// List of SPU configuration information that will be used in the new
        /// nPod.
        /// </summary>
        [JsonPath("$.spus", true)]
        public NPodSpuInput[] Spus { get; set; }

        /// <summary>
        /// The timezone to be configured for all SPUs in the nPod
        /// </summary>
        [JsonPath("$.timeZone", false)]
        public string Timezone { get; set; }
    }

    /// <summary>
    /// Describes information for nPods that are about to be created
    /// <para>
    /// Allows predicting of the storage configuration of an nPod before its
    /// creation.
    /// </para>
    /// </summary>
    public sealed class ExpectedNPodCapacity
    {
        /// <summary>
        /// Savings factor used for the calculation, provided by the template
        /// </summary>
        [JsonPath("$.templateSavingFactor", false)]
        public long TemplateSavingFactor { get; set; }

        /// <summary>
        /// Total physical drive capacity in blocks (512 bytes)
        /// </summary>
        [JsonPath("$.totalPDCapacityBlk", false)]
        public long TotalPhysicalDiskCapacityBlocks { get; set; }

        /// <summary>
        /// Total capacity presented to hosts
        /// </summary>
        [JsonPath("$.totalPresentedCapacity", false)]
        public long TotalPresentedCapacity { get; set; }

        /// <summary>
        /// Total raw capacity in blocks (512 bytes)
        /// </summary>
        [JsonPath("$.totalRawCapacityBlk", false)]
        public long TotalRawCapacityBlocks { get; set; }

        /// <summary>
        /// Total usable capacity in blocks (512 bytes)
        /// </summary>
        [JsonPath("$.totalUserDataCapacityBlk", false)]
        public long TotalUserDataCapacityBlocks { get; set; }

        /// <summary>
        /// Total number of volumes that will be created
        /// </summary>
        [JsonPath("$.totalVVCount", false)]
        public long TotalVolumeCount { get; set; }
    }

    /// <summary>
    /// An input object to configure SPU networking
    ///
    /// <para>
    /// SPU network configuration is determined at nPod creation.
    /// Customers have the option to use static IP addresses for the control
    /// network or DHCP. When using DHCP, it is recommended to use static IP
    /// reservations for the data networks.
    /// </para>
    /// <para>
    /// Customers can choose between using two separate networks for the data
    /// network or a link aggregation (LACP). When using link aggregation, two
    /// interface names are expected, one if not.
    /// </para>
    /// <para>
    /// When specifying an IP address, it can be either IPv4 or IPv6 and
    /// supports the CIDR address format.
    /// </para>
    /// </summary>
    public sealed class IPInfoConfigInput
    {
        /// <summary>
        /// The IPv4 or IPv6 address for the data network interface.
        /// If CIDR format is used, the <c>NetmaskBits</c> value is ignored. If
        /// <c>Dhcp</c> is set to <c>true</c>, this field must not be specified.
        /// </summary>
        [JsonPath("$.addr", true)]
        public string Address { get; set; }

        /// <summary>
        /// Allows altering the default LACP transmit rate. This field is
        /// ignored if <c>BondMode</c> is not set to <c>BondMode8023ad</c>.
        /// </summary>
        [JsonPath("$.bondLACPTransmitRate", false)]
        public BondLACPTransmitRate? BondLACPTransmitRate { get; set; }

        /// <summary>
        /// Allows alerting the default media independent interface monitoring
        /// interval. This field is ignored when <c>BondMonde</c> is set to
        /// <c>None</c>.
        /// </summary>
        [JsonPath("$.bondMIIMonitorMilliSeconds", false)]
        public long? BondMIIMonitorMilliSeconds { get; set; }

        /// <summary>
        /// Specifies the link aggregation mode for the data
        /// network ports. If not set to <c>None</c>, the <c>Interfaces</c>
        /// parameter must be an array that lists the names of both interfaces:
        /// <c>["enP8p1s0f0np0", "enP8p1s0f1np1"]</c>, if set to <c>None</c>
        /// the specific interface must be identified by its name.
        /// </summary>
        [JsonPath("$.bondModeV2", false)]
        public BondType? BondMode { get; set; }

        /// <summary>
        /// Allows specifying the transmit hashing policy mode when using link
        /// aggregation.This field is ignored when <c>BondMonde</c> is set to
        /// <c>None</c>.
        /// </summary>
        [JsonPath("$.bondTransmitHashPolicy", false)]
        public BondTransmitHashPolicy? BondTransmitHashPolicy { get; set; }

        /// <summary>
        /// Specifies if DHCP should be used for the data network. If
        /// set to <c>true</c>, fields <c>Address</c>, <c>NetMaskBits</c>,
        /// <c>Gateway</c> are optional. If set to <c>false</c>, these values
        /// become mandatory.
        /// </summary>
        [JsonPath("$.dhcp", true)]
        public bool? Dhcp { get; set; }

        /// <summary>
        /// The network gateway address for the network interface.
        /// If <c>Dhcp</c> is set to <c>true</c> this field is optional and
        /// ignored. If static IP address is used, this field is mandatory.
        /// </summary>
        [JsonPath("$.gateway", true)]
        public string Gateway { get; set; }

        /// <summary>
        /// Specifies if the network interface shall use half duplex. By
        /// default, this field should be set to <c>false</c>.
        /// </summary>
        [JsonPath("$.halfDuplex", true)]
        public bool? HalfDuplex { get; set; }

        /// <summary>
        /// List of interfaces that shall be configured with this object.
        /// If <c>BondMone</c> is set to <c>None</c> a single interface
        /// shall be specified. If set to a link aggregation mode both data
        /// interface names shall be specified. Options are "enP8p1s0f0np0" and
        /// "enP8p1s0f1np1".
        /// </summary>
        [JsonPath("$.interfaces", true)]
        public string[] Interfaces { get; set; }

        /// <summary>
        /// Allows setting the interface speed and the duplex mode to specific
        /// values. If set to <c>true</c> the values of <c>SpeedMB</c> and
        /// <c>HalfDuplex</c> are enforced.
        /// </summary>
        [JsonPath("$.lockedSpeed", true)]
        public bool? LockedSpeed { get; set; }

        /// <summary>
        /// Allows setting the maximum transfer unit (MTU) for the interface.
        /// By default an MTU of <c>1500</c> is used.
        /// </summary>
        [JsonPath("$.mtu", true)]
        public long? Mtu { get; set; }

        /// <summary>
        /// The network mask in bits. If <c>Address</c> is specified in CIDR
        /// format, this value will be ignored, otherwise this is a mandatory
        /// field.
        /// </summary>
        [JsonPath("$.netmaskBits", true)]
        public long? NetmaskBits { get; set; }

        /// <summary>
        /// Allows setting the interface speed to a specific value. This field
        /// is ignored when <c>LockedSpeed</c> is set to <c>false</c> (default).
        /// </summary>
        [JsonPath("$.speedMB", true)]
        public long? SpeedMB { get; set; }
    }

    /// <summary>
    /// A sort object for nPods
    ///
    /// <para>
    /// Allows sorting nPods on common properties.The sort object allows
    /// only one property to be specified
    /// </para>
    /// </summary>
    public sealed class NebNpodSort
    {
        /// <summary>
        /// Sort direction for <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// Defines a nebulon Pod (nPod)
    /// <para>
    /// A nPod is a collection of network-connected application servers with
    /// SPUs installed that form an application cluster. Together, the SPUs in
    /// a nPod serve shared or local storage to the servers in the application
    /// cluster, e.g.a hypervisor cluster, container platform, or clustered
    /// bare metal application.
    /// </para>
    /// </summary>
    public sealed class NPod
    {
        /// <summary>
        /// The unique identifier of the nPod
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Number of hosts / servers part of this nPod
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// List of host identifiers part of this nPod
        /// </summary>
        [JsonPath("$.hosts[*].uuid", false)]
        public Guid[] HostGuids { get; set; }

        /// <summary>
        /// The name of the nPod
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the nPod
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// The unique identifier of the nPod group this nPod belongs to
        /// </summary>
        [JsonPath("$.nPodGroup.uuid", false)]
        public Guid NPodGroupGuid { get; set; }

        /// <summary>
        /// List of snapshot identifiers defined in this nPod
        /// </summary>
        [JsonPath("$.snapshots[*].uuid", false)]
        public Guid[] SnapshotGuids { get; set; }

        /// <summary>
        /// Number of spus part of this nPod
        /// </summary>
        [JsonPath("$.spuCount", true)]
        public long SpuCount { get; set; }

        /// <summary>
        /// List of serial numbers part of this nPod
        /// </summary>
        [JsonPath("$.spus[*].serial", false)]
        public string[] SpuSerials { get; set; }

        /// <summary>
        /// List of updates performed on this nPod
        /// </summary>
        [JsonPath("$.updateHistory", false)]
        public UpdateHistory[] UpdateHistory { get; set; }

        /// <summary>
        /// Number of volumes defined in this nPod
        /// </summary>
        [JsonPath("$.volumeCount", true)]
        public long VolumeCount { get; set; }

        /// <summary>
        /// List of volume identifiers defined in this nPod
        /// </summary>
        [JsonPath("$.volumes[*].uuid", false)]
        public Guid[] VolumeGuids { get; set; }

        /*
        /// <summary>
        /// Unique identifier for the nPod template used during nPod creation
        /// </summary>
        [JsonPath("$.nPodTemplate.uid", false)]
        public Guid NPodTemplateGuid { get; set; }
        */
    }

    /// <summary>
    /// Defines a custom diagnostics script
    /// <para>
    /// Custom diagnostics scripts are used by nebulon customer satisfaction
    /// when custom commands and diagnostics scripts need to be executed on
    /// SPUs in customers datacenters to resolve issues.
    /// </para>
    /// <para>
    /// Commands cannot be executed without customer approval as they need to
    /// be approved and authenticated through the security triangle. Custom
    /// diagnostics scripts are the vehicle to facilitate the security
    /// triangle.
    /// </para>
    /// </summary>
    public sealed class NPodCustomDiagnostics
    {
        /// <summary>
        /// Human readable name for the diagnostic script
        /// </summary>
        [JsonPath("$.diagnosticName", false)]
        public string DiagnosticName { get; set; }

        /// <summary>
        /// An optional note for the diagnostics script
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier of the nPod on which the script should run
        /// </summary>
        [JsonPath("$.podUID", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Indicates that the script will only be executed once
        /// </summary>
        [JsonPath("$.onceOnly", false)]
        public bool OnceOnly { get; set; }

        /// <summary>
        /// Unique identifier for the diagnostic script
        /// </summary>
        [JsonPath("$.requestUID", false)]
        public Guid RequestGuid { get; set; }
    }

    /// <summary>
    /// A filter object to filter nPods.
    ///
    /// <para>
    /// Allows filtering for specific nPods in nebulon ON.The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class NPodFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public NPodFilter And { get; set; }

        /// <summary>
        /// Filter based on nPod name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Filter based on nPod unique identifiers
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter NPodGuid { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public NPodFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated nPod list object
    /// <para>
    /// Contains a list of nPod objects and information for pagination.
    /// By default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class NPodList : PageList<NPod>
    {
    }

    /// <summary>
    /// An input object to configure SPUs for nPod creation.
    /// <para>
    /// Allows specifying SPU configuration options when creating a new nPod.
    /// Configuration is mostly for network configuration.
    /// </para>
    /// </summary>
    public sealed class NPodSpuInput
    {
        /// <summary>
        /// Allows configuring the SPUs network interfaces.
        /// </summary>
        [JsonPath("$.SPUDataIPs", false)]
        public IPInfoConfigInput[] DataIps { get; set; }

        /// <summary>
        /// Human readable name for a SPU
        /// </summary>
        [JsonPath("$.SPUName", true)]
        public string Name { get; set; }

        /// <summary>
        /// Serial number for the SPU
        /// </summary>
        [JsonPath("$.SPUSerial", true)]
        public string Serial { get; set; }
    }

    /// <summary>
    /// Allows setting the timezone of an nPod or SPU
    /// </summary>
    public sealed class SetNPodTimeZoneInput
    {
        /// <summary>
        /// The time zone to set
        /// </summary>
        [JsonPath("$.timeZone", true)]
        public string Timezone { get; set; }
    }
}