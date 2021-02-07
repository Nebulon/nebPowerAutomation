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
    /// A state for IP configuration of a SPU logical network interface
    /// </summary>
    public sealed class IPInfoState
    {
        /// <summary>
        /// List of IPv4 or IPv6 addresses in CIDR format
        /// </summary>
        [JsonPath("$.addresses", true)]
        public string[] Addresses { get; set; }

        /// <summary>
        /// The active LACP transmit rate for the link aggregation
        /// </summary>
        [JsonPath("$.bondLACPTransmitRate", false)]
        public BondLACPTransmitRate BondLACPTransmitRate { get; set; }

        /// <summary>
        /// The active MII monitoring interval in ms for the link aggregation
        /// </summary>
        [JsonPath("$.bondMIIMonitorMilliSeconds", false)]
        public long BondMIIMonitorMilliSeconds { get; set; }

        /// <summary>
        /// The link aggregation mode for the interface
        /// </summary>
        [JsonPath("$.bondMode", true)]
        public BondType BondMode { get; set; }

        /// <summary>
        /// The active transmit hash policy for the link aggregation
        /// </summary>
        [JsonPath("$.bondTransmitHashPolicy", false)]
        public BondTransmitHashPolicy BondTransmitHashPolicy { get; set; }

        /// <summary>
        /// Indicates if DHCP is used for IP addressing
        /// </summary>
        [JsonPath("$.dhcp", true)]
        public bool Dhcp { get; set; }

        /// <summary>
        /// The gateway IP address specified for the interface
        /// </summary>
        [JsonPath("$.gateway", true)]
        public string Gateway { get; set; }

        /// <summary>
        /// Indicates if the interface operates in half-duplex
        /// </summary>
        [JsonPath("$.halfDuplex", true)]
        public bool HalfDuplex { get; set; }

        /// <summary>
        /// The physical address of the interface
        /// </summary>
        [JsonPath("$.interfaceMAC", true)]
        public string InterfaceMAC { get; set; }

        /// <summary>
        /// The names of the physical interfaces for the logical interface
        /// </summary>
        [JsonPath("$.interfaceNames", true)]
        public string[] InterfaceNames { get; set; }

        /// <summary>
        /// Indicates if the network interface speed is locked
        /// </summary>
        [JsonPath("$.lockedSpeed", true)]
        public bool LockedSpeed { get; set; }

        /// <summary>
        /// maximum transfer unit
        /// </summary>
        [JsonPath("$.mtu", true)]
        public long Mtu { get; set; }

        /// <summary>
        /// Indicates the network interface speed
        /// </summary>
        [JsonPath("$.speed", true)]
        public long Speed { get; set; }

        /// <summary>
        /// The name of the switch this interface connects to
        /// </summary>
        [JsonPath("$.switchMAC", true)]
        public string SwitchMAC { get; set; }

        /// <summary>
        /// The name of the switch this interface connects to
        /// </summary>
        [JsonPath("$.switchName", true)]
        public string SwitchName { get; set; }

        /// <summary>
        /// The port identifier of the switch this interface connects to
        /// </summary>
        [JsonPath("$.switchPort", true)]
        public string SwitchPort { get; set; }
    }

    /// <summary>
    /// A network time protocol server
    ///
    /// <para>
    /// NTP servers are used for automatic time configuration on the services
    /// processing unit (SPU).
    /// </para>
    /// </summary>
    public sealed class NTPServer
    {
        /// <summary>
        /// Indicates if the specified NTP server hostname is a NTP pool
        /// </summary>
        [JsonPath("$.pool", true)]
        public bool Pool { get; set; }

        /// <summary>
        /// Indicates if the specified NTP server is the preferred NTP server
        /// </summary>
        [JsonPath("$.prefer", true)]
        public bool Prefer { get; set; }

        /// <summary>
        /// The DNS hostname of the NTP server
        /// </summary>
        [JsonPath("$.serverHostname", true)]
        public string ServerHostname { get; set; }
    }

    /// <summary>
    /// An input object to configure a NTP server
    ///
    /// <para>
    /// NTP servers are used for automatic time configuration on the services
    /// processing unit (SPU). The SPU has default network time servers (NTP)
    /// configured. However, customers can customize them if the default NTP
    /// servers are not accessible or different time settings are required.
    /// </para>
    /// </summary>
    public sealed class NTPServerInput
    {
        /// <summary>
        /// Indicates if the specified NTP server hostname is a NTP pool
        /// </summary>
        [JsonPath("$.pool", false)]
        public bool Pool { get; set; }

        /// <summary>
        /// Indicates if the specified NTP server is the preferred NTP server
        /// </summary>
        [JsonPath("$.prefer", false)]
        public bool Prefer { get; set; }

        /// <summary>
        /// The DNS hostname of the NTP server
        /// </summary>
        [JsonPath("$.serverHostname", true)]
        public string ServerHostname { get; set; }
    }

    /// <summary>
    /// An input object to replace a services processing unit (SPU)
    ///
    /// <para>
    /// The replace services processing unit (SPU) operation is used to
    /// transition the configuration of an old, likely failed, SPU to a new
    /// replacement unit and allows modifying the configuration during the
    /// process.
    /// </para>
    /// </summary>
    public sealed class ReplaceSpuInput
    {
        /// <summary>
        /// Configuration information for the new SPU
        /// </summary>
        [JsonPath("$.newSPUInfo", true)]
        public NPodSpuInput NewSpuInfo { get; set; }

        /// <summary>
        /// The unique identifier of the nPod of the old SPU that is being
        /// replaced
        /// </summary>
        [JsonPath("$.nPodUUID", true)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// The serial number of the old SPU that is being replaced
        /// </summary>
        [JsonPath("$.previousSPUSerial", true)]
        public string PreviousSpuSerial { get; set; }

        /// <summary>
        /// The storage set information for the existing SPU. This information
        /// can be obtained from the active replacement alert and only used to
        /// verify that the correct SPU is selected
        /// </summary>
        [JsonPath("$.ssetUUID", true)]
        public Guid SsetGuid { get; set; }
    }

    /// <summary>
    /// An input object to secure-erase a services processing unit (SPU)
    ///
    /// <para>
    /// The secure erase functionality allows a deep-erase of data stored on
    /// the physical drives attached to the SPU.Only SPUs that are not part of
    /// a nPod can be secure-erased.
    /// </para>
    /// </summary>
    public sealed class SecureEraseSpuInput
    {
        /// <summary>
        /// The serial number of the SPU
        /// </summary>
        [JsonPath("$.spuSerial", true)]
        public string SpuSerial { get; set; }
    }

    /// <summary>
    /// An input object to configure NTP servers
    ///
    /// <para>
    /// NTP servers are used for automatic time configuration on the services
    /// processing unit(SPU). The SPU has default network time servers (NTP)
    /// configured. However, customers can customize them if the default NTP
    /// servers are not accessible or different time settings are required.
    /// </para>
    /// </summary>
    public sealed class SetNTPServerInput
    {
        /// <summary>
        /// The unique identifier of the nPod
        /// </summary>
        [JsonPath("$.podUUID", true)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// List of NTP server configurations that shall be applied to an SPU
        /// </summary>
        [JsonPath("$.servers", true)]
        public NTPServerInput[] Servers { get; set; }

        /// <summary>
        /// The serial number of the services processing unit
        /// </summary>
        [JsonPath("$.spuSerial", true)]
        public string SpuSerial { get; set; }
    }

    /// <summary>
    /// A services processing unit (SPU)
    /// </summary>
    public class Spu
    {
        /// <summary>
        /// Network information for the control interface
        /// </summary>
        [JsonPath("$.controlInterface", false)]
        public IPInfoState ControlInterface { get; set; }

        /// <summary>
        /// Network information for the data interfaces
        /// </summary>
        [JsonPath("$.dataInterfaces", false)]
        public IPInfoState[] DataInterfaces { get; set; }

        /// <summary>
        /// The hardware revision of the SPU
        /// </summary>
        [JsonPath("$.hwRev", true)]
        public string HardwareRevision { get; set; }

        /// <summary>
        /// The unique identifier of the host the SPU is installed in
        /// </summary>
        [JsonPath("$.host.uuid", false)]
        public Guid HostGuid { get; set; }

        /// <summary>
        /// Date and time when the SPU last reported state to nebulon ON
        /// </summary>
        [JsonPath("$.lastReported", true)]
        public DateTime LastReported { get; set; }

        /// <summary>
        /// Number of provisioned LUNs on the SPU
        /// </summary>
        [JsonPath("$.lunCount", true)]
        public long LunCount { get; set; }

        /// <summary>
        /// List of unique identifiers of LUNs provisioned on the SPU
        /// </summary>
        [JsonPath("$.luns[*].uuid", false)]
        public Guid[] LunGuids { get; set; }

        /// <summary>
        /// The services processing unit's nPod identifier
        /// </summary>
        [JsonPath("$.nPod.uuid", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Number of SPUs that can successfully communicate with each other
        /// </summary>
        [JsonPath("$.podMemberCanTalkCount", true)]
        public long NPodMemberCanTalkCount { get; set; }

        /// <summary>
        /// Number of physical drives attached to the SPU
        /// </summary>
        [JsonPath("$.physicalDriveCount", true)]
        public long PhysicalDriveCount { get; set; }

        /// <summary>
        /// List of WWNs for all physical drives attached to the SPU
        /// </summary>
        [JsonPath("$.physicalDrives[*].wwn", false)]
        public string[] PhysicalDriveWwns { get; set; }

        /// <summary>
        /// A integer representation of the reason why a SPU was reset
        /// </summary>
        [JsonPath("$.resetReasonInt", true)]
        public long ResetReasonInt { get; set; }

        /// <summary>
        /// A string representation of the reason why a SPU was reset
        /// </summary>
        [JsonPath("$.resetReasonString", true)]
        public string ResetReasonString { get; set; }

        /// <summary>
        /// The unique serial number of the SPU
        /// </summary>
        [JsonPath("$.serial", true)]
        public string Serial { get; set; }

        /// <summary>
        /// The type of SPU
        /// </summary>
        [JsonPath("$.spuType", true)]
        public string SpuType { get; set; }

        /// <summary>
        /// The configured time zone
        /// </summary>
        [JsonPath("$.timeZone", true)]
        public string Timezone { get; set; }

        /// <summary>
        /// Status message for NTP
        /// </summary>
        /// [JsonPath("$.ntpStatus", true)]
        /// public string NtpStatus { get; set; }
        /// <summary>
        /// Version for UEFI
        /// </summary>
        [JsonPath("$.uefiVersion", true)]
        public string UEFIVersion { get; set; }

        /// <summary>
        /// List of historical updates that were applied to the SPU
        /// </summary>
        [JsonPath("$.updateHistory", true)]
        public UpdateHistory[] UpdateHistory { get; set; }

        /// <summary>
        /// Uptime of the services processing unit in seconds
        /// </summary>
        [JsonPath("$.uptimeSeconds", true)]
        public long UptimeSeconds { get; set; }

        /// <summary>
        /// The version of nebOS that is running on the SPU
        /// </summary>
        [JsonPath("$.version", true)]
        public string Version { get; set; }

        /// <summary>
        /// List of configured NTP servers
        /// </summary>
        /// [JsonPath("$.ntpServers", true)]
        /// public NTPServer[] NtpServers { get; set; }
        /// <summary>
        /// Indicates if the SPU is doing a secure wipe
        /// </summary>
        [JsonPath("$.wiping", true)]
        public bool Wiping { get; set; }
    }

    /// <summary>
    /// A staged custom diagnostics request
    ///
    /// <para>
    /// SPU custom diagnostics requests allows customers to run arbitrary
    /// diagnostic commands on the services processing units as part of
    /// troubleshooting issues during a support case.
    /// </para>
    /// </summary>
    public class SpuCustomDiagnostic
    {
        /// <summary>
        /// The human readable name of the custom diagnostic request
        /// </summary>
        [JsonPath("$.diagnosticName", true)]
        public string DiagnosticName { get; set; }

        /// <summary>
        /// An optional note for the diagnostic request
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Indicates if this request will disappear after execution
        /// </summary>
        [JsonPath("$.onceOnly", true)]
        public bool OnceOnly { get; set; }

        /// <summary>
        /// The unique identifier or the custom diagnostic request
        /// </summary>
        [JsonPath("$.requestUID", true)]
        public Guid RequestGuid { get; set; }

        /// <summary>
        /// The serial number of the SPU on which to run diagnostic
        /// </summary>
        [JsonPath("$.spuSerial", true)]
        public string SpuSerial { get; set; }
    }

    /// <summary>
    /// A filter object to filter services processing units (SPU)
    ///
    /// <para>
    /// Allows filtering for specific SPUs registered in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on
    /// multiple properties is needed, use the <c>And</c> and <c>Or</c> options
    /// to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class SpuFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public SpuFilter And { get; set; }

        /// <summary>
        /// Filter for SPUs that are not in a nPod
        /// </summary>
        [JsonPath("$.notInNPod", false)]
        public bool? NotInNPod { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public SpuFilter Or { get; set; }

        /// <summary>
        /// Filter based on SPU serial number
        /// </summary>
        [JsonPath("$.serial", false)]
        public StringFilter Serial { get; set; }
    }

    /// <summary>
    /// Paginated services processing unit (SPU) list
    ///
    /// <para>
    /// Contains a list of SPU objects and information for pagination. By
    /// default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class SpuList : PageList<Spu>
    {
    }

    /// <summary>
    /// A sort object for services processing units (SPU)
    ///
    /// <para>
    /// Allows sorting SPUs on common properties. The sort object allows only
    /// one property to be specified.
    /// </para>
    /// </summary>
    public sealed class SpuSort
    {
        /// <summary>
        /// Sort direction for the <c>Serial</c> property
        /// </summary>
        [JsonPath("$.serial", false)]
        public SortDirection Serial { get; set; }
    }
}