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
        /// Allows creation of a new RBAC policy
        /// </summary>
        /// <param name="roleGuid">
        /// The RBAC role unique identifier to associate with the policy
        /// </param>
        /// <param name="scopes">
        /// List of scope definitions that this policy applies to. At least one
        /// scope must be provided.
        /// </param>
        /// <returns>The new RBAC policy</returns>
        public RbacPolicy CreateRbacPolicy(
            Guid roleGuid,
            string[] scopes)
        {
            // setup input
            CreateRbacPolicyInput input = new CreateRbacPolicyInput();
            input.RoleGuid = roleGuid;
            input.Scopes = scopes;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<RbacPolicy>(@"createRBACPolicy", parameters);
        }

        /// <summary>
        /// Allows creation of a new RBAC role.
        /// </summary>
        /// <param name="name">
        /// Human readable name for the RBAC role
        /// </param>
        /// <param name="description">
        /// A description that well describes the role and associated rights.
        /// The role description should provide enough clarity so that users
        /// should not have to read individual rights
        /// </param>
        /// <param name="rights">
        /// List of rights definitions. Please review the class description of
        /// options for <c>Resource</c> and <c>Permission</c> in the rights string
        /// that is in the format <c>{resource}/{permission}</c>. Use the
        /// <c>GetMetadata</c> query to retrieve the latest list of options.
        /// </param>
        /// <returns>The new RBAC role</returns>
        public RbacRole CreateRbacRole(
            string name,
            string description,
            string[] rights)
        {
            // setup room input
            CreateRbacRoleInput input = new CreateRbacRoleInput();
            input.Name = name;
            input.Description = description;
            input.Rights = rights;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, false);

            return RunMutation<RbacRole>(@"createRBACRole", parameters);
        }

        /// <summary>
        /// Allows deletion of an RBAC policy object
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the RBAC policy to delete
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteRbacPolicy(Guid guid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);

            return RunMutation<bool>(@"deleteRBACPolicy", parameters);
        }

        /// <summary>
        /// Allows deletion of an RBAC role object
        /// </summary>
        /// <param name="guid">
        /// The unique identifier of the RBAC role to delete
        /// </param>
        /// <returns>If the query was successful</returns>
        public bool DeleteRbacRole(Guid guid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);

            return RunMutation<bool>(@"deleteRBACRole", parameters);
        }

        /// <summary>
        /// Retrieves a list of RBAC policy objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of <c>100</c> items.</param>
        /// <param name="filter">
        /// A filter object to filter the RBAC policies on the server. If omitted,
        /// the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the RBAC policies on supported properties.
        /// If omitted objects are returned in the order as they were created in.
        /// </param>
        /// <returns>A paginated list of RBAC policies.</returns>
        public RbacPolicyList GetRbacPolicies(
            PageInput page = null,
            RbacPolicyFilter filter = null,
            RbacPolicySort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<RbacPolicyList>(@"getRBACPolicies", parameters);
        }

        /// <summary>
        /// Retrieves a list of RBAC role objects
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of `100` items.</param>
        /// <param name="filter">
        /// A filter object to filter the RBAC roles on the server. If omitted,
        /// the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the RBAC roles on supported properties.
        /// If omitted objects are returned in the order as they were created in.
        /// </param>
        /// <returns>A paginated list of RBAC roles</returns>
        public RbacRoleList GetRbacRoles(
            PageInput page = null,
            RbacRoleFilter filter = null,
            RbacRoleSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<RbacRoleList>(@"getRBACRoles", parameters);
        }

        /// <summary>
        /// Allows updating of RBAC policy properties
        /// </summary>
        /// <param name="guid">
        /// The RBAC policy unique identifier to update
        /// </param>
        /// <param name="scopes">
        /// List of scope definitions that this policy applies to. At least one
        /// scope must be provided.
        /// </param>
        /// <returns></returns>
        public RbacPolicy UpdateRbacPolicy(
            Guid guid,
            string[] scopes)
        {
            UpdateRbacPolicyInput input = new UpdateRbacPolicyInput();
            input.Scopes = scopes;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, false);
            parameters.Add("input", input, false);

            return RunMutation<RbacPolicy>(@"updateRBACPolicy", parameters);
        }

        /// <summary>
        /// Allows updating of RBAC role properties
        /// </summary>
        /// <param name="guid">
        /// The RBAC role unique identifier to update
        /// </param>
        /// <param name="name">
        /// Human readable name for the RBAC role
        /// </param>
        /// <param name="description">
        /// A description that well describes the role and associated rights.
        /// The role description should provide enough clarity so that users
        /// should not have to read individual rights
        /// </param>
        /// <param name="rights">
        /// List of rights definitions. Please review the class description of
        /// options for <c>Resource</c> and <c>Permission</c> in the rights string
        /// that is in the format <c>{resource}/{permission}</c>. Use the
        /// <c>GetMetadata</c> query to retrieve the latest list of options.
        /// </param>
        /// <returns>The new RBAC role</returns>
        public RbacRole UpdateRbacRole(
            Guid guid,
            string name = null,
            string description = null,
            string[] rights = null)
        {
            UpdateRbacRoleInput input = new UpdateRbacRoleInput();
            input.Name = name;
            input.Description = description;
            input.Rights = rights;

            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("uuid", guid, true);
            parameters.Add("input", input, false);

            return RunMutation<RbacRole>(@"updateRBACRole", parameters);
        }
    }
}