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
    /// Defines a user's notification preferences
    /// </summary>
    public enum SendNotificationType
    {
        /// <summary>
        /// No email notifications are sent to the user
        /// </summary>
        Disabled,

        /// <summary>
        /// The user will receive email notifications as events are triggered
        /// </summary>
        Instant,

        /// <summary>
        /// The user will receive a daily digest of alerts over the last 24 hours
        /// </summary>
        Daily
    }

    /// <summary>
    /// An input object to create a new user account in nebulon ON
    /// </summary>
    public sealed class CreateUserInput
    {
        /// <summary>
        /// The business phone number of the user
        /// </summary>
        [JsonPath("$.businessPhone", false)]
        public string BusinessPhone { get; set; }

        /// <summary>
        /// The business email address for the user
        /// </summary>
        [JsonPath("$.email", true)]
        public string Email { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [JsonPath("$.firstName", true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Unique identifiers of user groups the user is part of
        /// </summary>
        [JsonPath("$.userGroupUIDs", true)]
        public Guid[] GroupGuids { get; set; }

        /// <summary>
        /// Indicates if the user is marked as inactive / disabled
        /// </summary>
        [JsonPath("$.inactive", false)]
        public bool Inactive { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        [JsonPath("$.lastName", true)]
        public string LastName { get; set; }

        /// <summary>
        /// The mobile phone number of the user
        /// </summary>
        [JsonPath("$.mobilePhone", false)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// An optional note for the user
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [JsonPath("$.password", true)]
        public string Password { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user
        /// </summary>
        [JsonPath("$.policyUUIDs", false)]
        public Guid[] PolicyGuids { get; set; }

        /// <summary>
        /// The user's notification preferences for alerts
        /// </summary>
        [JsonPath("$.sendNotification", false)]
        public SendNotificationType SendNotification { get; set; }

        /// <summary>
        /// The user's time zone
        /// </summary>
        [JsonPath("$.timeZone", false)]
        public string Timezone { get; set; }

        /// <summary>
        /// User name. This is used to login to nebulon ON and must be unique
        /// across all users in nebulon ON.
        /// </summary>
        [JsonPath("$.name", true)]
        public string UserName { get; set; }
    }

    /// <summary>
    /// An input object to update properties of a user in nebulon ON
    /// </summary>
    public sealed class UpdateUserInput
    {
        /// <summary>
        /// The business phone number of the user
        /// </summary>
        [JsonPath("$.businessPhone", false)]
        public string BusinessPhone { get; set; }

        /// <summary>
        /// The business email address for the user
        /// </summary>
        [JsonPath("$.email", false)]
        public string Email { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [JsonPath("$.firstName", false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Unique identifiers of user groups the user is part of
        /// </summary>
        [JsonPath("$.userGroupUIDs", false)]
        public Guid[] GroupGuids { get; set; }

        /// <summary>
        /// Indicates if the user is marked as inactive / disabled
        /// </summary>
        [JsonPath("$.inactive", false)]
        public bool Inactive { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        [JsonPath("$.lastName", false)]
        public string LastName { get; set; }

        /// <summary>
        /// The mobile phone number of the user
        /// </summary>
        [JsonPath("$.mobilePhone", false)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// An optional note for the user
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [JsonPath("$.password", false)]
        public string Password { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user
        /// </summary>
        [JsonPath("$.policyUUIDs", false)]
        public Guid[] PolicyGuids { get; set; }

        /// <summary>
        /// The user's notification preferences for alerts
        /// </summary>
        [JsonPath("$.sendNotification", false)]
        public SendNotificationType SendNotification { get; set; }

        /// <summary>
        /// The user's time zone
        /// </summary>
        [JsonPath("$.timeZone", false)]
        public string Timezone { get; set; }

        /// <summary>
        /// User name. This is used to login to nebulon ON and must be unique
        /// across all users in nebulon ON.
        /// </summary>
        [JsonPath("$.name", false)]
        public string UserName { get; set; }
    }

    /// <summary>
    /// A user in nebulon ON
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// The user's business phone number
        /// </summary>
        [JsonPath("$.businessPhone", true)]
        public string BusinessPhone { get; set; }

        /// <summary>
        /// Indicates if the user has to change the password during next login
        /// </summary>
        [JsonPath("$.changePassword", false)]
        public bool ChangePassword { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonPath("$.email", true)]
        public string Email { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [JsonPath("$.firstName", true)]
        public string FirstName { get; set; }

        /// <summary>
        /// List of user group unique identifiers the user is part of
        /// </summary>
        [JsonPath("$.groups[*].uuid", false)]
        public Guid[] GroupGuids { get; set; }

        /// <summary>
        /// The unique identifier of the user in nebulon ON
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Indicates if the user is marked as inactive. If true, the user can't
        /// log in to nebulon ON.
        /// </summary>
        [JsonPath("$.inactive", true)]
        public bool Inactive { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        [JsonPath("$.lastName", true)]
        public string LastName { get; set; }

        /// <summary>
        /// Mobile phone number of the user
        /// </summary>
        [JsonPath("$.mobilePhone", true)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// An optional note for the user
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// List of RBAC policies associated with the user
        /// </summary>
        [JsonPath("$.policies[*].uuid", false)]
        public Guid[] PolicyGuids { get; set; }

        /// <summary>
        /// The user name. This is used for login and must be unique across all
        /// users in nebulon ON
        /// </summary>
        [JsonPath("$.name", true)]
        public string UserName { get; set; }

        /// <summary>
        /// The user's personal preferences
        /// </summary>
        [JsonPath("$.preferences", false)]
        public UserPreferences UserPreferences { get; set; }
    }

    /// <summary>
    /// A filter object to filter users
    ///
    /// <para>
    /// Allows filtering for specific user objects in nebulon ON. The filter
    /// allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class UserFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public UserFilter And { get; set; }

        /// <summary>
        /// Filter based on user email address
        /// </summary>
        [JsonPath("$.email", false)]
        public StringFilter Email { get; set; }

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
        public UserFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated user list
    ///
    /// <para>
    /// Contains a list of user objects and information for pagination. By
    /// default a single page includes a maximum of <c>100</c> items unless
    /// specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per
    /// default the server does not return the full list of alerts but only one
    /// page.
    /// </para>
    /// </summary>
    public sealed class UserList : PageList<User>
    {
    }

    /// <summary>
    /// Settings and configuration options for a user
    ///
    /// <para>
    /// User preferences define settings and configuration options for
    /// individual user accounts in nebulon ON that are not globally configured.
    /// </para>
    /// </summary>
    public sealed class UserPreferences
    {
        /// <summary>
        /// Specifies the user's preferred date and time formatting
        /// </summary>
        [JsonPath("$.dateFormat", true)]
        public DateFormat DateFormat { get; set; }

        /// <summary>
        /// Specifies if and the rate at which the user receives notifications
        /// </summary>
        [JsonPath("$.sendNotification", true)]
        public SendNotificationType NotificationType { get; set; }

        /// <summary>
        /// Specifies if the user wants capacity values displayed in base2
        /// </summary>
        [JsonPath("$.showBaseTwo", true)]
        public bool ShowBaseTwo { get; set; }

        /// <summary>
        /// Specifies the time zone of the user
        /// </summary>
        [JsonPath("$.timeZone", true)]
        public string Timezone { get; set; }
    }

    /// <summary>
    /// An input object to define user preferences
    ///
    /// <para>
    /// User preferences define settings and configuration options for
    /// individual user accounts in nebulon ON that are not globally
    /// configured.
    /// </para>
    /// </summary>
    public sealed class UserPreferencesInput
    {
        /// <summary>
        /// Specifies the user's preferred date and time formatting
        /// </summary>
        [JsonPath("$.dateFormat", false)]
        public DateFormat DateFormat { get; set; }

        /// <summary>
        /// Specifies if and the rate at which the user receives notifications
        /// </summary>
        [JsonPath("$.sendNotification", false)]
        public SendNotificationType NotificationType { get; set; }

        /// <summary>
        /// Specifies if the user wants capacity values displayed in base2
        /// </summary>
        [JsonPath("$.showBaseTwo", false)]
        public bool ShowBaseTwo { get; set; }

        /// <summary>
        /// Specifies the time zone of the user
        /// </summary>
        [JsonPath("$.timeZone", false)]
        public string Timezone { get; set; }
    }

    /// <summary>
    /// A sort object for users
    ///
    /// <para>
    /// Allows sorting users on common properties. The sort object allows only
    /// one property to be specified.
    /// </para>
    /// </summary>
    public sealed class UserSort
    {
        /// <summary>
        /// Sort direction for the <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }
}