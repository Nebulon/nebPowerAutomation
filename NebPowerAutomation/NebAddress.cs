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
using System.Management.Automation;

namespace NebPowerAutomation
{
    /// <summary>
    /// <para type="synopsis">
    /// Creates a new address input for a datacenter
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebAddress")]
    [OutputType(typeof(AddressInput))]
    public class NewNebAddress : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The address field 1
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Address1 { get; set; }

        /// <summary>
        /// <para type="description">
        /// The address field 2
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Address2 { get; set; }

        /// <summary>
        /// <para type="description">
        /// The address field 3
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Address3 { get; set; }

        /// <summary>
        /// <para type="description">
        /// The city
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string City { get; set; }

        /// <summary>
        /// <para type="description">
        /// The country code
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string CountryCode { get; set; }

        /// <summary>
        /// <para type="description">
        /// The house number
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string HouseNumber { get; set; }

        /// <summary>
        /// <para type="description">
        /// The postal code
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string PostalCode { get; set; }

        /// <summary>
        /// <para type="description">
        /// The state or province code
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string StateProvinceCode { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            AddressInput addressInput = new AddressInput();
            addressInput.HouseNumber = HouseNumber;
            addressInput.Address1 = Address1;
            addressInput.Address2 = Address2;
            addressInput.Address3 = Address3;
            addressInput.City = City;
            addressInput.StateProvinceCode = StateProvinceCode;
            addressInput.PostalCode = PostalCode;
            addressInput.CountryCode = CountryCode;

            WriteObject(addressInput);
        }
    }
}