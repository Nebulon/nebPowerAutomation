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
    /// Get a list of open alerts
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebAlert")]
    [OutputType(typeof(Alert))]
    public class GetNebAlert : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter for alerts created after the specified date
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public DateTime CreatedAfter { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for alerts created before the specified date
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public DateTime CreatedBefore { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for alerts that are releated to the specified resource Id
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ResourceId { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for alerts that are related to the specified resource type
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for alerts that match the specified alert severity
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public AlertSeverity Severity { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter for alerts that match the specified alert status
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public AlertStatus Status { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                AlertFilter filter = new AlertFilter();

                if (ParameterPresent("CreatedAfter"))
                    filter.CreatedAfter = CreatedAfter;

                if (ParameterPresent("CreatedBefore"))
                    filter.CreatedBefore = CreatedBefore;

                if (ParameterPresent("ResourceId"))
                    filter.ResourceID = ResourceId;

                if (ParameterPresent("ResourceType"))
                    filter.ResourceType = ResourceType;

                if (ParameterPresent("Severity"))
                    filter.Severity = Severity;

                if (ParameterPresent("Status"))
                    filter.Status = Status;

                PageInput page = PageInput.First;
                AlertList list = Connection.GetOpenAlerts(page, filter);

                foreach (Alert alert in list.Items)
                    WriteObject(alert);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetOpenAlerts(page);
                    foreach (Alert alert in list.Items)
                        WriteObject(alert);
                }
            }
            catch (AggregateException exceptions)
            {
                foreach (Exception ex in exceptions.InnerExceptions)
                {
                    ErrorRecord record = new ErrorRecord(
                        ex,
                        ex.GetType().ToString(),
                        ErrorCategory.NotSpecified,
                        null);

                    WriteError(record);
                }
            }
            catch (Exception ex)
            {
                ErrorRecord record = new ErrorRecord(
                    ex,
                    ex.GetType().ToString(),
                    ErrorCategory.NotSpecified,
                    null);

                WriteError(record);
            }
        }
    }
}