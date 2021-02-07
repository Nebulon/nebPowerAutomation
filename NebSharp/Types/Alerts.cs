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
    /// Severity of an alert
    /// </summary>
    public enum AlertSeverity
    {
        /// <summary>
        /// Alert severity is not known
        /// </summary>
        Unknown,

        /// <summary>
        /// Alert severity indicates complete loss of functionality or data
        /// unavailability and requires immediate attention by service owners
        /// </summary>
        Urgent,

        /// <summary>
        /// Alert severity indicates complete loss of functionality or data
        /// unavailability and requires immediate attention by service owners
        /// </summary>
        Critical,

        /// <summary>
        /// Alert severity indicates that there is partial loss of functionality or
        /// partial data unavailability, no redundancy in a service (failure of one
        /// more resource will cause an outage) and requires immediate attention by
        /// service owners
        /// </summary>
        Major,

        /// <summary>
        /// Alert severity indicates that there is no loss in functionality, performance
        /// and stability issues that require attention by service owners
        /// </summary>
        Minor,

        /// <summary>
        /// Alert severity indicates that there is no loss in functionality, only minor
        /// impacts to performance, or cosmetic issues or bugs, not affecting the
        /// customer's ability to use the product
        /// </summary>
        Trivial
    }

    /// <summary>
    /// Status of an alert
    /// </summary>
    public enum AlertStatus
    {
        /// <summary>
        /// Status of the alert is not known
        /// </summary>
        Unknown,

        /// <summary>
        /// The alert is not resolved
        /// </summary>
        Open,

        /// <summary>
        /// The alert is resolved
        /// </summary>
        Closed,
    }

    /// <summary>
    /// Instance of an alert in nebulon ON
    ///
    /// An alert represents an issue in nebulon ON or your infrastructure.
    /// If the alert is reporting a status of ``Open`` it is an active alert in your
    /// environment that may require immediate attention.If it is reporting
    /// ``Closed`` the issue was resolved.
    /// </summary>
    public sealed class Alert
    {
        /// <summary>
        /// Action that can be exectuted from this alert
        /// </summary>
        [JsonPath("$.actionOperation", true)]
        public string ActionOperation { get; set; }

        /// <summary>
        /// Parameters fort he action that can be executed from this alert.
        /// </summary>
        [JsonPath("$.actionParams", true)]
        public string ActionParameters { get; set; }

        /// <summary>
        /// Alert code allows the unique identification of an alert type
        /// </summary>
        [JsonPath("$.code", true)]
        public string Code { get; set; }

        /// <summary>
        /// List of recommended corrective actions that a user can perform
        /// </summary>
        [JsonPath("$.correctiveActions", false)]
        public string[] CorrectiveActions { get; set; }

        /// <summary>
        /// Timestamp for when the alert was detected
        /// </summary>
        [JsonPath("$.createTime", true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Detailed description for the alert
        /// </summary>
        [JsonPath("$.details", true)]
        public string Details { get; set; }

        /// <summary>
        /// Unique identifier of the event (Open or Close)
        /// </summary>
        [JsonPath("$.eventID", false)]
        public string EventId { get; set; }

        /// <summary>
        /// Unique identifier of the incident
        /// </summary>
        [JsonPath("$.incidentID", true)]
        public Guid IncidentGuid { get; set; }

        /// <summary>
        /// Related nPod identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Related resource identifier
        /// </summary>
        [JsonPath("$.resourceID", true)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Related resource name
        /// </summary>
        [JsonPath("$.resourceName", true)]
        public string ResourceName { get; set; }

        /// <summary>
        /// Related resource type
        /// </summary>
        [JsonPath("$.resourceType", true)]
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Severity of the alert
        /// </summary>
        [JsonPath("$.severity", true)]
        public AlertSeverity Severity { get; set; }

        /// <summary>
        /// Related services processing unit serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Status of the alert
        /// </summary>
        [JsonPath("$.status", true)]
        public AlertStatus Status { get; set; }

        /// <summary>
        /// A short summary of the alert
        /// </summary>
        [JsonPath("$.summary", true)]
        public string Summary { get; set; }
    }

    /// <summary>
    /// A filter object to filter alerts.
    ///
    /// Allows filtering for specific alerts in nebulon ON. The
    /// filter allows multiple properties to be specified.
    /// If multiple properties are provided they are concatenated
    /// with a logical ``AND``.
    /// </summary>
    public sealed class AlertFilter
    {
        /// <summary>
        /// Filter for alerts created after the specified date and time
        /// </summary>
        [JsonPath("$.createdAfter", false)]
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Filter for alerts created before the specified date and time
        /// </summary>
        [JsonPath("$.createdBefore", false)]
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Filter for alerts that match the specified resource identifier
        /// </summary>
        [JsonPath("$.resourceID", false)]
        public string ResourceID { get; set; }

        /// <summary>
        /// Filter for alerts that match the specified resource type
        /// </summary>
        [JsonPath("$.resourceType", false)]
        public ResourceType? ResourceType { get; set; }

        /// <summary>
        /// Filter for alerts that match the specified alert severity
        /// </summary>
        [JsonPath("$.severity", false)]
        public AlertSeverity? Severity { get; set; }

        /// <summary>
        /// Filter for alerts that match the specified status
        /// </summary>
        [JsonPath("$.status", false)]
        public AlertStatus? Status { get; set; }
    }

    /// <summary>
    /// Paginated Alert list object
    ///
    /// Contains a list of alert objects and information for pagination.
    /// By default a single page includes a maximum of ``100`` items
    /// unless specified otherwise in the paginated query.
    ///
    /// Consumers should always check for the property ``more`` as per default
    /// the server does not return the full list of alerts but only one page.
    /// </summary>
    public sealed class AlertList : PageList<Alert>
    {
    }
}