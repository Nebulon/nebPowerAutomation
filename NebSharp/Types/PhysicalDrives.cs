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
    /// An input object to turn on the physical drive locate LED
    ///
    /// <para>
    /// Allows turning on the locate LED of a physical drive identified by WWN
    /// to easily find it in a server.
    /// </para>
    /// </summary>
    public sealed class LocatePhysicalDriveInput
    {
        /// <summary>
        /// Number of seconds after which the locate LED will turn off
        /// </summary>
        [JsonPath("$.durationSeconds", true)]
        public long DurationSeconds { get; set; }

        /// <summary>
        /// The world-wide name of the physical drive
        /// </summary>
        [JsonPath("$.wwn", true)]
        public string Wwn { get; set; }
    }

    /// <summary>
    /// A physical drive
    ///
    /// <para>
    /// Represents a physical drive that is attached to a services processing
    /// unit in a server.
    /// </para>
    /// </summary>
    public class PhysicalDrive
    {
        /// <summary>
        /// The firmware revision of the physical drive
        /// </summary>
        [JsonPath("$.firmwareRev", true)]
        public string FirmwareRevision { get; set; }

        /// <summary>
        /// The interface type of the physical drive
        /// </summary>
        [JsonPath("$.interfaceType", true)]
        public string InterfaceType { get; set; }

        /// <summary>
        /// The media type of the physical drive
        /// </summary>
        [JsonPath("$.mediaType", true)]
        public string MediaType { get; set; }

        /// <summary>
        /// The model of the physical drive
        /// </summary>
        [JsonPath("$.model", true)]
        public string Model { get; set; }

        /// <summary>
        /// The position or slot in the host's storage enclosure
        /// </summary>
        [JsonPath("$.position", true)]
        public long Position { get; set; }

        /// <summary>
        /// The serial number of the physical drive
        /// </summary>
        [JsonPath("$.serial", true)]
        public string Serial { get; set; }

        /// <summary>
        /// The physical capacity of the physical drive in bytes
        /// </summary>
        [JsonPath("$.sizeBytes", true)]
        public long SizeBytes { get; set; }

        /// <summary>
        /// The serial number of the SPU that the physical drive connects to
        /// </summary>
        [JsonPath("$.spu.serial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// The physical drive state.
        ///
        /// <para>Possible values are:<br/>
        /// 1: PdNormal : normal<br/>
        /// 2: PdFailed: failed, can’t handle IO<br/>
        /// 3: PdMissing: device is missing<br/>
        /// 4: PdVacant: data on device has been vacated<br/>
        /// 5: PdIncompatible: device is incompatible to other devices<br/>
        /// </para>
        /// </summary>
        [JsonPath("$.state", true)]
        public long State { get; set; }

        /// <summary>
        /// The physical drive state as a string
        /// </summary>
        [JsonPath("$.stateDisplay", true)]
        public string StateDisplay { get; set; }

        /// <summary>
        /// Indicates if the physical drive is not yet used for data storage
        /// </summary>
        [JsonPath("$.unadmitted", true)]
        public bool Unadmitted { get; set; }

        /// <summary>
        /// The vendor for the physical drive
        /// </summary>
        [JsonPath("$.vendor", true)]
        public string Vendor { get; set; }

        /// <summary>
        /// The unique world-wide name of the physical drive
        /// </summary>
        [JsonPath("$.wwn", true)]
        public string Wwn { get; set; }
    }

    /// <summary>
    /// A filter object to filter physical drives.
    ///
    /// <para>
    /// Allows filtering for specific physical drives in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on
    /// multiple properties is needed, use the <c>And</c> and <c>Or</c> options
    /// to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveFilter
    {
        /// <summary>
        /// Allows specifying more filters. This filter will be evaluated with
        /// a logical "and" operation.
        /// </summary>
        [JsonPath("$.and", false)]
        public PhysicalDriveFilter And { get; set; }

        /// <summary>
        /// Filter by interface type
        /// </summary>
        [JsonPath("$.interfaceType", false)]
        public StringFilter InterfaceType { get; set; }

        /// <summary>
        /// Filter by drive model
        /// </summary>
        [JsonPath("$.model", false)]
        public StringFilter Model { get; set; }

        /// <summary>
        /// Allows specifying more filters. This filter will be evaluated with
        /// a logical "or" operation.
        /// </summary>
        [JsonPath("$.or", false)]
        public PhysicalDriveFilter Or { get; set; }

        /// <summary>
        /// Filter by drive size
        /// </summary>
        [JsonPath("$.sizeBytes", false)]
        public IntFilter SizeBytes { get; set; }

        /// <summary>
        /// Filter by drive vendor
        /// </summary>
        [JsonPath("$.vendor", false)]
        public StringFilter Vendor { get; set; }

        /// <summary>
        /// Filter by WWN
        /// </summary>
        [JsonPath("$.wwn", false)]
        public StringFilter Wwn { get; set; }
    }

    /// <summary>
    /// Paginated physical drive list object
    ///
    /// <para>
    /// Contains a list of datacenter objects and information for
    /// pagination.By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveList : PageList<PhysicalDrive>
    {
    }

    /// <summary>
    /// A sort object for physical drives
    ///
    /// <para>
    /// Allows sorting physical drives on common properties. The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveSort
    {
        /// <summary>
        /// Sort direction for the interface type
        /// </summary>
        [JsonPath("$.interfaceType", false)]
        public SortDirection InterfaceType { get; set; }

        /// <summary>
        /// Sort direction for model
        /// </summary>
        [JsonPath("$.model", false)]
        public SortDirection Model { get; set; }

        /// <summary>
        /// Sort direction for the drive size
        /// </summary>
        [JsonPath("$.sizeBytes", false)]
        public SortDirection SizeBytes { get; set; }

        /// <summary>
        /// Sort direction for drive vendor
        /// </summary>
        [JsonPath("$.vendor", false)]
        public SortDirection Vendor { get; set; }

        /// <summary>
        /// Sort direction for WWN
        /// </summary>
        [JsonPath("$.wwn", false)]
        public SortDirection Wwn { get; set; }
    }

    /// <summary>
    /// An update for a physical drive
    ///
    /// <para>
    /// nebulon ON only reports recommended physical drive updates.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveUpdate
    {
        /// <summary>
        /// The model of the physical drive this update is relevant for
        /// </summary>
        [JsonPath("$.model", true)]
        public string Model { get; set; }

        /// <summary>
        /// The firmware revision this update will install
        /// </summary>
        [JsonPath("$.newFirmwareRev", true)]
        public string NewFirmwareRev { get; set; }

        /// <summary>
        /// The firmware revision this update replaces
        /// </summary>
        [JsonPath("$.oldFirmwareRev", true)]
        public string OldFirmwareRev { get; set; }

        /// <summary>
        /// The unique identifier of the nPod this update is relevant for
        /// </summary>
        [JsonPath("$.nPod.uuid", false)]
        public Guid PodGuid { get; set; }

        /// <summary>
        /// The serial number of the SPU this update is relevant for
        /// </summary>
        [JsonPath("$.spu.serial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// The vendor of the physical drive this update is relevant for
        /// </summary>
        [JsonPath("$.vendor", true)]
        public string Vendor { get; set; }

        /// <summary>
        /// The unique WWN of the physical drive this update is relevant for
        /// </summary>
        [JsonPath("$.wwn", true)]
        public string Wwn { get; set; }
    }

    /// <summary>
    /// A filter object to filter physical drive updates
    ///
    /// <para>
    /// Allows filtering for specific physical drive updates in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on
    /// multiple properties is needed, use the <c>And</c> and <c>Or</c> options
    /// to concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveUpdatesFilter
    {
        /// <summary>
        /// Allows specifying more filters. This filter will be evaluated with
        /// a logical "and" operation.
        /// </summary>
        [JsonPath("$.and", false)]
        public PhysicalDriveUpdatesFilter And { get; set; }

        /// <summary>
        /// Filter by nPod Guid
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public GuidFilter NPodGuid { get; set; }

        /// <summary>
        /// Allows specifying more filters. This filter will be evaluated with
        /// a logical "or" operation.
        /// </summary>
        [JsonPath("$.or", false)]
        public PhysicalDriveUpdatesFilter Or { get; set; }

        /// <summary>
        /// Filter by SPU serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public StringFilter SpuSerial { get; set; }
    }

    /// <summary>
    /// Paginated physical drive update list object
    ///
    /// <para>
    /// Contains a list of physical drive update objects and information for
    /// pagination. By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveUpdatesList : PageList<PhysicalDriveUpdate>
    {
    }

    /// <summary>
    /// A sort object for physical drive updates
    ///
    /// <para>
    /// Allows sorting physical drive updates on common properties. The sort
    /// object allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class PhysicalDriveUpdatesSort
    {
        /// <summary>
        /// Sort by physical drive model
        /// </summary>
        [JsonPath("$.model", false)]
        public SortDirection Model { get; set; }

        /// <summary>
        /// Sort by NPod Guid
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public SortDirection NPodGuid { get; set; }

        /// <summary>
        /// Sort by SPU serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public SortDirection SpuSerial { get; set; }

        /// <summary>
        /// Sort by vendor
        /// </summary>
        [JsonPath("$.vendor", false)]
        public SortDirection Vendor { get; set; }

        /// <summary>
        /// Sort by world wide name
        /// </summary>
        [JsonPath("$.wwn", false)]
        public SortDirection Wwn { get; set; }
    }

    /// <summary>
    /// An input object to update physical drive firmware
    ///
    /// <para>
    /// Allows updating all physical drives in an SPU or nPod to the
    /// recommended drive firmware level.
    /// </para>
    /// </summary>
    public sealed class UpdatePhysicalDriveFirmwareInput
    {
        /// <summary>
        /// Indicates if the user accepts the end user license agreement
        /// </summary>
        [JsonPath("$.acceptEULA", false)]
        public bool AcceptEula { get; set; }

        /// <summary>
        /// The nPod unique identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public Guid? NPodGuid { get; set; }

        /// <summary>
        /// The SPU serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public string SpuSerial { get; set; }
    }
}