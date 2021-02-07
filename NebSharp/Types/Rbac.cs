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
    /// An input object to create a RBAC policy
    ///
    /// <para>
    /// Policies in role-based access control associate RBAC roles with users
    /// and resources in nebulon ON. Scopes can be added and removed from policies,
    /// users and user groups can be added and removed from policies. Nebulon ON
    /// will not allow two policies with the same definition.
    /// </para>
    /// <para>
    /// Scopes are defined through a string with the format
    /// <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c> with varying
    /// length. For example:<br/>
    /// - <c>/nPodGroup/*</c>: scopes the policy to all nPod groups in the
    /// organization<br/>
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/*</c>: scopes the policy to a
    /// specific nPod group in the organization and all nPods in this group.
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c>: scopes the
    /// policy to a specific nPod.
    /// </para>
    /// <para>
    /// User groups are not added and removed from a policy through this API,
    /// but through the users API. Use <c>CreateUserGroup</c>, <c>UpdateUserGroup</c>
    /// for adding and removing user groups to RBAC policies and similarly
    /// <c>CreateUser</c> and <c>UpdateUser</c>.
    /// </para>
    /// </summary>
    public sealed class CreateRbacPolicyInput
    {
        /// <summary>
        /// The RBAC role unique identifier to associate with the policy
        /// </summary>
        [JsonPath("$.roleUUID", true)]
        public Guid RoleGuid { get; set; }

        /// <summary>
        /// List of scope definitions that this policy applies to
        /// </summary>
        [JsonPath("$.scopes", true)]
        public string[] Scopes { get; set; }
    }

    /// <summary>
    /// An input object to create a RBAC role
    ///
    /// <para>
    /// Roles in role-based access control define a set of permissions (rights)
    /// that can be assigned to user groups according to their responsibilities.
    /// Rights can be added and removed if these responsibilities change and
    /// propagate to any user that is associated with a role.
    /// </para>
    /// <para>
    /// Rights are defined through a string with the format
    /// <c>{resource}/{permission}</c>, where the following resources are
    /// available: <c>*</c>, <c>Datacenter</c>, <c>Lab</c>, <c>Audit</c>,
    /// <c>Alert</c>, <c>FirmwareUpdate</c>, <c>UserGroup</c>, <c>nPodGroup</c>,
    /// <c>Volume</c>, <c>PhysicalDrive</c>, <c>User</c>, <c>nPod</c>,
    /// <c>SnapshotScheduleTemplate</c>, <c>SPU</c>, <c>Row</c>, <c>Rack</c>,
    /// <c>nPodTemplate</c>, <c>SnapshotSchedule</c>, <c>Host</c>, <c>LUN</c>,
    /// <c>Webhook</c>
    /// </para>
    /// <para>
    /// The following permissions are available:<br/>
    /// <c>*</c> - everything is permitted<br/>
    /// <c>read</c> - read operations are permitted<br/>
    /// <c>create</c> - create operations are permitted<br/>
    /// <c>update</c> - update operations are permitted<br/>
    /// <c>delete</c> - delete operations are permitted<br/>
    /// <c>execute</c> - execute operations are permitted (used for operations
    /// that do not fall in the above categories)
    /// </para>
    /// <para>
    /// The number and type of permissions and resources may change over time
    /// and users can query the currently available resources and permissions
    /// with the <c>GetMetadata</c> query.
    /// </para>
    /// </summary>
    public sealed class CreateRbacRoleInput
    {
        /// <summary>
        /// Description of the RBAC role and associated rights
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }

        /// <summary>
        /// Human readable name for the RBAC role
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// List of rights definitions for the RBAC role
        /// </summary>
        [JsonPath("$.rights", true)]
        public string[] Rights { get; set; }
    }

    /// <summary>
    /// A RBAC policy object
    ///
    /// <para>
    /// Policies in role-based access control associate RBAC roles with users
    /// and resources in nebulon ON. Scopes can be added and removed from policies,
    /// users and user groups can be added and removed from policies. Nebulon ON
    /// will not allow two policies with the same definition.
    /// </para>
    /// <para>
    /// Scopes are defined through a string with the format
    /// <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c> with varying
    /// length. For example:<br/>
    /// - <c>/nPodGroup/*</c>: scopes the policy to all nPod groups in the
    /// organization<br/>
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/*</c>: scopes the policy to a
    /// specific nPod group in the organization and all nPods in this group.
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c>: scopes the
    /// policy to a specific nPod.
    /// </para>
    /// <para>
    /// User groups are not added and removed from a policy through this API,
    /// but through the users API. Use <c>CreateUserGroup</c>, <c>UpdateUserGroup</c>
    /// for adding and removing user groups to RBAC policies and similarly
    /// <c>CreateUser</c> and <c>UpdateUser</c>.
    /// </para>
    /// </summary>
    public sealed class RbacPolicy
    {
        /// <summary>
        /// The unique identifier of the RBAC policy
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// The unique identifier of the RBAC role in this policy
        /// </summary>
        [JsonPath("$.role.uuid", true)]
        public Guid RoleGuid { get; set; }

        /// <summary>
        /// List of scope definitions that this policy applies to
        /// </summary>
        [JsonPath("$.scopes", false)]
        public string[] Scopes { get; set; }

        /// <summary>
        /// List of unique identifiers of user groups in the RBAC policy
        /// </summary>
        [JsonPath("$.userGroups.uuid", false)]
        public Guid[] UserGroupGuids { get; set; }

        /// <summary>
        /// List of unique identifiers of explicit users in the RBAC policy
        /// </summary>
        [JsonPath("$.users.uuid", false)]
        public Guid[] UserGuids { get; set; }
    }

    /// <summary>
    /// A filter object to filter RBAC policies
    ///
    /// <para>
    /// Allows filtering for specific RBAC policies in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class RbacPolicyFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public RbacPolicyFilter And { get; set; }

        /// <summary>
        /// Filter based on RBAC policy unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public RbacPolicyFilter Or { get; set; }

        /// <summary>
        /// Filter based on RBAC role unique identifier
        /// </summary>
        [JsonPath("$.roleUUID", false)]
        public GuidFilter RoleGuid { get; set; }
    }

    /// <summary>
    /// Paginated RBAC policy list object
    ///
    /// <para>
    /// Contains a list of RBAC policy objects and information for
    /// pagination.By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class RbacPolicyList : PageList<RbacPolicy>
    {
    }

    /// <summary>
    /// A sort object for RBAC policies
    ///
    /// <para>
    /// Allows sorting RBAC policies on common properties. The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class RbacPolicySort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// A role definition for role-based access control (RBAC)
    ///
    /// <para>
    /// Roles in role-based access control define a set of permissions (rights)
    /// that can be assigned to user groups according to their responsibilities.
    /// Rights can be added and removed if these responsibilities change and
    /// propagate to any user that is associated with a role.
    /// </para>
    /// <para>
    /// Rights are defined through a string with the format
    /// <c>{resource}/{permission}</c>, where the following resources are
    /// available: <c>*</c>, <c>Datacenter</c>, <c>Lab</c>, <c>Audit</c>,
    /// <c>Alert</c>, <c>FirmwareUpdate</c>, <c>UserGroup</c>, <c>nPodGroup</c>,
    /// <c>Volume</c>, <c>PhysicalDrive</c>, <c>User</c>, <c>nPod</c>,
    /// <c>SnapshotScheduleTemplate</c>, <c>SPU</c>, <c>Row</c>, <c>Rack</c>,
    /// <c>nPodTemplate</c>, <c>SnapshotSchedule</c>, <c>Host</c>, <c>LUN</c>,
    /// <c>Webhook</c>
    /// </para>
    /// <para>
    /// The following permissions are available:<br/>
    /// <c>*</c> - everything is permitted<br/>
    /// <c>read</c> - read operations are permitted<br/>
    /// <c>create</c> - create operations are permitted<br/>
    /// <c>update</c> - update operations are permitted<br/>
    /// <c>delete</c> - delete operations are permitted<br/>
    /// <c>execute</c> - execute operations are permitted (used for operations
    /// that do not fall in the above categories)
    /// </para>
    /// <para>
    /// The number and type of permissions and resources may change over time
    /// and users can query the currently available resources and permissions
    /// with the <c>GetMetadata</c> query.
    /// </para>
    /// </summary>
    public sealed class RbacRole
    {
        /// <summary>
        /// Indicates if the RBAC role was user defined
        /// </summary>
        [JsonPath("$.custom", true)]
        public bool Custom { get; set; }

        /// <summary>
        /// Description of the RBAC role and associated rights
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }

        /// <summary>
        /// Unique identifier of the RBAC role
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Human readable name for the RBAC role
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// List of rights definitions for the RBAC role
        /// </summary>
        [JsonPath("$.rights", true)]
        public string[] Rights { get; set; }
    }

    /// <summary>
    /// A filter object to filter RBAC roles
    ///
    /// <para>
    /// Allows filtering for specific RBAC roles in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class RbacRoleFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public RbacRoleFilter And { get; set; }

        /// <summary>
        /// Filter based on RBAC role unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on RBAC role name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public RbacRoleFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated RBAC role list object
    ///
    /// Contains a list of RBAC role objects and information for
    /// pagination. By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    ///
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </summary>
    public sealed class RbacRoleList : PageList<RbacRole>
    {
    }

    /// <summary>
    /// A sort object for RBAC roles
    ///
    /// <para>
    /// Allows sorting RBAC roles on common properties. The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public sealed class RbacRoleSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update RBAC policy properties
    ///
    /// <para>
    /// Policies in role-based access control associate RBAC roles with users
    /// and resources in nebulon ON. Scopes can be added and removed from policies,
    /// users and user groups can be added and removed from policies. Nebulon ON
    /// will not allow two policies with the same definition.
    /// </para>
    /// <para>
    /// Scopes are defined through a string with the format
    /// <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c> with varying
    /// length. For example:<br/>
    /// - <c>/nPodGroup/*</c>: scopes the policy to all nPod groups in the
    /// organization<br/>
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/*</c>: scopes the policy to a
    /// specific nPod group in the organization and all nPods in this group.
    /// - <c>/nPodGroup/{npod_group_uuid}/nPod/{npod_uuid}</c>: scopes the
    /// policy to a specific nPod.
    /// </para>
    /// <para>
    /// User groups are not added and removed from a policy through this API,
    /// but through the users API. Use <c>CreateUserGroup</c>, <c>UpdateUserGroup</c>
    /// for adding and removing user groups to RBAC policies and similarly
    /// <c>CreateUser</c> and <c>UpdateUser</c>.
    /// </para>
    /// </summary>
    public sealed class UpdateRbacPolicyInput
    {
        /// <summary>
        /// List of scope definitions that this policy applies to
        /// </summary>
        [JsonPath("$.scopes", true)]
        public string[] Scopes { get; set; }
    }

    /// <summary>
    /// An input object to update properties a RBAC role
    ///
    /// <para>
    /// Roles in role-based access control define a set of permissions (rights)
    /// that can be assigned to user groups according to their responsibilities.
    /// Rights can be added and removed if these responsibilities change and
    /// propagate to any user that is associated with a role.
    /// </para>
    /// <para>
    /// Rights are defined through a string with the format
    /// <c>{resource}/{permission}</c>, where the following resources are
    /// available: <c>*</c>, <c>Datacenter</c>, <c>Lab</c>, <c>Audit</c>,
    /// <c>Alert</c>, <c>FirmwareUpdate</c>, <c>UserGroup</c>, <c>nPodGroup</c>,
    /// <c>Volume</c>, <c>PhysicalDrive</c>, <c>User</c>, <c>nPod</c>,
    /// <c>SnapshotScheduleTemplate</c>, <c>SPU</c>, <c>Row</c>, <c>Rack</c>,
    /// <c>nPodTemplate</c>, <c>SnapshotSchedule</c>, <c>Host</c>, <c>LUN</c>,
    /// <c>Webhook</c>
    /// </para>
    /// <para>
    /// The following permissions are available:<br/>
    /// <c>*</c> - everything is permitted<br/>
    /// <c>read</c> - read operations are permitted<br/>
    /// <c>create</c> - create operations are permitted<br/>
    /// <c>update</c> - update operations are permitted<br/>
    /// <c>delete</c> - delete operations are permitted<br/>
    /// <c>execute</c> - execute operations are permitted (used for operations
    /// that do not fall in the above categories)
    /// </para>
    /// <para>
    /// The number and type of permissions and resources may change over time
    /// and users can query the currently available resources and permissions
    /// with the <c>GetMetadata</c> query.
    /// </para>
    /// </summary>
    public sealed class UpdateRbacRoleInput
    {
        /// <summary>
        /// Description of the RBAC role and associated rights
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }

        /// <summary>
        /// Human readable name for the RBAC role
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// List of rights definitions for the RBAC role
        /// </summary>
        [JsonPath("$.rights", true)]
        public string[] Rights { get; set; }
    }
}