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
        /// Allows creation of a new nPod group object
        ///
        /// <para>
        /// Allows creation of a new nPod group object in nebulon ON. A nPod
        /// group allows logical grouping of nPods into security domains. Each
        /// nPod group can receive custom security policies.
        /// </para>
        /// </summary>
        /// <param name="name">
        /// The name of the new nPod group
        /// </param>
        /// <param name="note">
        /// The optional note for the new nPod group.
        /// </param>
        /// <returns>The new nPod group</returns>
        public NPodGroup CreateNPodGroup(string name, string note)
        {
            // setup input object
            CreateNPodGroupInput input = new CreateNPodGroupInput();
            input.Name = name;
            input.Note = note;

            // setup query parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input);

            return RunMutation<NPodGroup>("createNPodGroup", parameters);
        }

        /// <summary>
        /// Allows deleting a nPod group object
        /// </summary>
        /// <param name="nPodGroupGuid">
        /// The unique identifier of the nPod group to delete
        /// </param>
        /// <returns>If the deletion was a success</returns>
        public bool DeleteNPodGroup(Guid nPodGroupGuid)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", nPodGroupGuid, false);

            return RunMutation<bool>("deleteNPodGroup", parameters);
        }

        /// <summary>
        /// Retrieves a list of nPod group objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the items on the server. If omitted, the
        /// server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the objects on supported
        /// properties.If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns>Paginated list of nPod groups</returns>
        public NPodGroupList GetNebNPodGroups(
            PageInput page = null,
            NPodGroupFilter filter = null,
            NPodGroupSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            // this should return exactly one item.
            return RunQuery<NPodGroupList>(@"getNPodGroups", parameters);
        }

        /// <summary>
        /// Retrieves the number of nPod groups matching the filter
        /// </summary>
        /// <param name="filter">
        /// A filter object to filter the items on the server. If omitted, all
        /// items are counted on the server
        /// </param>
        /// <returns>The number of nPod groups matching the filter</returns>
        public long GetNPodGroupCount(NPodGroupFilter filter = null)
        {
            // setup query parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("filter", filter);

            return RunQuery<long>("getNPodGroupCount", parameters);
        }

        /// <summary>
        /// Allows updating new nPod group object properties
        ///
        /// <para>
        /// Allows updating of an existing nPod group object in nebulon ON. A
        /// nPod group allows logical grouping of nPods into security domains.
        /// Each nPod group can receive custom security policies.
        /// </para>
        /// </summary>
        /// <param name="nPodGroupGuid">
        /// The unique identifier of the nPod group to update
        /// </param>
        /// <param name="name">
        /// The new name of the nPod group
        /// </param>
        /// <param name="note">
        /// The new note for the nPod group. For removing the note, provide an
        /// empty string.
        /// </param>
        /// <returns>The updated nPod group</returns>
        public NPodGroup UpdateNPodGroup(
            Guid nPodGroupGuid,
            string name = null,
            string note = null)
        {
            // setup input object
            UpdateNPodGroupInput input = new UpdateNPodGroupInput();
            input.Name = name;
            input.Note = note;

            // setup query parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", nPodGroupGuid);
            parameters.Add("input", input);

            return RunMutation<NPodGroup>("updateNPodGroup", parameters);
        }
    }
}