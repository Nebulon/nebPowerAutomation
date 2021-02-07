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
    /// An input object to create a rack
    ///
    /// <para>
    /// Allows the creation of a rack object in nebulon ON. A rack record
    /// allows customers to logically organize their infrastructure by physical
    /// location.
    /// </para>
    /// </summary>
    public sealed class CreateRackInput
    {
        /// <summary>
        /// An optional location for the new rack
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// Name for the new rack
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the new rack
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier for the parent row
        /// </summary>
        [JsonPath("$.rowUUID", false)]
        public Guid RowGuid { get; set; }
    }

    /// <summary>
    /// A rack in a datacenter
    ///
    /// <para>
    /// A rack record allows customers to logically organize their
    /// infrastructure by physical location.
    /// </para>
    /// </summary>
    public class Rack
    {
        /// <summary>
        /// Unique identifier for the rack
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Number of hosts (servers) in the rack
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// Unique identifiers of hosts in the rack
        /// </summary>
        [JsonPath("$.hosts[*].uuid", false)]
        public Guid[] HostGuids { get; set; }

        /// <summary>
        /// An optional location for the rack
        /// </summary>
        [JsonPath("$.location", true)]
        public string Location { get; set; }

        /// <summary>
        /// Name of the rack
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the rack
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier of the parent row
        /// </summary>
        [JsonPath("$.row.uuid", false)]
        public Guid RowGuid { get; set; }
    }

    /// <summary>
    /// A filter object to filter racks.
    ///
    /// <para>
    /// Allows filtering for specific racks in nebulon ON. The filter allows
    /// only one property to be specified. If filtering on multiple properties
    /// is needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public sealed class RackFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public RackFilter And { get; set; }

        /// <summary>
        /// Filter based on rack unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on rack location
        /// </summary>
        [JsonPath("$.location", false)]
        public StringFilter Location { get; set; }

        /// <summary>
        /// Filter based on rack name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public RackFilter Or { get; set; }

        /// <summary>
        /// Filter based on the rack's row unique identifier
        /// </summary>
        [JsonPath("$.rowUUID", false)]
        public GuidFilter RowGuid { get; set; }
    }

    /// <summary>
    /// Paginated rack list object
    ///
    /// <para>
    /// Contains a list of rack objects and information for pagination. By
    /// default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class RackList : PageList<Rack>
    {
    }

    /// <summary>
    /// A sort object for racks
    ///
    /// <para>
    /// Allows sorting racks on common properties. The sort object allows only
    /// one property to be specified.
    /// </para>
    /// </summary>
    public sealed class RackSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update rack properties
    ///
    /// <para>
    /// Allows updating of an existing rack object in nebulon ON. A rack record
    /// allows customers to logically organize their infrastructure by physical
    /// location.
    /// </para>
    /// </summary>
    public sealed class UpdateRackInput
    {
        /// <summary>
        /// A new optional location for the rack
        /// </summary>
        [JsonPath("$.location", false)]
        public string Location { get; set; }

        /// <summary>
        /// The new name of the rack
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// The new note of the rack
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// Unique identifier for a new row for the rack
        /// </summary>
        [JsonPath("$.rowUUID", false)]
        public Guid? RowGuid { get; set; }
    }
}