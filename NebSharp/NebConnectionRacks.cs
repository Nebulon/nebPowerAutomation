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

using NebSharp.Types;
using System;

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Allows creation of a new rack object
        ///
        /// <para>
        /// A rack record allows customers to logically organize their
        /// infrastructure by physical location.
        /// </para>
        /// </summary>
        /// <param name="rowGuid">
        /// Unique identifier for the parent row
        /// </param>
        /// <param name="name">
        /// Name for the new rack
        /// </param>
        /// <param name="note">
        /// An optional note for the new rack
        /// </param>
        /// <param name="location">
        /// An optional location for the new rack
        /// </param>
        /// <returns>The new rack</returns>
        public Rack CreateRack(
            Guid rowGuid,
            string name,
            string note = null,
            string location = null)
        {
            // setup room input
            CreateRackInput input = new CreateRackInput
            {
                RowGuid = rowGuid,
                Name = name,
                Note = note,
                Location = location
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<Rack>(@"createRack", parameters);
        }

        /// <summary>
        /// Allows deletion of an existing rack object
        ///
        /// <para>
        /// The deletion of a rack is only possible if the rack has no hosts
        /// (servers) associated.
        /// </para>
        /// </summary>
        /// <param name="rackGuid">
        /// The unique identifier of the datacenter room to delete
        /// </param>
        /// <returns></returns>
        public bool DeleteRack(Guid rackGuid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", rackGuid, false);

            return RunMutation<bool>(@"deleteRack", parameters);
        }

        /// <summary>
        /// Retrieves a list of rack objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of `100` items.</param>
        /// <param name="filter">
        /// A filter object to filter the racks on the server. If omitted,
        /// the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the racks on supported properties.
        /// If omitted objects are returned in the order as they were created in.
        /// </param>
        /// <returns></returns>
        public RackList GetRacks(
            PageInput page = null,
            RackFilter filter = null,
            RackSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<RackList>(@"getRacks", parameters);
        }

        /// <summary>
        /// Allows updating properties of an existing rack object
        ///
        /// <para>At least one property must be specified</para>
        /// </summary>
        /// <param name="rackGuid">
        /// The unique identifier of the rack to update
        /// </param>
        /// <param name="rowGuid">
        /// New parent row for the rack
        /// </param>
        /// <param name="name">
        /// New name for the rack
        /// </param>
        /// <param name="note">
        /// The new note for the rack. For removing the note, provide an empty
        /// str.
        /// </param>
        /// <param name="location">
        /// A new optional location for the new datacenter room
        /// </param>
        /// <returns>The updated rack object</returns>
        public Rack UpdateRack(
            Guid rackGuid,
            Guid? rowGuid = null,
            string name = null,
            string note = null,
            string location = null)
        {
            // setup room update input
            UpdateRackInput input = new UpdateRackInput
            {
                RowGuid = rowGuid,
                Name = name,
                Note = note,
                Location = location
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", rackGuid, false);
            parameters.Add("input", input, false);

            return RunMutation<Rack>(@"updateRack", parameters);
        }
    }
}