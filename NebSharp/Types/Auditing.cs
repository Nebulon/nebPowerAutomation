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
    /// Defines the status of a record in the audit log
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// Operation is still ongoing
        /// </summary>
        InProgress,

        /// <summary>
        /// Operation completed successfully
        /// </summary>
        Completed,

        /// <summary>
        /// Operation failed
        /// </summary>
        Failed,
    }

    /// <summary>
    /// Instance of an audit record entry in nebulon ON.
    ///
    /// <para>
    /// An audit record entry represents an action that a user performed in
    /// nebulon ON. Actions include exclusively commands that alter the
    /// configuration of resources in nebulon ON or in your infrastructure.
    /// Queries that read configuration information are not included in
    /// the audit record list.
    /// </para>
    /// </summary>
    public class AuditLogEntry
    {
        /// <summary>
        /// Client application identifier from which the operation was called
        /// </summary>
        [JsonPath("$.clientApp", true)]
        public string ClientApp { get; set; }

        /// <summary>
        /// IP address of the client that invoked the operation
        /// </summary>
        [JsonPath("$.clientIP", true)]
        public string ClientIP { get; set; }

        /// <summary>
        /// Client platform information from which the operation was called
        /// </summary>
        [JsonPath("$.clientPlatform", true)]
        public string ClientPlatform { get; set; }

        /// <summary>
        /// Affected component by the operation
        /// </summary>
        [JsonPath("$.component", true)]
        public string Component { get; set; }

        /// <summary>
        /// Identifier of the affected component
        /// </summary>
        [JsonPath("$.componentID", true)]
        public string ComponentId { get; set; }

        /// <summary>
        /// Error message associated with the operation
        /// </summary>
        [JsonPath("$.error", true)]
        public string Error { get; set; }

        /// <summary>
        /// Completion date and time for the operation
        /// </summary>
        [JsonPath("$.finish", false)]
        public DateTime Finish { get; set; }

        /// <summary>
        /// Identifier of the nPod associated with the operation
        /// </summary>
        [JsonPath("$.nPod.uuid", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Name of the nPod associated with the operation
        /// </summary>
        [JsonPath("$.nPod.name", false)]
        public string NPodName { get; set; }

        /// <summary>
        /// The operation a user executed
        /// </summary>
        [JsonPath("$.operation", true)]
        public string Operation { get; set; }

        /// <summary>
        /// Parameters that were supplied with the operation as a JSON string
        /// </summary>
        [JsonPath("$.parameters", true)]
        public string Parameters { get; set; }

        /// <summary>
        /// The SPU serial number that is associated with the operation
        /// </summary>
        [JsonPath("$.spu.serial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Start date and time for the operation
        /// </summary>
        [JsonPath("$.start", true)]
        public DateTime Start { get; set; }

        /// <summary>
        /// Status of the operation
        /// </summary>
        [JsonPath("$.status", true)]
        public AuditStatus Status { get; set; }

        /// <summary>
        /// The identifier of the user that executed the operation
        /// </summary>
        [JsonPath("$.user.uuid", false)]
        public Guid UserGuid { get; set; }

        /// <summary>
        /// The user name of the user that executed the operation
        /// </summary>
        [JsonPath("$.user.name", false)]
        public string UserName { get; set; }
    }

    /// <summary>
    /// A filter object to filter audit log entries
    ///
    /// Allows filtering for specific audit log entries in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <tt>and_filter</tt> and <tt>or_filter</tt> options to
    /// concatenate multiple filters.
    /// </summary>
    public class AuditLogFilter
    {
        /// <summary>
        /// Filter for records that match the specified component type
        /// </summary>
        [JsonPath("$.component", false)]
        public string Component { get; set; }

        /// <summary>
        /// Filter for records that match the specified nPod UUID
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Filter for records that match the specified operation name
        /// </summary>
        [JsonPath("$.operation", false)]
        public string Operation { get; set; }

        /// <summary>
        /// Filter for records that match the specified SPU serial number
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Filter for records created after the specified date and time
        /// </summary>
        [JsonPath("$.startAfter", false)]
        public DateTime StartAfter { get; set; }

        /// <summary>
        /// Filter for records created before the specified date and time
        /// </summary>
        [JsonPath("$.startBefore", false)]
        public DateTime StartBefore { get; set; }

        /// <summary>
        /// Filter for records that match the specified operation status
        /// </summary>
        [JsonPath("$.status", false)]
        public AuditStatus Status { get; set; }
    }

    /// <summary>
    /// Paginated audit record list object.
    ///
    /// <para>
    /// Contains a list of audit records and information for pagination.
    /// By default a single page includes a maximum of <c>100</c> items
    /// unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>more</c> as per
    /// default the server does not return the full list of alerts but
    /// only one page.
    /// </para>
    /// </summary>
    public class AuditLogList
    {
        /// <summary>
        /// List of audit log entries in the pagination list
        /// </summary>
        [JsonPath("$.items", true)]
        public AuditLogEntry[] Items { get; set; }

        /// <summary>
        /// Indicates if there are more items on the server
        /// </summary>
        [JsonPath("$.more", true)]
        public bool More { get; set; }
    }
}