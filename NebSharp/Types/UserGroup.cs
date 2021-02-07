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

using System;

namespace NebSharp.Types
{
    /// <summary>
    /// An input object to create a new user group in nebulon ON
    /// </summary>
    public sealed class CreateUserGroupInput
    {
        /// <summary>
        /// The name of the user group"
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the user group
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user group
        /// </summary>
        [JsonPath("$.policyUUIDs", false)]
        public Guid[] PolicyGuids { get; set; }
    }

    /// <summary>
    /// An input object to update properties of a user group in nebulon ON
    /// </summary>
    public sealed class UpdateUserGroupInput
    {
        /// <summary>
        /// The name of the user group"
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the user group
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user group
        /// </summary>
        [JsonPath("$.policyUUIDs", false)]
        public Guid[] PolicyGuids { get; set; }
    }

    /// <summary>
    /// A user group in nebulon ON
    ///
    /// <para>
    /// User groups allow grouping users for more convenient management of
    /// permissions and policies.
    /// </para>
    /// </summary>
    public sealed class UserGroup
    {
        /// <summary>
        /// The unique identifier of the user group in nebulon ON
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// The name of the user group
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the user
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user group
        /// </summary>
        [JsonPath("$.policies[*].uuid", true)]
        public Guid PolicyGuids { get; set; }

        /// <summary>
        /// List of user unique identifiers that are part of the group
        /// </summary>
        [JsonPath("$.users[*].uuid", false)]
        public Guid[] UserGuids { get; set; }
    }

    /// <summary>
    /// A filter object to filter user groups
    ///
    /// <para>
    /// Allows filtering for specific user groups in nebulon ON. The filter
    /// allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class UserGroupFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public UserGroupFilter And { get; set; }

        /// <summary>
        /// Filter based on users unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on user name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public UserGroupFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated user group list
    ///
    /// <para>
    /// Contains a list of user group objects and information for
    /// pagination. By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class UserGroupList : PageList<UserGroup>
    {
    }

    /// <summary>
    /// A sort object for user groups
    ///
    /// <para>
    /// Allows sorting user groups on common properties. The sort object allows
    /// only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class UserGroupSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }
}