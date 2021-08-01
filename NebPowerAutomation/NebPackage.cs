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
    /// Update the nebOS software version of a nPod
    /// </summary>
    [Cmdlet(VerbsData.Update, "NebFirmware")]
    [OutputType(typeof(void))]
    public class UpdateNebFirmware : NebPSCmdlet
    {
        /// <summary>
        /// Guid of the nPod to update
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// SPU Serial number to update
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Serial { get; set; }

        /// <summary>
        /// Name of the nebOS package to install
        /// </summary>
        [Parameter(Mandatory = true)]
        public string PackageName { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                if (ParameterPresent("Guid"))
                {
                    Connection.UpdateNPodFirmware(Guid, PackageName);
                    return;
                }

                if (ParameterPresent("Serial"))
                {
                    Connection.UpdateSpuFirmware(Serial, PackageName, false);
                    return;
                }

                throw new Exception("One of 'Guid' or 'Serial' must be provided.");

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