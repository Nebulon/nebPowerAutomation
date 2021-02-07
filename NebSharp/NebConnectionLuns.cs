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
using System.Threading;

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Allows creation of a new LUN
        ///
        /// <para>
        /// Allows the creation of a LUN for a volume.A LUN is an instance of a
        /// volume export that makes a volume accessible to a host.
        /// </para>
        /// <para>
        /// At least one host must be specified via <c>HostGuids</c> or
        /// <c>SpuSerials</c> - either one option must be specified but not
        /// both. If the <c>Locak</c> option is provided and set to <c>true</c>
        /// then the volume will be exported with ALUA, otherwise with ALUA
        /// turned off.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// An input definition for the new LUN. Review the CreateLunInput
        /// properties for configuration details
        /// </param>
        /// <returns>The new LUN if successful</returns>
        public Lun CreateLun(CreateLunInput input)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"input", input);

            TokenResponse token = RunMutation<TokenResponse>(@"createLUN", parameters);

            if (!DeliverToken(token))
                throw new Exception("Token delivery failed");

            // wait until the new LUN is reporting in nebulon ON
            Thread.Sleep(TOKEN_WAITTIME_MS);

            // Query for LUN
            LunFilter filter = new LunFilter();
            filter.LunGuid = new GuidFilter();
            filter.LunGuid.MustEqual = token.WaitOn;

            LunList lunList = GetLuns(null, filter, null);

            if (lunList.FilteredCount != 1)
                throw new Exception("Unexpected number of results returned");

            return lunList.Items[0];
        }

        /// <summary>
        /// Allows deletion of a LUN
        /// </summary>
        /// <param name="lunGuid">
        /// The unique identifier of the LUN to delete
        /// </param>
        /// <returns>If the deletion was a success</returns>
        public bool DeleteLun(Guid lunGuid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", lunGuid);

            TokenResponse token = RunMutation<TokenResponse>("deleteLUN", parameters);
            return DeliverToken(token);
        }

        /// <summary>
        /// Allows deletion of multiple LUNs
        /// </summary>
        /// <param name="volumeGuid">
        /// The unique identifier of the volume from which the LUNs shall be
        /// deleted
        /// </param>
        /// <param name="lunGuids">
        /// The list of LUN identifiers that shall be deleted. If <c>hostGuids</c>
        /// is not specified this parameter is mandatory
        /// </param>
        /// <param name="hostGuids">
        /// The list of host identifiers from which the LUNs shall be deleted.
        /// If <c>lunGuids</c> is not specified this parameter is mandatory
        /// </param>
        /// <returns>If the deletion was a success</returns>
        public bool DeleteLuns(
            Guid volumeGuid,
            Guid[] lunGuids = null,
            Guid[] hostGuids = null)
        {
            BatchDeleteLunInput input = new BatchDeleteLunInput
            {
                VolumeGuid = volumeGuid,
                LunGuids = lunGuids,
                HostGuids = hostGuids
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input);

            TokenResponse token = RunMutation<TokenResponse>(@"batchDeleteLUN", parameters);
            return DeliverToken(token);
        }

        /// <summary>
        /// Retrieves a list of LUN objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the LUNs on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the LUN objects
        /// on supported properties.If omitted objects are returned in the
        /// order as they were created in.
        /// </param>
        /// <returns>A paginated list of LUNs</returns>
        public LunList GetLuns(
            PageInput page = null,
            LunFilter filter = null,
            LunSort sort = null)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            // this should return exactly one item.
            return RunQuery<LunList>(@"getLUNs", parameters);
        }
    }
}