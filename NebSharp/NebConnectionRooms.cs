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
        /// Allows creation of a new datacenter room object
        ///
        /// <para>
        /// A datacenter room record allows customers to logically organize their
        /// infrastructure by physical location.
        /// </para>
        /// </summary>
        /// <param name="dataCenterGuid">
        /// Unique identifier for the parent datacenter
        /// </param>
        /// <param name="name">
        /// Name for the new datacenter room
        /// </param>
        /// <param name="note">
        /// An optional note for the new datacenter room
        /// </param>
        /// <param name="location">
        /// An optional location for the new datacenter room
        /// </param>
        /// <returns>The new datacenter room</returns>
        public Room CreateRoom(
            Guid dataCenterGuid,
            string name,
            string note = null,
            string location = null)
        {
            // setup room input
            CreateRoomInput input = new CreateRoomInput
            {
                DataCenterGuid = dataCenterGuid,
                Name = name,
                Note = note,
                Location = location
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<Room>(@"createLab", parameters);
        }

        /// <summary>
        /// Allows deletion of an existing datacenter room object
        ///
        /// <para>
        /// The deletion of a datacenter room is only possible if the room
        /// has no hosts (servers) associated with any child items. By default,
        /// deletion of a datacenter room is only allowed when it is not
        /// referenced by any rows or if the <c>cascade</c> parameter is set
        /// to <c>true</c>.
        /// </para>
        /// </summary>
        /// <param name="roomGuid">
        /// The unique identifier of the datacenter room to delete
        /// </param>
        /// <param name="cascade">
        /// If set to True any child items of the datacenter room
        /// (row, rack) will automatically deleted with this request. If set
        /// to False or omitted and the datacenter room has child objects, the
        /// deletion will fail with an error.
        /// </param>
        public void DeleteRoom(
            Guid roomGuid,
            bool cascade = false)
        {
            // setup delete input
            DeleteRoomInput input = new DeleteRoomInput
            {
                Cascade = cascade
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", roomGuid, false);
            parameters.Add("input", input, false);

            RunMutation<Room>(@"deleteLab", parameters);
        }

        /// <summary>
        /// Retrieves a list of datacenter room objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of `100` items.</param>
        /// <param name="filter">
        /// A filter object to filter the datacenter rooms on
        /// the server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the datacenter rooms
        /// on supported properties.If omitted objects are returned in the
        /// order as they were created in.
        /// </param>
        /// <returns></returns>
        public RoomList GetRooms(
            PageInput page = null,
            RoomFilter filter = null,
            RoomSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<RoomList>(@"getLabs", parameters);
        }

        /// <summary>
        /// Allows updating properties of an existing datacenter room object
        ///
        /// <para>At least one property must be specified</para>
        /// </summary>
        /// <param name="roomGuid">
        /// The unique identifier of the datacenter room to update
        /// </param>
        /// <param name="name">
        /// New name for the datacenter room
        /// </param>
        /// <param name="note">
        /// The new note for the datacenter room. For removing the
        /// note, provide an empty str
        /// </param>
        /// <param name="location">
        /// A new optional location for the new datacenter room
        /// </param>
        /// <returns></returns>
        public Room UpdateRoom(
            Guid roomGuid,
            string name,
            string note,
            string location)
        {
            // setup room update input
            UpdateRoomInput input = new UpdateRoomInput
            {
                Name = name,
                Note = note,
                Location = location
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", roomGuid, false);
            parameters.Add("input", input, false);

            return RunMutation<Room>(@"updateLab", parameters);
        }
    }
}