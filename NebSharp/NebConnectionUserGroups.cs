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
        /// Allows creating a new user group in nebulon ON
        /// </summary>
        /// <param name="name">
        /// The name of the user group
        /// </param>
        /// <param name="policyGuids">
        /// List of RBAC policies that shall be assigned to the user group
        /// </param>
        /// <param name="note">
        /// An optional note for the user
        /// </param>
        /// <returns></returns>
        public UserGroup CreateUserGroup(string name, Guid[] policyGuids, string note)
        {
            CreateUserGroupInput input = new CreateUserGroupInput();
            input.Name = name;
            input.PolicyGuids = policyGuids;
            input.Note = note;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<UserGroup>(@"createOrgUserGroup", parameters);
        }

        /// <summary>
        /// Allows deletion of a user group
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the user group that should be deleted
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteNebUserGroup(Guid guid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"uuid", guid, false);

            return RunMutation<bool>(@"deleteOrgUserGroup", parameters);
        }

        /// <summary>
        /// Get the number of user groups that match the specified filter
        /// </summary>
        /// <param name="filter">
        /// A filter object to filter the user group objects on the server. If
        /// omitted, the server will count all objects.
        /// </param>
        /// <returns>The number of user groups matching the filter</returns>
        public long GetNebUserGroupCount(UserGroupFilter filter = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("filter", filter, true);

            return RunQuery<long>(@"getUserGroupsCount", parameters);
        }

        /// <summary>
        /// Retrieves a list of user group objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the user group objects on the server. If
        /// omitted, the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the user group objects on supported
        /// properties. If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns></returns>
        public UserGroupList GetUserGroups(
            PageInput page = null,
            UserGroupFilter filter = null,
            UserGroupSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<UserGroupList>(@"getUserGroups", parameters);
        }

        /// <summary>
        /// Allow updating properties of an existing user group
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the user group to update
        /// </param>
        /// <param name="name">
        /// New name for the user group
        /// </param>
        /// <param name="policyGuids">
        /// List of RBAC policies that shall be assigned to the user group
        /// </param>
        /// <param name="note">
        /// An optional note for the user
        /// </param>
        /// <returns></returns>
        public UserGroup UpdateUserGroup(
            Guid guid,
            string name = null,
            Guid[] policyGuids = null,
            string note = null)
        {
            UpdateUserGroupInput input = new UpdateUserGroupInput();
            input.Name = name;
            input.PolicyGuids = policyGuids;
            input.Note = note;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);
            parameters.Add("input", input, false);

            return RunMutation<UserGroup>(@"updateOrgUserGroup", parameters);
        }
    }
}