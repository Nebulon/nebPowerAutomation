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
        /// Retrieves a list of host objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of `100` items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the hosts on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the host objects
        /// on supported properties.If omitted objects are returned in the
        /// order as they were created in.
        /// </param>
        /// <returns>A paginated list of hosts</returns>
        public HostList GetHosts(
            PageInput page,
            HostFilter filter,
            HostSort sort)
        {
            // setup parameter
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<HostList>(@"getHosts", parameters);
        }

        /// <summary>
        /// Allows updating properties of a host object
        /// </summary>
        /// <param name="hostGuid">
        /// The unique identifier of the host to update
        /// </param>
        /// <param name="input">
        /// An input object to update host properties
        /// </param>
        /// <returns>Updated host</returns>
        public Host UpdateHost(
            Guid hostGuid,
            UpdateHostInput input)
        {
            // setup parameter
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"uuid", hostGuid, false);
            parameters.Add(@"input", input, false);

            return RunMutation<Host>(@"updateHost", parameters);
        }
    }
}