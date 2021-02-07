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
    /// Indicates the type of support case
    /// </summary>
    public enum SupportCaseIssueType
    {
        /// <summary>
        /// Customer has a question
        /// </summary>
        Question,

        /// <summary>
        /// Suport case is related to a hardware issue
        /// </summary>
        Hardware,

        /// <summary>
        /// Support case is related to a software issue
        /// </summary>
        Software,

        /// <summary>
        /// Customer has a feature request
        /// </summary>
        FeatureRequest,

        /// <summary>
        /// Support case issue type is not known
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// Indicates the customer specified priority for the support case
    /// </summary>
    public enum SupportCasePriority
    {
        /// <summary>
        /// High urgency
        /// </summary>
        High,

        /// <summary>
        /// Medium urgency
        /// </summary>
        Medium,

        /// <summary>
        /// Low urgency
        /// </summary>
        Low,
    }

    /// <summary>
    /// Indicates the status of the support case
    /// </summary>
    public enum SupportCaseStatus
    {
        /// <summary>
        /// A new support case
        /// </summary>
        New,

        /// <summary>
        /// Work is pending information from the customer
        /// </summary>
        Pending,

        /// <summary>
        /// Customer support is actively working the support case
        /// </summary>
        Working,

        /// <summary>
        /// The support case is escalated with customer support
        /// </summary>
        Escalated,

        /// <summary>
        /// The support case was resolved
        /// </summary>
        Closed,
    }

    /// <summary>
    /// An input object to create a new support case
    ///
    /// <para>
    /// Allows creation of a support case in nebulon ON. A support case allows
    /// customers to get their issues associated with nebulon Cloud-Defined Storage
    /// to be resolved with their preferred support channel. Issues may include
    /// infrastructure and hardware issues, software issues, or questions.
    /// </para>
    /// <para>
    /// Depending on the type of support case the required parameters change.
    /// At a minimum, customers need to supply a <c>subject</c> that describes the
    /// high level summary of the issue, a <c>description</c> that details their
    /// specific problem, a <c>priority</c> to indicate the urgency of the request,
    /// and the <c>issue_type</c> to better route the support case to the appropriate
    /// subject matter expert.
    /// </para>
    /// <para>
    /// If the issue is related to a specific services processing unit (SPU) or
    /// resource in nebulon ON or in the customer's datacenter, <c>spu_serial</c>,
    /// <c>resource_type</c>, and <c>resource_id</c> shall be specified.
    /// </para>
    /// </summary>
    public class CreateSupportCaseInput
    {
        /// <summary>
        /// Detailed description of the issue that requires resolution
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }

        /// <summary>
        /// The type of issue. If the issue is not clearly  identifiable,
        /// use <c>SupportCaseIssueType.Unknown</c>.
        /// </summary>
        [JsonPath("$.issueType", true)]
        public SupportCaseIssueType IssueType { get; set; }

        /// <summary>
        /// The urgency of the request
        /// </summary>
        [JsonPath("$.priority", true)]
        public SupportCasePriority Priority { get; set; }

        /// <summary>
        /// The unique identifier of the resource related to the support case.
        /// If <c>resourceType</c> is specified, also this parameter should be supplied.
        /// </summary>
        [JsonPath("$.resourceID", false)]
        public string ResourceId { get; set; }

        /// <summary>
        /// The type of resource related to the support case
        /// </summary>
        [JsonPath("$.resourceType", false)]
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// The serial number of an SPU related to the support case.
        /// </summary>
        [JsonPath("$.spuSerial", false)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// High level summary of an issue
        /// </summary>
        [JsonPath("$.subject", true)]
        public string Subject { get; set; }
    }

    /// <summary>
    /// A support case object in nebulon ON.
    ///
    /// <para>
    /// A support case is used by customers to have their issues with nebulon
    /// infrastructure resolved. Issues may include infrastructure and hardware
    /// issues, software issues, or general questions.
    /// </para>
    /// </summary>
    public class SupportCase
    {
        /// <summary>
        /// Unique identifier of the associated alert for the support case
        /// </summary>
        [JsonPath("$.alertID", true)]
        public string AlertId { get; set; }

        /// <summary>
        /// List of attachments for the support case
        /// </summary>
        [JsonPath("$.attachments", false)]
        public SupportCaseAttachment[] Attachments { get; set; }

        /// <summary>
        /// Date and time when the support case / issue was resolved
        /// </summary>
        [JsonPath("$.closedDate", false)]
        public DateTime ClosedDate { get; set; }

        /// <summary>
        /// List of comments for the support case
        /// </summary>
        [JsonPath("$.comments", false)]
        public SupportCaseComment[] Comments { get; set; }

        /// <summary>
        /// The customer contact for the support case
        /// </summary>
        [JsonPath("$.contact", true)]
        public SupportCaseContact Contact { get; set; }

        /// <summary>
        /// Date and time when the support case was created
        /// </summary>
        [JsonPath("$.createdDate", true)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Detailed description of the support case / issue
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }

        /// <summary>
        /// Customer feedback for improving future requests
        /// </summary>
        [JsonPath("$.improvementSuggestion", true)]
        public string ImprovementSuggestion { get; set; }

        /// <summary>
        /// Type of issue
        /// </summary>
        [JsonPath("$.issueType", true)]
        public SupportCaseIssueType IssueType { get; set; }

        /// <summary>
        /// Knowledge Base article related to this support case
        /// </summary>
        [JsonPath("$.kbLink", true)]
        public string KnowledgeBaseLink { get; set; }

        /// <summary>
        /// Support case number
        /// </summary>
        [JsonPath("$.number", true)]
        public string Number { get; set; }

        /// <summary>
        /// Support case number with the server vendor
        /// </summary>
        [JsonPath("$.oemCaseNumber", true)]
        public string OemCaseNumber { get; set; }

        /// <summary>
        /// Date and time of support case creation with the server vendor
        /// </summary>
        [JsonPath("$.oemCreatedDate", false)]
        public DateTime OemCreatedDate { get; set; }

        /// <summary>
        /// Name of the server vendor associated with the infrastructure
        /// </summary>
        [JsonPath("$.oemName", true)]
        public string OemName { get; set; }

        /// <summary>
        /// Date and time of last update with the server vendor
        /// </summary>
        [JsonPath("$.oemUpdatedDate", false)]
        public DateTime OemUpdatedDate { get; set; }

        /// <summary>
        /// The email of the case owner working the support case in support
        /// </summary>
        [JsonPath("$.ownerEmail", true)]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// The case owner working the support case in support
        /// </summary>
        [JsonPath("$.ownerName", true)]
        public string OwnerName { get; set; }

        /// <summary>
        /// Urgency of the support case
        /// </summary>
        [JsonPath("$.priority", true)]
        public SupportCasePriority Priority { get; set; }

        /// <summary>
        /// Unique identifier of the associated resource for the support case
        /// </summary>
        [JsonPath("$.resourceID", true)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Associated resource type for the support case
        /// </summary>
        [JsonPath("$.resourceType", true)]
        public string ResourceType { get; set; }

        /// <summary>
        /// Serial number of the associated SPU for the support case
        /// </summary>
        [JsonPath("$.spuSerial", true)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Status of the support case
        /// </summary>
        [JsonPath("$.status", true)]
        public SupportCaseStatus Status { get; set; }

        /// <summary>
        /// High level summary of the support case / issue
        /// </summary>
        [JsonPath("$.subject", true)]
        public string Subject { get; set; }

        /// <summary>
        /// Date and time when the support case was last updated
        /// </summary>
        [JsonPath("$.updatedDate", false)]
        public DateTime UpdatedDate { get; set; }
    }

    /// <summary>
    /// A file attachment to a support case.
    ///
    /// <para>
    /// Allows customers to attach arbitrary data to a support case. Examples are
    /// screenshots of the user interface, log files from application servers, or
    /// other supporting data for resolving a support case.
    /// </para>
    /// </summary>
    public class SupportCaseAttachment
    {
        /// <summary>
        /// A link to the file where it is uploaded
        /// </summary>
        [JsonPath("$.fileLink", true)]
        public string FileLink { get; set; }

        /// <summary>
        /// The name of the uploaded file
        /// </summary>
        [JsonPath("$.fileName", true)]
        public string FileName { get; set; }

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        [JsonPath("$.fileSizeBytes", true)]
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// The unique identifier of the uploaded file
        /// </summary>
        [JsonPath("$.uniqueID", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// The date and time of upload
        /// </summary>
        [JsonPath("$.uploadTime", true)]
        public DateTime UploadTime { get; set; }
    }

    /// <summary>
    /// A comment in the support case history.
    ///
    /// <para>
    /// Allows interaction between the customer and support for further
    /// clarification of issues or providing support case status updates. Customers
    /// and support can add comments to a support case for bi-directional
    /// communication.
    /// </para>
    /// </summary>
    public class SupportCaseComment
    {
        /// <summary>
        /// The text contents of the comment
        /// </summary>
        [JsonPath("$.text", true)]
        public string Text { get; set; }

        /// <summary>
        /// The date and time when the comment was published
        /// </summary>
        [JsonPath("$.time", true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// The name of the user that published the comment
        /// </summary>
        [JsonPath("$.name", true)]
        public string UserName { get; set; }
    }

    /// <summary>
    /// Represents the user contact for a support case.
    ///
    /// <para>
    /// The support case contact is used by support to work on resolving an issue.
    /// By default the contact for a support case is the user that created the
    /// support case, but can be altered.
    /// </para>
    /// </summary>
    public class SupportCaseContact
    {
        /// <summary>
        /// The unique identifier of the contact
        /// </summary>
        [JsonPath("$.contactID", true)]
        public Guid ContactGuid { get; set; }

        /// <summary>
        /// The email address of the contact
        /// </summary>
        [JsonPath("$.email", true)]
        public string Email { get; set; }

        /// <summary>
        /// The mobile phone number of the contact
        /// </summary>
        [JsonPath("$.mobile", true)]
        public string Mobile { get; set; }

        /// <summary>
        /// The name of the contact
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// The phone number of the contact
        /// </summary>
        [JsonPath("$.phone", true)]
        public string Phone { get; set; }
    }

    /// <summary>
    /// A filter object to filter support cases.
    ///
    /// <para>
    /// Allows filtering for specific support cases in nebulon ON. The
    /// filter allows only one property to be specified.
    /// </para>
    /// </summary>
    public class SupportCaseFilter
    {
        /// <summary>
        /// Filter based on the support case contact
        /// </summary>
        [JsonPath("$.contactID", false)]
        public Guid ContactGuid { get; set; }

        /// <summary>
        /// Filter based on the support case type
        /// </summary>
        [JsonPath("$.status", false)]
        public SupportCaseIssueType IssueType { get; set; }

        /// <summary>
        /// Filter based on the support case number
        /// </summary>
        [JsonPath("$.number", false)]
        public string Number { get; set; }

        /// <summary>
        /// Filter based on the support case status
        /// </summary>
        [JsonPath("$.status", false)]
        public SupportCaseStatus Status { get; set; }
    }

    /// <summary>
    /// Paginated support case list object.
    ///
    /// <para>
    /// Contains a list of support case objects and information for
    /// pagination.By default a single page includes a maximum of <c>100</c> items
    /// unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>more</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public class SupportCaseList : PageList<SupportCase>
    {
    }

    /// <summary>
    /// A sort object for support cases
    ///
    /// <para>
    /// Allows sorting support cases on common properties. The sort object allows
    /// only one property to be specified.
    /// </para>
    /// </summary>
    public class SupportCaseSort
    {
        /// <summary>
        /// Sort direction for the <c>CreatedDate</c> property of a support case
        /// </summary>
        [JsonPath("$.createdDate", false)]
        public SortDirection CreatedDate { get; set; }

        /// <summary>
        /// Sort direction for the <c>IssueType</c> property of a support case
        /// </summary>
        [JsonPath("$.issueType", false)]
        public SortDirection IssueType { get; set; }

        /// <summary>
        /// Sort direction for the <c>Status</c> property of a support case
        /// </summary>
        [JsonPath("$.status", false)]
        public SortDirection Status { get; set; }

        /// <summary>
        /// Sort direction for the <c>UpdatedDate</c> property of a support case
        /// </summary>
        [JsonPath("$.updatedDate", false)]
        public SortDirection UpdatedDate { get; set; }
    }

    /// <summary>
    /// An input object to update an existing support case.
    ///
    /// <para>
    /// Allows updating of a support case in nebulon ON. A support case allows
    /// customers to get their issues associated with nebulon Cloud-Defined Storage
    /// to be resolved with their preferred support channel. Issues may include
    /// infrastructure and hardware issues, software issues, or questions.
    /// </para>
    /// </summary>
    public class UpdateSupportCaseInput
    {
        /// <summary>
        /// A comment to add to the support case history
        /// </summary>
        [JsonPath("$.comment", false)]
        public string Comment { get; set; }

        /// <summary>
        /// The identifier for the user to be contacted for the support case
        /// </summary>
        [JsonPath("$.contactUserUUID", false)]
        public Guid ContactUserGuid { get; set; }

        /// <summary>
        /// Detailed description of the issue that requires resolution
        /// </summary>
        [JsonPath("$.description", false)]
        public string Description { get; set; }

        /// <summary>
        /// Feedback for support for improvement
        /// </summary>
        [JsonPath("$.improvementSuggestion", false)]
        public string ImprovementSuggestion { get; set; }

        /// <summary>
        /// The urgency of the request
        /// </summary>
        [JsonPath("$.priority", false)]
        public SupportCasePriority Priority { get; set; }

        /// <summary>
        /// The new status of the support case
        /// </summary>
        [JsonPath("$.status", false)]
        public SupportCaseStatus Status { get; set; }

        /// <summary>
        /// High level summary of an issue
        /// </summary>
        [JsonPath("$.subject", false)]
        public string Subject { get; set; }
    }
}