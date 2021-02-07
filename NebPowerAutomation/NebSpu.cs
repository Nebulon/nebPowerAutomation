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
    /// Get a list of services processing units
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebSpu")]
    [OutputType(typeof(Spu))]
    public class GetNebSpu : NebPSCmdlet
    {
        /// <summary>
        /// Filter SPUs for a specific serial number
        /// </summary>
        [Parameter(Mandatory = false, Position = 0)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Filter list of SPUs to include only unused SPUs
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Unused { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<SpuFilter> filters = new List<SpuFilter>();

                if (ParameterPresent("SpuSerial"))
                {
                    SpuFilter f = new SpuFilter();
                    f.Serial = new StringFilter();
                    f.Serial.MustEqual = SpuSerial;
                    filters.Add(f);
                }

                if (ParameterPresent("Unused"))
                {
                    SpuFilter f = new SpuFilter();
                    f.NotInNPod = true;
                    filters.Add(f);
                }

                // convert to filter
                SpuFilter filter = GenerateFilter(filters);

                SpuSort sort = new SpuSort();
                sort.Serial = SortDirection.Ascending;

                PageInput page = PageInput.First;
                SpuList list = Connection.GetSpus(page, filter, sort);

                foreach (Spu item in list.Items)
                    WriteObject(item);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetSpus(page, filter, sort);
                    foreach (Spu item in list.Items)
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

        private SpuFilter GenerateFilter(List<SpuFilter> filters)
        {
            SpuFilter result = null;

            foreach (SpuFilter filter in filters)
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
    /// Retrieves a list of custom diagnostic command requests.
    ///
    /// <para>
    /// Custom diagnostic command requests are used by customer satisfaction
    /// teams to run arbitrary troubleshooting commands on SPUs. These
    /// require user confirmation.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebSpuCustomDiagnostics")]
    [OutputType(typeof(SpuCustomDiagnostic))]
    public class GetNebSpuCustomDiagnostics : NebPSCmdlet
    {
        /// <summary>
        /// Filter for a specific serial number
        /// </summary>
        [Parameter(Mandatory = false, Position = 0)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                SpuCustomDiagnostic[] spuCustomDiagnostics = Connection.GetSpuCustomDiagnostics(SpuSerial);

                foreach (SpuCustomDiagnostic item in spuCustomDiagnostics)
                    WriteObject(item);
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
    /// Allows to secure-erase data on a services processing unit (SPU)
    ///
    /// <para type="description">
    /// The secure erase functionality allows a deep-erase of data stored
    /// on the physical drives attached to the SPU.Only SPUs that are not
    /// part of a nPod can be secure-erased.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "NebSpuSecureErase")]
    [OutputType(typeof(void))]
    public class InvokeNebSpuSecureErase : NebPSCmdlet
    {
        /// <para type="description">
        /// Serial number of the services processing unit
        /// </para>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Serial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.SecureEraseSpu(Serial);
                if (!success)
                    throw new Exception("secure erase invokation failed");
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
    /// Allows turning on the locate LEDs of an SPU
    /// </summary>
    /// <para type="description">
    /// Allows testing if a services processing unit with a given serial
    /// number is reachable over the network.
    /// </para>
    [Cmdlet(VerbsDiagnostic.Ping, "NebSpu")]
    [OutputType(typeof(void))]
    public class PingNebSpu : NebPSCmdlet
    {
        /// <para type="description">
        /// Serial number of the services processing unit
        /// </para>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Serial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.LocateSpu(Serial);
                if (!success)
                    throw new Exception("operation was unsuccessful");
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
    /// Adds an unregistered SPU to the organization
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "NebSpu")]
    [OutputType(typeof(void))]
    public class RegisterNebSpu : NebPSCmdlet
    {
        /// <summary>
        /// The serial number of the SPU to register
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ValueFromPipelineByPropertyName = true)]
        public string Serial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.ClaimSpu(Serial);
                if (!success)
                    throw new Exception("Register failed");
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
    /// Allows running custom diagnostic commands
    ///
    /// <para>
    /// SPU custom diagnostics requests allows customers to run arbitrary
    /// diagnostic commands on the services processing units as part of
    /// troubleshooting issues during a support case.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Request, "NebCustomDiagnostics")]
    [OutputType(typeof(SpuCustomDiagnostic))]
    public class RequestNebCustomDiagnostics : NebPSCmdlet
    {
        /// <summary>
        /// The name of the diagnostics to execute
        /// </summary>
        [Parameter(Mandatory = false)]
        public string DiagnosticName { get; set; }

        /// <summary>
        /// The Guid of a nPod for which to execute custom diagnostics
        /// </summary>
        [Parameter(Mandatory = false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// The custom diagnostics request Guid
        /// </summary>
        [Parameter(Mandatory = false)]
        public Guid RequestGuid { get; set; }

        /// <summary>
        /// The serial number of an SPU for which to execute custom diagnostics
        /// </summary>
        [Parameter(Mandatory = false, Position = 0)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.RunCustomDiagnostics(
                    SpuSerial, NPodGuid, DiagnosticName, RequestGuid
                );
                if (!success)
                    throw new Exception("Custom diagnostics failed");
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
    /// Removes a registered SPU from the organization
    /// </summary>
    [Cmdlet(VerbsLifecycle.Unregister, "NebSpu")]
    [OutputType(typeof(void))]
    public class UnregisterNebSpu : NebPSCmdlet
    {
        /// <summary>
        /// The serial number of the SPU to unregister
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ValueFromPipelineByPropertyName = true)]
        public string Serial { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                bool success = Connection.ReleaseSpu(Serial);
                if (!success)
                    throw new Exception("Unregister failed");
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