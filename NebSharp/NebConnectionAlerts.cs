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

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Retrieves a list of resolved alerts
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <tt>100</tt> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the open alerts on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <exception cref="Core.NebException">
        /// An error with the GraphQL endpoint
        /// </exception>
        /// <returns>A paginated list of resolved alerts</returns>
        public AlertList GetHistoricalAlerts(
            PageInput page = null,
            AlertFilter filter = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);

            return RunQuery<AlertList>(@"getHistoricalAlerts", parameters);
        }

        /// <summary>
        /// Retrieves a list of open alerts
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <tt>100</tt> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the open alerts on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <exception cref="Core.NebException">
        /// An error with the GraphQL endpoint
        /// </exception>
        /// <returns>A paginated list of open alerts</returns>
        public AlertList GetOpenAlerts(
            PageInput page = null,
            AlertFilter filter = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);

            return RunQuery<AlertList>(@"getOpenAlerts", parameters);
        }
    }
}