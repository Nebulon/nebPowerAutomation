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
        /// Retrieves a list of audit records
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the audit records on the
        /// server. If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <returns></returns>
        public AuditLogList GetAuditRecords(
            PageInput page = null,
            AuditLogFilter filter = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);

            return RunQuery<AuditLogList>(@"getAuditLog", parameters);
        }
    }
}