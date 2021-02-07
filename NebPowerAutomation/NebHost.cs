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
    /// Get a list of hosts / servers
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebHost")]
    [OutputType(typeof(Host))]
    public class GetNebHost : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter for host by a specific board serial
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string BoardSerial { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for host by a specific chassis serial
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ChassisSerial { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter hosts by a specific unique identifier
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 0)]
        [ValidateNotNullOrEmpty]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for host by a specific manufacturer
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for host by a specific model
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Model { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for host by a specific host name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for hosts in a specific nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public NPod NPod { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for host
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public Host Server { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            List<HostFilter> filters = new List<HostFilter>();

            if (ParameterPresent("Guid"))
            {
                HostFilter f = new HostFilter();
                f.HostGuid = new GuidFilter();
                f.HostGuid.MustEqual = Guid;
                filters.Add(f);
            }

            if (ParameterPresent("Server"))
            {
                HostFilter f = new HostFilter();
                f.HostGuid = new GuidFilter();
                f.HostGuid.MustEqual = Server.Guid;
                filters.Add(f);
            }

            if (ParameterPresent("Name"))
            {
                HostFilter f = new HostFilter();
                f.Name = new StringFilter();
                f.Name.MustEqual = Name;
                filters.Add(f);
            }

            if (ParameterPresent("BoardSerial"))
            {
                HostFilter f = new HostFilter();
                f.BoardSerial = new StringFilter();
                f.BoardSerial.MustEqual = BoardSerial;
                filters.Add(f);
            }

            if (ParameterPresent("ChassisSerial"))
            {
                HostFilter f = new HostFilter();
                f.ChassisSerial = new StringFilter();
                f.ChassisSerial.MustEqual = ChassisSerial;
                filters.Add(f);
            }

            if (ParameterPresent("Manufacturer"))
            {
                HostFilter f = new HostFilter();
                f.Manufacturer = new StringFilter();
                f.Manufacturer.MustEqual = Manufacturer;
                filters.Add(f);
            }

            if (ParameterPresent("Model"))
            {
                HostFilter f = new HostFilter();
                f.Model = new StringFilter();
                f.Model.MustEqual = Model;
                filters.Add(f);
            }

            if (ParameterPresent("NPod"))
            {
                HostFilter f = new HostFilter();
                f.NPodGuid = new GuidFilter();
                f.NPodGuid.MustEqual = NPod.Guid;
                filters.Add(f);
            }

            HostFilter filter = GenerateFilter(filters);

            PageInput page = PageInput.First;
            HostList list = Connection.GetHosts(page, filter, null);

            foreach (Host item in list.Items)
                WriteObject(item);

            while (list.More)
            {
                // advance the page
                page.Page = page.Page + 1;

                list = Connection.GetHosts(page, filter, null);
                foreach (Host item in list.Items)
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
        private HostFilter GenerateFilter(List<HostFilter> filters)
        {
            HostFilter result = null;

            foreach (HostFilter filter in filters)
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
    /// Allows updating of properties of a host
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "NebHost")]
    [OutputType(typeof(Host))]
    public class SetNebHost : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The unique identifier of the host to update
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// The new name for the host
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// The new note for the host
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Note { get; set; }

        /// <summary>
        /// <para type="description">
        /// The new rack unique identifier, when moving a server to a
        /// different rack
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public Guid RackGuid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            // build input object for the NebSharp call
            UpdateHostInput input = new UpdateHostInput();

            if (ParameterPresent("Name"))
                input.Name = Name;

            if (ParameterPresent("RackGuid"))
                input.RackGuid = RackGuid;

            if (ParameterPresent("Note"))
                input.Note = Note;

            Host updatedNebHost = Connection.UpdateHost(Guid, input);

            WriteObject(updatedNebHost);
        }
    }
}