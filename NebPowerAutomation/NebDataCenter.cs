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
using System.Collections.Generic;
using System.Management.Automation;

namespace NebPowerAutomation
{
    /// <summary>
    /// <para type="synopsis">
    /// Get a list of existing datacenters and their properties
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebDataCenter")]
    [OutputType(typeof(DataCenter))]
    public class GetNebDataCenters : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter for a datacenter by a specific Guid
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for a datacenter by a specific name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// Set the sort direction for the reurned list
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SortDirection Sort { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            List<DataCenterFilter> filters = new List<DataCenterFilter>();

            if (ParameterPresent("Guid"))
            {
                DataCenterFilter f = new DataCenterFilter();
                f.DataCenterGuid = new GuidFilter();
                f.DataCenterGuid.MustEqual = Guid;
                filters.Add(f);
            }

            if (ParameterPresent("Name"))
            {
                DataCenterFilter f = new DataCenterFilter();
                f.Name = new StringFilter();
                f.Name.MustEqual = Name;
                filters.Add(f);
            }

            // convert to filter
            DataCenterFilter filter = GenerateFilter(filters);

            // setup sorting
            DataCenterSort sort = new DataCenterSort();
            sort.Name = Sort;

            PageInput page = PageInput.First;
            DataCenterList list = Connection.GetDataCenters(page, filter, sort);

            foreach (DataCenter item in list.Items)
                WriteObject(item);

            while (list.More)
            {
                // advance the page
                page.Page = page.Page + 1;

                list = Connection.GetDataCenters(page, filter, sort);
                foreach (DataCenter item in list.Items)
                    WriteObject(item);
            }
        }

        /// <summary>
        /// Compiles a single filter from a list of filters by combining them
        /// with a logical AND
        /// </summary>
        /// <param name="filters">
        /// List of filters to combine
        /// </param>
        /// <returns></returns>
        private DataCenterFilter GenerateFilter(List<DataCenterFilter> filters)
        {
            DataCenterFilter result = null;

            foreach (DataCenterFilter filter in filters)
            {
                if (result == null)
                {
                    result = filter;
                    continue;
                }

                filter.And = result;
                result = filter;
            }

            return result;
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Create a new datacenter object
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebDataCenter")]
    [OutputType(typeof(DataCenter))]
    public class NewNebDataCenter : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The datacenter address
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public AddressInput Address { get; set; }

        /// <summary>
        /// <para type="description">
        /// A list of contacts for the datacenter. At least one contact must carry
        /// the primary property.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public ContactInput[] Contacts { get; set; }

        /// <summary>
        /// <para type="description">
        /// The name of the datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// An optional note for the datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Note { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                DataCenter newDataCenter = Connection.CreateDataCenter(
                    Name,
                    Address,
                    Contacts,
                    Note
                );

                WriteObject(newDataCenter);
            }
            catch (AggregateException exceptions)
            {
                foreach (Exception ex in exceptions.InnerExceptions)
                    WriteError(ex);
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Deletea a datacenter object
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebDataCenter")]
    public class RemoveNebDataCenter : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// An optional parameter that allows deletion of all child objects
        /// of the datacenter when deleting. If the parameter is not provided
        /// and the datacenter has child objects, the deletion will fail.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Cascade { get; set; }

        /// <summary>
        /// <para type="description">
        /// The unique identifer of the datacenter to delete
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public Guid Guid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.DeleteDataCenter(Guid, Cascade.IsPresent);
                if (!success)
                    throw new Exception("Deletion failed");
            }
            catch (AggregateException exceptions)
            {
                foreach (Exception ex in exceptions.InnerExceptions)
                    WriteError(ex);
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Allows updating of datacenter properties
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "NebDataCenter")]
    [OutputType(typeof(DataCenter))]
    public class SetNebDataCenter : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Allows updating the address of a datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public AddressInput Address { get; set; }

        /// <summary>
        /// <para type="description">
        /// Allows updating the list of contacts for a datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public ContactInput[] Contacts { get; set; }

        /// <summary>
        /// <para type="description">
        /// The unique identifier of the datacenter to update
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Allows updating the name of a datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// Allows updating the optional note of a datacenter
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Note { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                DataCenter updatedDataCenter = Connection.UpdateDataCenter(
                    Guid,
                    Name,
                    Contacts,
                    Address,
                    Note);

                WriteObject(updatedDataCenter);
            }
            catch (AggregateException exceptions)
            {
                foreach (Exception ex in exceptions.InnerExceptions)
                    WriteError(ex);
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
    }
}