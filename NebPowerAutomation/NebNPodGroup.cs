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
    /// Get a list of nPodGroups
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebNPodGroup")]
    [OutputType(typeof(NPodGroup))]
    public class GetNebNPodGroup : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter nPodGroups by a specific unique identifier
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter nPodGroups by a specific name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<NPodGroupFilter> filters = new List<NPodGroupFilter>();

                if (ParameterPresent("Guid"))
                {
                    NPodGroupFilter f = new NPodGroupFilter();
                    f.Guid = new GuidFilter();
                    f.Guid.MustEqual = Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("Name"))
                {
                    NPodGroupFilter f = new NPodGroupFilter();
                    f.Name = new StringFilter();
                    f.Name.MustEqual = Name;
                    filters.Add(f);
                }

                // convert to filter
                NPodGroupFilter filter = GenerateFilter(filters);

                PageInput page = PageInput.First;
                NPodGroupList list = Connection.GetNebNPodGroups(page, filter, null);

                foreach (NPodGroup item in list.Items)
                    WriteObject(item);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetNebNPodGroups(page, filter, null);
                    foreach (NPodGroup item in list.Items)
                        WriteObject(item);
                }
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

        /// <summary>
        /// Compiles a single filter from a list of filters by combining them
        /// with a logical AND
        /// </summary>
        /// <param name="filters">
        /// List of filters to combine
        /// </param>
        /// <returns></returns>
        private NPodGroupFilter GenerateFilter(List<NPodGroupFilter> filters)
        {
            NPodGroupFilter result = null;

            foreach (NPodGroupFilter filter in filters)
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
    /// Creates a new nPod Group
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebNPodGroup")]
    [OutputType(typeof(NPodGroup))]
    public class NewNebNPodGroup : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Name of the nPod group
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// Optional note for the nPod group
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public string Note { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                NPodGroup newGroup = Connection.CreateNPodGroup(Name, Note);
                WriteObject(newGroup);
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
    /// Deletes an existing nPod group
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebNPodGroup")]
    [OutputType(typeof(void))]
    public class RemoveNebNPodGroup : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Unique identifier of the nPod group to delete
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.DeleteNPodGroup(Guid);
                if (!success)
                    throw new Exception("nPodGroup deletion failed");
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