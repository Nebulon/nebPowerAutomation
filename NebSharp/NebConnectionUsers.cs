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
        /// Allows creating a new user in nebulon ON
        /// </summary>
        /// <param name="input">
        /// The configuration for the new user
        /// </param>
        /// <returns>The new user</returns>
        public User CreateUser(CreateUserInput input)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"input", input, false);

            return RunMutation<User>(@"createOrgUser", parameters);
        }

        /// <summary>
        /// Allows deletion of a user account
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the user that should be deleted
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteUser(Guid guid)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"uuid", guid);

            return RunMutation<bool>(@"deleteOrgUser", parameters);
        }

        /// <summary>
        /// Get the number of users that match the specified filter
        /// </summary>
        /// <param name="filter">
        /// A filter object to filter the user objects on the server. If
        /// omitted, the server will count all objects.
        /// </param>
        /// <returns></returns>
        public long GetUserCount(UserFilter filter = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("filter", filter, true);

            return RunQuery<long>(@"getUsersCount", parameters);
        }

        /// <summary>
        /// Retrieves a list of user objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the user objects on the server. If
        /// omitted, the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the user objects on supported
        /// properties. If omitted objects are returned in the order as they
        /// were created in.
        /// </param>
        /// <returns>A paginated list of users</returns>
        public UserList GetUsers(
            PageInput page = null,
            UserFilter filter = null,
            UserSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<UserList>(@"getUsers", parameters);
        }

        /// <summary>
        /// Allow updating properties of an existing user
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the user that should be updated
        /// </param>
        /// <param name="input">
        /// The updates for the user
        /// </param>
        /// <returns></returns>
        public User UpdateUser(Guid guid, UpdateUserInput input)
        {
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"uuid", guid);
            parameters.Add(@"input", input);

            return RunMutation<User>(@"updateOrgUser", parameters);
        }
    }
}