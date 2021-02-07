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
    /// An input object to create a row
    ///
    /// <para>
    /// Allows the creation of a row object in nebulon ON. A row record allows
    /// customers to logically organize their infrastructure by physical
    /// location.
    /// </para>
    /// </summary>
    public class CreateRowInput
    {
        /// <summary>
        /// An optional location for the new row
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// Name for the new row
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the new row
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier for the parent room
        /// </summary>
        [JsonPath("$.labUUID", true)]
        public Guid RoomGuid { get; set; }
    }

    /// <summary>
    /// An input object to delete a row object
    ///
    /// <para>
    /// Allows additional options when deleting a row. When cascade is
    /// set to <c>true</c> all child resources are deleted with the datacenter
    /// room if no hosts are associated with the datacenter room.
    /// </para>
    /// </summary>
    public class DeleteRowInput
    {
        /// <summary>
        /// If set to <i>true</i> all child objects of the row will also
        /// be deleted. This includes racks. This will also unlink
        /// all affected racks with their hosts.
        /// </summary>
        [JsonPath("$.cascade", true)]
        public bool Cascade { get; set; }
    }

    /// <summary>
    /// A row object
    ///
    /// <para>
    /// A row record allows customers to logically organize their infrastructure
    /// by physical location.
    /// </para>
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Unique identifier of the row
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Number of hosts (servers) in the row
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// An optional location for the row
        /// </summary>
        [JsonPath("$.location", true)]
        public string Location { get; set; }

        /// <summary>
        /// Name of the row
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the row
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Number of racks in the row
        /// </summary>
        [JsonPath("$.rackCount", true)]
        public long RackCount { get; set; }

        /// <summary>
        /// Unique identifiers of racks in the row
        /// </summary>
        [JsonPath("$.racks[*].uuid", false)]
        public Guid[] RackGuids { get; set; }

        /// <summary>
        /// Unique identifier of the parent room
        /// </summary>
        [JsonPath("$.lab.uuid", false)]
        public Guid RoomGuid { get; set; }
    }

    /// <summary>
    /// A filter object to filter rows in a datacenter.
    ///
    /// <para>
    /// Allows filtering for specific rows in nebulon ON. The filter allows
    /// only one property to be specified. If filtering on multiple properties
    /// is needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public class RowFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and")]
        public RowFilter And { get; set; }

        /// <summary>
        /// Filter based on row unique identifier
        /// </summary>
        [JsonPath("$.uuid")]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on location
        /// </summary>
        [JsonPath("$.location")]
        public StringFilter Location { get; set; }

        /// <summary>
        /// Filter based on row name
        /// </summary>
        [JsonPath("$.name")]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or")]
        public RowFilter Or { get; set; }

        /// <summary>
        /// Filter based on the room's row unique identifier
        /// </summary>
        [JsonPath("$.labUUID")]
        public GuidFilter RoomGuid { get; set; }
    }

    /// <summary>
    /// Paginated row list object
    ///
    /// <para>
    /// Contains a list of row objects and information for pagination.
    /// By default a single page includes a maximum of <c>100</c> items
    /// unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class RowList : PageList<Row>
    {
    }

    /// <summary>
    /// A sort object for rows
    ///
    /// <para>
    /// Allows sorting rows on common properties. The sort object allows
    /// only one property to be specified.
    /// </para>
    /// </summary>
    public class RowSort
    {
        /// <summary>
        /// Sort direction for the row name
        /// </summary>
        [JsonPath("$.name")]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update row properties
    ///
    /// <para>
    /// Allows updating of an existing row object in nebulon ON. A row record
    /// allows customers to logically organize their infrastructure by physical
    /// location.
    /// </para>
    /// </summary>
    public class UpdateRowInput
    {
        /// <summary>
        /// An optional location for the new row
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// Name for the new row
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the new row
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier for the parent room
        /// </summary>
        [JsonPath("$.labUUID", false)]
        public Guid? RoomGuid { get; set; }
    }
}