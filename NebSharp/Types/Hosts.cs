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
    /// A memory DIMM object
    /// </summary>
    public class Dimm
    {
        /// <summary>
        /// Location of the DIMM in the server
        /// </summary>
        [JsonPath("$.location", true)]
        public string Location { get; set; }

        /// <summary>
        /// Location of the DIMM in the server
        /// </summary>
        [JsonPath("$.manufacturer", true)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Location of the DIMM in the server
        /// </summary>
        [JsonPath("$.sizeBytes", true)]
        public long SizeBytes { get; set; }
    }

    /// <summary>
    /// A host or server that contains a nebulon SPU
    /// </summary>
    public sealed class Host
    {
        /// <summary>
        /// Board serial number of the host
        /// </summary>
        [JsonPath("$.boardSerial", true)]
        public string BoardSerial { get; set; }

        /// <summary>
        /// Date and time when the host booted
        /// </summary>
        [JsonPath("$.bootTime", true)]
        public DateTime BootTime { get; set; }

        /// <summary>
        /// Chassis serial number of the host
        /// </summary>
        [JsonPath("$.chassisSerial", true)]
        public string ChassisSerial { get; set; }

        /// <summary>
        /// Number of cores of the installed CPUs
        /// </summary>
        [JsonPath("$.cpuCoreCount", true)]
        public long CpuCoreCount { get; set; }

        /// <summary>
        /// Number of installed CPUs in this host
        /// </summary>
        [JsonPath("$.cpuCount", true)]
        public long CpuCount { get; set; }

        /// <summary>
        /// CPU manufacturer of the CPUs installed in this host
        /// </summary>
        [JsonPath("$.cpuManufacturer", true)]
        public string CpuManufacturer { get; set; }

        /// <summary>
        /// CPU clock speed
        /// </summary>
        [JsonPath("$.cpuSpeed", true)]
        public long CpuSpeed { get; set; }

        /// <summary>
        /// Number of threads of the installed CPUs
        /// </summary>
        [JsonPath("$.cpuThreadCount", true)]
        public long CpuThreadCount { get; set; }

        /// <summary>
        /// CPU type of the CPUs installed in this host
        /// </summary>
        [JsonPath("$.cpuType", true)]
        public string CpuType { get; set; }

        /// <summary>
        /// CPU clock speed
        /// </summary>
        [JsonPath("$.dimmCount", true)]
        public long DimmCount { get; set; }

        /// <summary>
        /// List of DIMMs installed in this host
        /// </summary>
        [JsonPath("$.dimms", true)]
        public Dimm[] Dimms { get; set; }

        /// <summary>
        /// Unique identifier of the host
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// IP address of the lights out management address of the host
        /// </summary>
        [JsonPath("$.lomAddress", true)]
        public string LomAddress { get; set; }

        /// <summary>
        /// Hostname of the lights out management address of the host
        /// </summary>
        [JsonPath("$.lomHostname", true)]
        public string LomHostname { get; set; }

        /// <summary>
        /// Manufacturer name for this host
        /// </summary>
        [JsonPath("$.manufacturer", true)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Total amount of memory in bytes
        /// </summary>
        [JsonPath("$.memoryBytes", true)]
        public long MemoryBytes { get; set; }

        /// <summary>
        /// Model of the host
        /// </summary>
        [JsonPath("$.model", true)]
        public string Model { get; set; }

        /// <summary>
        /// Name of the host
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// Optional note for the host
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// The unique identifier of the nPod this host is part of
        /// </summary>
        [JsonPath("$.nPod.uuid", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Unique identifier associated with this host
        /// </summary>
        [JsonPath("$.rack.uuid", false)]
        public Guid RackGuid { get; set; }

        /// <summary>
        /// Number of SPUs installed in this host
        /// </summary>
        [JsonPath("$.spuCount", true)]
        public long SpuCount { get; set; }

        /// <summary>
        /// List of SPU serial numbers that are installed in this host
        /// </summary>
        [JsonPath("$.spus[*].serial", false)]
        public string[] SpuSerials { get; set; }
    }

    /// <summary>
    /// A filter object to filter hosts.
    ///
    /// <para>
    /// Allows filtering for specific hosts in nebulon ON.The filter allows only
    /// one property to be specified. If filtering on multiple properties is
    /// needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public sealed class HostFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical <c>AND</c>
        /// </summary>
        [JsonPath("$.and", false)]
        public HostFilter And { get; set; }

        /// <summary>
        /// Filter based on board serial number
        /// </summary>
        [JsonPath("$.boardSerial", false)]
        public StringFilter BoardSerial { get; set; }

        /// <summary>
        /// Filter based on host chassis serial number
        /// </summary>
        [JsonPath("$.chassisSerial", false)]
        public StringFilter ChassisSerial { get; set; }

        /// <summary>
        /// Filter based on host unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter HostGuid { get; set; }

        /// <summary>
        /// Filter based on host manufacturer name
        /// </summary>
        [JsonPath("$.manufacturer", false)]
        public StringFilter Manufacturer { get; set; }

        /// <summary>
        /// Filter based on host model name
        /// </summary>
        [JsonPath("$.model", false)]
        public StringFilter Model { get; set; }

        /// <summary>
        /// Filter based on host name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Filter based on nPod unique identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public GuidFilter NPodGuid { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical <c>OR</c>
        /// </summary>
        [JsonPath("$.or", false)]
        public HostFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated host list object
    ///
    /// <para>
    /// Contains a list of host objects and information for
    /// pagination.By default a single page includes a maximum of <c>100</c> items
    /// unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>more</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class HostList : PageList<Host>
    {
    }

    /// <summary>
    /// A sort object for hosts
    ///
    /// <para>
    /// Allows sorting hosts on common properties. The sort object allows only one
    /// property to be specified.
    /// </para>
    /// </summary>
    public sealed class HostSort
    {
        /// <summary>
        /// Sort direction for the <c>Manufacturer</c> property of a host object
        /// </summary>
        [JsonPath("$.manufacturer", false)]
        public SortDirection Manufacturer { get; set; }

        /// <summary>
        /// Sort direction for the <c>Model</c> property of a host object
        /// </summary>
        [JsonPath("$.model", false)]
        public SortDirection Model { get; set; }

        /// <summary>
        /// Sort direction for the <c>Name</c> property of a host object
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update host properties.
    ///
    /// <para>
    /// Allows updating of an existing host object in nebulon ON. Only few
    /// properties of a host are user modifiable. Most properties are automatically
    /// populated by the host's services processing unit (SPU).
    /// </para>
    /// </summary>
    public sealed class UpdateHostInput
    {
        /// <summary>
        /// Name of the host (server)
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// Optional note for the host
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Associated unique identifier of a rack associated with the host
        /// </summary>
        [JsonPath("$.rackUUID", false)]
        public Guid RackGuid { get; set; }
    }
}