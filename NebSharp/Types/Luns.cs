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
    /// An input object to delete multiple LUNs at once
    /// </summary>
    public sealed class BatchDeleteLunInput
    {
        /// <summary>
        /// List of host identifiers from which LUNs shall be deleted
        /// </summary>
        [JsonPath("$.hostUUIDs", false)]
        public Guid[] HostGuids { get; set; }

        /// <summary>
        /// List of LUN identifiers that shall be deleted
        /// </summary>
        [JsonPath("$.lunUUIDs", false)]
        public Guid[] LunGuids { get; set; }

        /// <summary>
        /// Identifier of the volume from which LUNs shall be deleted
        /// </summary>
        [JsonPath("$.volumeUUID", true)]
        public Guid VolumeGuid { get; set; }
    }

    /// <summary>
    /// An input object to create a LUN for a volume
    ///
    /// <para>
    /// Allows the creation of a LUN for a volume.A LUN is an instance of a
    /// volume export that makes a volume accessible to a host.
    /// </para>
    /// </summary>
    public sealed class CreateLunInput
    {
        /// <summary>
        /// List of host UUIDs that identify the hosts the volume shall be
        /// exported to. This must be provided if <c>SpuSerials</c> is not
        /// provided.
        /// </summary>
        [JsonPath("$.hostUUIDs", false)]
        public Guid[] HostGuids { get; set; }

        /// <summary>
        /// If <c>true</c>, volumes will be exported with ALUA turned off
        /// </summary>
        [JsonPath("$.local", false)]
        public bool? Local { get; set; }

        /// <summary>
        /// An optional LUN ID to assign to the volume export
        /// </summary>
        [JsonPath("$.lunID", false)]
        public long? LunId { get; set; }

        /// <summary>
        /// List of serial numbers of the SPUs from which a volume shall be
        /// exported from
        /// </summary>
        [JsonPath("$.spuSerials", false)]
        public string[] SpuSerials { get; set; }

        /// <summary>
        /// The unique identifier of the volume that shall be made available
        /// to a host
        /// </summary>
        [JsonPath("$.volumeUUID", true)]
        public Guid VolumeGuid { get; set; }
    }

    /// <summary>
    /// A LUN / an export of a volume to a host
    ///
    /// <para>
    /// A LUN is an instance of a volume export that makes a volume accessible
    /// to a host
    /// </para>
    /// </summary>
    public sealed class Lun
    {
        /// <summary>
        /// The unique identifier of the LUN
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// The unique identifier of the host this volume is exported to
        /// </summary>
        [JsonPath("$.host.uuid", false)]
        public Guid HostGuid { get; set; }

        /// <summary>
        /// The LUN ID of the volume export
        /// </summary>
        [JsonPath("$.lunID", true)]
        public long LunId { get; set; }

        /// <summary>
        /// The SPU serial where the LUN is exported from
        /// </summary>
        [JsonPath("$.spu.serial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// The unique identifier of the volume that is exported
        /// </summary>
        [JsonPath("$.volume.uuid", false)]
        public Guid VolumeGuid { get; set; }
    }

    /// <summary>
    /// A filter object to filter LUNs.
    ///
    /// <para>
    /// Allows filtering for specific LUNs.The filter allows only one property to
    /// be specified. If filtering on multiple properties is needed, use the
    /// <c>And</c> and <c>Or</c> options to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class LunFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public LunFilter And { get; set; }

        /// <summary>
        /// Filter based on host unique identifer
        /// </summary>
        [JsonPath("$.hostUUID", false)]
        public GuidFilter HostGuid { get; set; }

        /// <summary>
        /// Filter based on LUN unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter LunGuid { get; set; }

        /// <summary>
        /// Filter based on LUN ID
        /// </summary>
        [JsonPath("$.lunID", false)]
        public IntFilter LunId { get; set; }

        /// <summary>
        /// Filter based on nPod unique identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public GuidFilter NPodGuid { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public LunFilter Or { get; set; }

        /// <summary>
        /// Filter based on SPU serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public StringFilter SpuSerial { get; set; }

        /// <summary>
        /// Filter based on volume unique identifier
        /// </summary>
        [JsonPath("$.volumeUUID", false)]
        public GuidFilter VolumeGuid { get; set; }
    }

    /// <summary>
    /// Pagination list definition for LUN query
    /// </summary>
    public sealed class LunList : PageList<Lun>
    {
    }

    /// <summary>
    /// A sort object for LUNs
    ///
    /// <para>
    /// Allows sorting LUNs on common properties. The sort object allows
    /// only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class LunSort
    {
        /// <summary>
        /// Sort direction for the <c>LunId</c> property of a Lun
        /// </summary>
        [JsonPath("$.lunID", false)]
        public SortDirection LunId { get; set; }
    }
}