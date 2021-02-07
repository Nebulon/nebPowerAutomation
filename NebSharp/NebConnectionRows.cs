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
        /// Allows the creation of a row object in nebulon ON. A row record
        /// allows customers to logically organize their infrastructure by
        /// physical location.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// Name for the new row
        /// </param>
        /// <param name="roomGuid">
        /// Unique identifier for the parent room
        /// </param>
        /// <param name="note">
        /// An optional note for the new row
        /// </param>
        /// <param name="location">
        /// An optional location for the new row
        /// </param>
        /// <returns>The new row in a datacenter</returns>
        public Row CreateRow(
            string name,
            Guid roomGuid,
            string note = null,
            string location = null)
        {
            // create input
            CreateRowInput input = new CreateRowInput();
            input.RoomGuid = roomGuid;
            input.Name = name;
            input.Note = note;
            input.Location = location;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();

            return RunMutation<Row>(@"createRow", parameters);
        }

        /// <summary>
        /// Allows deletion of an existing row object
        /// <para>
        /// The deletion of a row is only possible if the row has no hosts
        /// (servers) associated with any child items. By default, deletion of
        /// a row is only allowed when it is not referenced by any racks or if
        /// the <c>cascade</c> parameter is set to <c>true</c>.
        /// </para>
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the row to delete
        /// </param>
        /// <param name="cascade">
        /// If set to <c>true</c> any child items of the row (rack) will
        /// automatically deleted with this request. If set to <c>false</c> or
        /// omitted and the row has child objects, the deletion will fail with
        /// an error.
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteRow(Guid guid, bool cascade)
        {
            // create input
            DeleteRowInput input = new DeleteRowInput();
            input.Cascade = cascade;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid);
            parameters.Add("input", input);

            return RunMutation<bool>(@"deleteRow", parameters);
        }

        /// <summary>
        /// Retrieves a list of row objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the row on the server. If omitted, the
        /// server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the row on supported properties.
        /// If omitted objects are returned in the order as they were created
        /// in.
        /// </param>
        /// <returns>A paginated list of rows in a datacenter</returns>
        public RowList GetRows(
            PageInput page = null,
            RowFilter filter = null,
            RowSort sort = null
        )
        {
            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<RowList>(@"getRows", parameters);
        }

        /// <summary>
        /// Allows updating properties of an existing datacenter room object.
        ///
        /// <para>
        /// At least one property must be specified.
        /// </para>
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the row to update
        /// </param>
        /// <param name="name">
        /// New name for the row
        /// </param>
        /// <param name="roomGuid">
        /// New parent room for the row
        /// </param>
        /// <param name="note">
        /// The new note for the row. For removing the note, provide an empty str.
        /// </param>
        /// <param name="location">
        /// A new optional location for the row
        /// </param>
        /// <returns>The updated datacenter row object</returns>
        public Row Update(
            Guid guid,
            string name = null,
            Guid? roomGuid = null,
            string note = null,
            string location = null)
        {
            // create input
            UpdateRowInput input = new UpdateRowInput();
            input.RoomGuid = roomGuid;
            input.Name = name;
            input.Note = note;
            input.Location = location;

            // prepare parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);
            parameters.Add("input", input, false);

            return RunMutation<Row>(@"updateRow", parameters);
        }
    }
}