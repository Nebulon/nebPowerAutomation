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
    /// Defines customer communication preferences
    /// </summary>
    public enum CommunicationMethodType
    {
        /// <summary>
        /// Prefer communication via E-Mail
        /// </summary>
        Email,

        /// <summary>
        /// Prefer communication via Phone
        /// </summary>
        Phone
    }

    /// <summary>
    /// An address for a datacenter.
    ///
    /// <para>
    /// This information is used for support cases related to physical equipment
    /// in customer's datacenters and to allow part shipments to the provided
    /// address.
    /// </para>
    /// </summary>
    public sealed class Address
    {
        /// <summary>
        /// Address field 1, typically the street address
        /// </summary>
        [JsonPath("$.address1", true)]
        public string Address1 { get; set; }

        /// <summary>
        /// Address field 2
        /// </summary>
        [JsonPath("$.address2", true)]
        public string Address2 { get; set; }

        /// <summary>
        /// Address field 3
        /// </summary>
        [JsonPath("$.address3", true)]
        public string Address3 { get; set; }

        /// <summary>
        /// City name for the address
        /// </summary>
        [JsonPath("$.city", true)]
        public string City { get; set; }

        /// <summary>
        /// Country code for the address
        /// </summary>
        [JsonPath("$.countryCode", true)]
        public string CountryCode { get; set; }

        /// <summary>
        /// House number and letters for the address
        /// </summary>
        [JsonPath("$.houseNumber", true)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Postal code for the address
        /// </summary>
        [JsonPath("$.postalCode", true)]
        public string PostalCode { get; set; }

        /// <summary>
        /// State or province code for the address
        /// </summary>
        [JsonPath("$.stateProvinceCode", true)]
        public string StateProvinceCode { get; set; }
    }

    /// <summary>
    /// An input object to setup an address for a datacenter.
    ///
    /// <para>
    /// Allows specifying a physical address for a datacenter. This information is
    /// used for support cases related to physical equipment in customer's
    /// datacenters and to allow part shipments to the provided address.
    /// </para>
    /// </summary>
    public sealed class AddressInput
    {
        /// <summary>
        /// Address field 1, typically the street address
        /// </summary>
        [JsonPath("$.address1", true)]
        public string Address1 { get; set; }

        /// <summary>
        /// Address field 2, if n/a provide an empty string
        /// </summary>
        [JsonPath("$.address2", false)]
        public string Address2 { get; set; }

        /// <summary>
        /// Address field 3, if n/a provide an empty string
        /// </summary>
        [JsonPath("$.address3", false)]
        public string Address3 { get; set; }

        /// <summary>
        /// City name for the address
        /// </summary>
        [JsonPath("$.city", true)]
        public string City { get; set; }

        /// <summary>
        /// Country code for the address
        /// </summary>
        [JsonPath("$.countryCode", true)]
        public string CountryCode { get; set; }

        /// <summary>
        /// House number and letters for the address
        /// </summary>
        [JsonPath("$.houseNumber", true)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Postal code for the address
        /// </summary>
        [JsonPath("$.postalCode", true)]
        public string PostalCode { get; set; }

        /// <summary>
        /// State or province code for the address
        /// </summary>
        [JsonPath("$.stateProvinceCode", false)]
        public string StateProvinceCode { get; set; }
    }

    /// <summary>
    /// Contact information for a datacenter.
    ///
    /// <para>
    /// This information is used to contact a customer in case of infrastructure
    /// issues and to send replacement parts.
    /// </para>
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// The business phone number of the contact
        /// </summary>
        [JsonPath("$.businessPhone", true)]
        public string BusinessPhone { get; set; }

        /// <summary>
        /// Indicates the preferred communication method for this contact
        /// </summary>
        [JsonPath("$.communicationMethod", true)]
        public CommunicationMethodType CommunicationMethod { get; set; }

        /// <summary>
        /// The email address of the contact
        /// </summary>
        [JsonPath("$.emailAddress", true)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The first name of the contact
        /// </summary>
        [JsonPath("$.firstName", true)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact
        /// </summary>
        [JsonPath("$.lastName", true)]
        public string LastName { get; set; }

        /// <summary>
        /// The mobile phone number of the contact
        /// </summary>
        [JsonPath("$.mobilePhone", true)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Indicates if this contact is the primary contact for a datacenter
        /// </summary>
        [JsonPath("$.primary", true)]
        public bool Primary { get; set; }

        /// <summary>
        /// The unique identifier of a nebulon ON user account
        /// </summary>
        [JsonPath("$.userUUID", true)]
        public Guid UserGuid { get; set; }
    }

    /// <summary>
    /// An input object to define a datacenter contact.
    ///
    /// <para>
    /// Allows specifying contact information for a data center.This information
    /// is used to contact a customer in case of infrastructure issues and to send
    /// replacement parts.
    /// </para>
    /// </summary>
    public class ContactInput
    {
        /// <summary>
        /// Indicates the preferred communication method for this contact
        /// </summary>
        [JsonPath("$.communicationMethod", true)]
        public CommunicationMethodType CommunicationMethod { get; set; }

        /// <summary>
        /// Indicates if this contact is the primary contact for a datacenter
        /// </summary>
        [JsonPath("$.primary", true)]
        public bool Primary { get; set; }

        /// <summary>
        /// The unique identifier of a nebulon ON user account
        /// </summary>
        [JsonPath("$.userUUID", true)]
        public Guid UserGuid { get; set; }
    }

    /// <summary>
    /// An input object to create a datacenter.
    ///
    /// <para>
    /// Allows the creation of a datacenter object in nebulon ON. A
    /// datacenter record allows customers to logically organize their
    /// infrastructure by physical location and associate address and contact
    /// information with the physical location. This is useful for effective support
    /// case handling and reporting purposes.
    /// </para>
    /// </summary>
    public class CreateDataCenterInput
    {
        /// <summary>
        /// Postal address for the datacenter
        /// </summary>
        [JsonPath("$.address", true)]
        public AddressInput Address { get; set; }

        /// <summary>
        /// List of contacts for the new datacenter. At least one
        /// contact must be provided.Exactly one contact must be marked
        /// as primary.
        /// </summary>
        [JsonPath("$.contacts", true)]
        public ContactInput[] Contacts { get; set; }

        /// <summary>
        /// Name of the datacenter
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the datacenter
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }

    /// <summary>
    /// A datacenter object.
    ///
    /// <para>
    /// A datacenter record allows customers to logically organize their
    /// infrastructure by physical location and associate address and contact
    /// information with the physical location. This is useful for effective support
    /// case handling and reporting purposes.
    /// </para>
    /// </summary>
    public class DataCenter
    {
        /// <summary>
        /// Postal address for the datacenter
        /// </summary>
        [JsonPath("$.address", true)]
        public Address Address { get; set; }

        /// <summary>
        /// List of contacts for the datacenter
        /// </summary>
        [JsonPath("$.contacts", false)]
        public Contact[] Contacts { get; set; }

        /// <summary>
        /// Unique identifier of the datacenter
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Number of hosts in the datacenter
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// Name for the datacenter
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the datacenter
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Number of racks in the datacenter
        /// </summary>
        [JsonPath("$.rackCount", true)]
        public long RackCount { get; set; }

        /// <summary>
        /// Number of rooms in the datacenter
        /// </summary>
        [JsonPath("$.labCount", true)]
        public long RoomCount { get; set; }

        /// <summary>
        /// Unique identifiers of rooms in the datacenter
        /// </summary>
        [JsonPath("$.labs[*].uuid", false)]
        public Guid[] RoomGuids { get; set; }

        /// <summary>
        /// Number of rows in the datacenter
        /// </summary>
        [JsonPath("$.rowCount", true)]
        public long RowCount { get; set; }
    }

    /// <summary>
    /// A filter object to filter datacenters.
    ///
    /// <para>
    /// Allows filtering for specific datacenters in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>and_filter</c> and <c>or_filter</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public class DataCenterFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical <c>AND</c>.
        /// </summary>
        [JsonPath("$.and", false)]
        public DataCenterFilter And { get; set; }

        /// <summary>
        /// Filter based on datacenter unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter DataCenterGuid { get; set; }

        /// <summary>
        /// Filter based on datacenter name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical <c>OR</c>.
        /// </summary>
        [JsonPath("$.or", false)]
        public DataCenterFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated datacenter list object.
    ///
    /// <para>
    /// Contains a list of datacenter objects and information for
    /// pagination. By default a single page includes a maximum of <c>100</c> items
    /// unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>more</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public class DataCenterList : PageList<DataCenter>
    {
    }

    /// <summary>
    /// A sort object for datacenters.
    ///
    /// <para>Allows sorting datacenters on common properties. The sort object
    /// allows only one property to be specified.
    /// </para>
    /// </summary>
    public class DataCenterSort
    {
        /// <summary>
        /// Sort direction for the <c>name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to delete a datacenter object.
    ///
    /// <para>
    /// Allows additional options when deleting a datacenter. When <c>cascade</c> is
    /// set to <c>true</c> all child resources are deleted with the datacenter if
    /// no hosts are associated with the datacenter.
    /// </para>
    /// </summary>
    public class DeleteDataCenterInput
    {
        /// <summary>
        /// If set to <c>true</c> any child resources are deleted with
        /// the datacenter if no hosts are associated with them.
        /// </summary>
        [JsonPath("$.cascade", false)]
        public bool Cascade { get; set; }
    }

    /// <summary>
    /// An input object to update datacenter properties.
    ///
    /// <para>
    /// Allows updating of an existing datacenter object in nebulon ON. A
    /// datacenter record allows customers to logically organize their
    /// infrastructure by physical location and associate address and contact
    /// information with the physical location. This is useful for effective support
    /// case handling and reporting purposes.
    /// </para>
    /// </summary>
    public class UpdateDataCenterInput
    {
        /// <summary>
        /// New postal address for the datacenter
        /// </summary>
        [JsonPath("$.address", false)]
        public AddressInput Address { get; set; }

        /// <summary>
        /// New list of contacts for the datacenter. If provided,
        /// the list of contacts must have at least one contact. Exactly one
        /// contact must be marked as primary.
        /// </summary>
        [JsonPath("$.contacts", false)]
        public ContactInput[] Contacts { get; set; }

        /// <summary>
        /// New name for the datacenter
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// The new note for the datacenter. For removing the note, provide an empty str.
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }
}