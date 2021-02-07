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
    /// An input object to create a datacenter room
    ///
    /// <para>
    /// Allows the creation of a datacenter room object in nebulon ON. A
    /// datacenter room record allows customers to logically organize their
    /// infrastructure by physical location.
    /// </para>
    /// </summary>
    public class CreateRoomInput
    {
        /// <summary>
        /// Unique identifier for the parent datacenter
        /// </summary>
        [JsonPath("$.dataCenterUUID", true)]
        public Guid DataCenterGuid { get; set; }

        /// <summary>
        /// An optional location for the new datacenter room
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// Name for the new datacenter room
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the new datacenter room
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }

    /// <summary>
    /// An input object to delete a datacenter room object
    ///
    /// <para>
    /// Allows additional options when deleting a datacenter room.When cascade
    /// is set to <c>true</c> all child resources are deleted with the datacenter
    /// room if no hosts are associated with the datacenter room.
    /// </para>
    /// </summary>
    public class DeleteRoomInput
    {
        /// <summary>
        /// Indicates that child items shall be deleted with the room
        /// </summary>
        [JsonPath("$.cascade", false)]
        public bool Cascade { get; set; }
    }

    /// <summary>
    /// A datacenter room object
    ///
    /// <para>
    /// A datacenter room record allows customers to logically organize their
    /// infrastructure by physical location.
    /// </para>
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Unique identifier of the datacenter room
        /// </summary>
        [JsonPath("$.datacenter.uuid", false)]
        public Guid DatacenterGuid { get; set; }

        /// <summary>
        /// Number of hosts (servers) in the datacenter room
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// Unique identifier of the parent datacenter
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid LabGuid { get; set; }

        /// <summary>
        /// An optional location for the datacenter room
        /// </summary>
        [JsonPath("$.location", true)]
        public string Location { get; set; }

        /// <summary>
        /// Name of the datacenter room
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the datacenter room
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Number of racks in the datacenter room
        /// </summary>
        [JsonPath("$.rackCount", true)]
        public long RackCount { get; set; }

        /// <summary>
        /// Number of rows in the datacenter room
        /// </summary>
        [JsonPath("$.rowCount", true)]
        public long RowCount { get; set; }

        /// <summary>
        /// Unique identifiers of rows in the datacenter room
        /// </summary>
        [JsonPath("$.rows[*].uuid", false)]
        public Guid[] RowGuids { get; set; }
    }

    /// <summary>
    /// A filter object to filter datacenter rooms
    ///
    /// <para>
    /// Allows filtering for specific datacenters in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public class RoomFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public RoomFilter And { get; set; }

        /// <summary>
        /// Filter based on datacenter unique identifier
        /// </summary>
        [JsonPath("$.datacenterUUID", false)]
        public GuidFilter DatacenterGuid { get; set; }

        /// <summary>
        /// Filter based on datacenter room name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public RoomFilter Or { get; set; }

        /// <summary>
        /// Filter based on datacenter room unique identifiers
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter RoomGuid { get; set; }
    }

    /// <summary>
    /// Paginated datacenter room list object
    ///
    /// <para>
    /// Contains a list of datacenter room objects and information for
    /// pagination.By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public class RoomList : PageList<Room>
    {
    }

    /// <summary>
    /// A sort object for rooms
    ///
    /// <para>
    /// Allows sorting rooms in datacenters on common properties.The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public class RoomSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c>name property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update datacenter room properties
    ///
    /// <para>
    /// Allows updating of an existing datacenter room object in nebulon ON. A
    /// datacenter room record allows customers to logically organize their
    /// infrastructure by physical location.
    /// </para>
    /// </summary>
    public class UpdateRoomInput
    {
        /// <summary>
        /// A new optional location for the datacenter room
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// New name for the datacenter room
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// The new note for the datacenter room
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }
}