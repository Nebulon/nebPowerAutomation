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
using System.Management.Automation;

namespace NebPowerAutomation
{
    /// <summary>
    /// <para type="synopsis">
    /// Creates a new contact input for a datacenter
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebContactInput")]
    [OutputType(typeof(ContactInput))]
    public class NewNebContact : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The prefered communication method for this customer, e.g. email or phone.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public CommunicationMethodType CommunicationMethod { get; set; }

        /// <summary>
        /// <para type="description">
        /// The unique identifier of an existing user in nebulon ON that
        /// is the user contact
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Indicates if this contact is the primary contact for a datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Primary { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            // Instantiate a new NebContactInput from the provided parameters
            ContactInput contactInput = new ContactInput();
            contactInput.UserGuid = Guid;
            contactInput.Primary = Primary.IsPresent;
            contactInput.CommunicationMethod = CommunicationMethod;

            // return it back to the pipeline
            WriteObject(contactInput);
        }
    }
}