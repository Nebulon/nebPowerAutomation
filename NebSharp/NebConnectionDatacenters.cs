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
        /// Allows creation of a new datacenter object.
        ///
        /// <para>
        /// A datacenter record allows customers to logically organize their
        /// infrastructure by physical location and associate address and contact
        /// information with the physical location. This is useful for effective
        /// support case handling and reporting purposes.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// Name for the new datacenter
        /// </param>
        /// <param name="address">
        /// Postal address for the new datacenter
        /// </param>
        /// <param name="contacts">
        /// List of contacts for the new datacenter. At least one
        /// contact must be provided and exactly one contact must have the
        /// <c>Primary</c> property set to <c>true</c>.
        /// </param>
        /// <param name="note">
        /// An optional note for the new datacenter
        /// </param>
        /// <returns>The new datacenter</returns>
        /// <exception cref="Core.NebException">
        /// An error with the GraphQL endpoint
        /// </exception>
        public DataCenter CreateDataCenter(
            string name,
            AddressInput address,
            ContactInput[] contacts,
            string note = null)
        {
            // setup input object
            CreateDataCenterInput input = new CreateDataCenterInput
            {
                Name = name,
                Note = note,
                Address = address,
                Contacts = contacts
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"input", input);

            return RunMutation<DataCenter>(@"createDataCenter", parameters);
        }

        /// <summary>
        /// Allows deletion of an existing datacenter object.
        ///
        /// <para>
        ///  The deletion of a datacenter is only possible if the datacenter has
        ///  no hosts (servers) associated with any child items. By default,
        ///  deletion of a datacenter is only allowed when the datacenter is not
        ///  referenced by any rooms or if the <c>cascade</c> parameter is set
        ///  to <c>true</c>.
        /// </para>
        /// </summary>
        /// <param name="dataCenterGuid">
        /// The unique identifier of the datacenter to delete
        /// </param>
        /// <param name="cascade">
        ///  If set to <c>true</c> any child items of the datacenter (room, row, rack)
        ///  will automatically deleted with this request. If set to <c>false</c>
        ///  or omitted and the datacenter has child objects, the deletion will
        ///  fail with an error.
        /// </param>
        /// <returns>A bool indicating if the deletion was successful</returns>
        public bool DeleteDataCenter(
            Guid dataCenterGuid,
            bool cascade)
        {
            // setup input object
            DeleteDataCenterInput deleteDataCenterInput = new DeleteDataCenterInput();
            deleteDataCenterInput.Cascade = cascade;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", dataCenterGuid);
            parameters.Add("input", deleteDataCenterInput);

            return RunMutation<bool>(@"deleteDataCenter", parameters);
        }

        /// <summary>
        /// Retrieves a list of datacenter objects.
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the datacenters on the
        /// server. If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the datacenter objects
        /// on supported properties. If omitted objects are returned in the
        /// order as they were created in.
        /// </param>
        /// <returns></returns>
        public DataCenterList GetDataCenters(
            PageInput page = null,
            DataCenterFilter filter = null,
            DataCenterSort sort = null
        )
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<DataCenterList>(@"getDataCenters", parameters);
        }

        /// <summary>
        /// Allows updating properties of an existing datacenter object
        /// </summary>
        /// <param name="dataCenterGuid">
        /// The unique identifier of the datacenter to update
        /// </param>
        /// <param name="name">
        /// New name for the datacenter
        /// </param>
        /// <param name="contacts">
        /// New list of contacts for the new datacenter. At least one
        /// contact must be provided and exactly one contact must have the
        /// <c>Primary</c> property set to <c>true</c>.
        /// </param>
        /// <param name="address">
        /// New postal address for the datacenter
        /// </param>
        /// <param name="note">
        /// A new note for the datacenter. For removing the note, provide an empty str
        /// </param>
        /// <returns>The updated datacenter</returns>
        public DataCenter UpdateDataCenter(
            Guid dataCenterGuid,
            string name = null,
            ContactInput[] contacts = null,
            AddressInput address = null,
            string note = null)
        {
            // setup input object
            UpdateDataCenterInput updateDataCenterInput = new UpdateDataCenterInput
            {
                Name = name,
                Contacts = contacts,
                Address = address,
                Note = note
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", dataCenterGuid);
            parameters.Add("input", updateDataCenterInput);

            return RunMutation<DataCenter>(@"updateDataCenter", parameters);
        }
    }
}