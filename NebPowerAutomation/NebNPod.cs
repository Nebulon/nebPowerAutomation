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
    /// Get a list of nPods
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebNPod")]
    [OutputType(typeof(NPod))]
    public class GetNebNPod : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter nPods by the specified unique identifier
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public Guid Guid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            // Compile a filter from the provided input
            NPodFilter filter = null;

            if (ParameterPresent("Guid"))
            {
                filter = new NPodFilter();
                filter.NPodGuid = new GuidFilter();
                filter.NPodGuid.MustEqual = Guid;
            }

            PageInput page = PageInput.First;
            NPodList list = Connection.GetNebNPods(page, filter, null);

            foreach (NPod item in list.Items)
                WriteObject(item);

            while (list.More)
            {
                // advance the page
                page.Page = page.Page + 1;

                list = Connection.GetNebNPods(page, filter, null);
                foreach (NPod item in list.Items)
                    WriteObject(item);
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Create a new input object for configuring network interfaces for SPUs
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebIPInfoConfigInput")]
    [OutputType(typeof(IPInfoConfigInput))]
    public class NewIPInfoConfigInput : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The network IP addresss (IPv4 or IPv6) for the network
        /// interface. This value is only considered when DHCP isn't used
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Address { get; set; }

        /// <summary>
        /// <para type="description">
        /// Specifies the bond mode for the data interfaces
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public BondType BondMode { get; set; }

        /// <summary>
        /// <para type="description">
        /// Include data port #1 in the configuration
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter DataPort1 { get; set; }

        /// <summary>
        /// <para type="description">
        /// Include data port #2 in the configuration
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter DataPort2 { get; set; }

        /// <summary>
        /// <para type="description">
        /// Use DHCP for the data network interface instead
        /// of a static configuration. Static configuration
        /// is recommended.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Dhcp { get; set; }

        /// <summary>
        /// <para type="description">
        /// The gateway IP address to use for the interface
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Gateway { get; set; }

        /// <summary>
        /// <para type="description">
        /// Allows overwriting the default MTU 1500
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public long Mtu { get; set; }

        /// <summary>
        /// <para type="description">
        /// The network mask in bits
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public long NetmaskBits { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            IPInfoConfigInput input = new IPInfoConfigInput();
            if (ParameterPresent("Address"))
                input.Address = Address;

            if (ParameterPresent("BondMode"))
                input.BondMode = BondMode;

            if (ParameterPresent("Gateway"))
                input.Gateway = Gateway;

            if (ParameterPresent("Mtu"))
                input.Mtu = Mtu;

            if (ParameterPresent("NetmaskBits"))
                input.NetmaskBits = NetmaskBits;

            // interfaces
            List<string> interfaces = new List<string>();

            if (DataPort1.IsPresent)
                interfaces.Add("enP8p1s0f0np0");

            if (DataPort2.IsPresent)
                interfaces.Add("enP8p1s0f1np1");

            if (interfaces.Count == 0)
                throw new Exception("Either DataPort1, DataPort2 or both must be specified");

            input.Interfaces = interfaces.ToArray();
            input.Dhcp = Dhcp.IsPresent;

            WriteObject(input);
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Creates a new nPod
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebNPod")]
    [OutputType(typeof(NPod))]
    public class NewNebNPod : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Ignore provisioning warnings and force the creation of
        /// the nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter IgnoreWarnings { get; set; }

        /// <summary>
        /// <para type="description">
        /// The name for the new nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNull]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// An optional note for the new nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public string Note { get; set; }

        /// <summary>
        /// <para type="description">
        /// The unique identifier of the nPod group this nPod
        /// shall be created in
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public Guid NPodGroupGuid { get; set; }

        /// <summary>
        /// <para type="description">
        /// List of SPU configurations to use for the new nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public NPodSpuInput[] Spus { get; set; }

        /// <summary>
        /// <para type="description">
        /// The unique identifier of the nPod template for the new
        /// nPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public Guid TemplateGuid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                NPod newNPod = Connection.CreateNPod(
                    Name,
                    NPodGroupGuid,
                    Spus,
                    TemplateGuid,
                    Note,
                    null,
                    IgnoreWarnings
                );

                WriteObject(newNPod);
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
    /// Creates a new input object to configure SPUs for nPods
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebNPodSpuInput")]
    [OutputType(typeof(NPodSpuInput))]
    public class NewNebNPodSpuInput : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// List of SPU network configurations for the SPUs data ports
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public IPInfoConfigInput[] IPInfoConfigInput { get; set; }

        /// <summary>
        /// <para type="description">
        /// A human readable name for the SPU
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// The serial number of the SPU
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNull]
        public string Serial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            NPodSpuInput input = new NPodSpuInput();
            if (ParameterPresent("Name"))
                input.Name = Name;

            if (ParameterPresent("Serial"))
                input.Serial = Serial;

            if (ParameterPresent("IPInfoConfigInput"))
                input.DataIps = IPInfoConfigInput;

            // make sure that all optional parameters are adjusted
            if (string.IsNullOrEmpty(input.Name))
                input.Name = input.Serial;

            WriteObject(input);
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Removes an existing nPod
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebNPod")]
    [OutputType(typeof(void))]
    public class RemoveNebNPod : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The unique identifier of the nPod to delete
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public Guid Guid { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.DeleteNPod(Guid, false);

                if (!success)
                    throw new Exception("NPod deletion failed");
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