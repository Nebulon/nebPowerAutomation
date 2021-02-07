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
        /// Remove a key value entry from a resource
        /// </summary>
        /// <param name="resourceType">Type of resource for the key value data</param>
        /// <param name="nPodGroupGuid">nPod Group identifier</param>
        /// <param name="resourceId">Identifier of the resource for the key value entry</param>
        /// <param name="key">Metadata key</param>
        /// <returns>Indicator if the query was successful</returns>
        public bool DeleteKeyValue(
            ResourceType resourceType,
            Guid nPodGroupGuid,
            string resourceId,
            string key
        )
        {
            // setup input
            DeleteKeyValueInput input = new DeleteKeyValueInput
            {
                ResourceType = resourceType,
                NPodGroupGuid = nPodGroupGuid,
                ResourceId = resourceId,
                Key = key
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"input", input);

            return RunMutation<bool>(@"deleteKeyValue", parameters);
        }

        /// <summary>
        /// Retrieves a list of key value objects
        /// </summary>
        /// <param name="filter">
        /// A filter object to filter key value objects on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <returns>A list of key value objects</returns>
        public KeyValueList GetKeyValues(KeyValueFilter filter)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("filter", filter);

            return RunQuery<KeyValueList>(@"getKeyValues", parameters);
        }

        /// <summary>
        /// Set a key value entry for a resource.
        ///
        /// <para>
        /// Allows adding metadata to various resources in nebulon ON in the form of
        /// key value pairs. This metadata can be used by customers to add arbitrary
        /// text information to resources that are not part of the default resource
        /// properties.
        /// </para>
        /// </summary>
        /// <param name="resourceType">Type of resource for the key value data</param>
        /// <param name="nPodGroupGuid">nPod Group identifier</param>
        /// <param name="resourceId">Identifier of the resource for the key value entry</param>
        /// <param name="key">Metadata key</param>
        /// <param name="value">Metadata value</param>
        /// <returns>Indicator if the query was successful</returns>
        public bool SetKeyValue(
            ResourceType resourceType,
            Guid nPodGroupGuid,
            string resourceId,
            string key,
            string value
        )
        {
            // setup input
            UpsertKeyValueInput input = new UpsertKeyValueInput
            {
                ResourceType = resourceType,
                NPodGroupId = nPodGroupGuid,
                ResourceId = resourceId,
                Key = key,
                Value = value
            };

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"input", input);

            return RunMutation<bool>(@"upsertKeyValue", parameters);
        }
    }
}