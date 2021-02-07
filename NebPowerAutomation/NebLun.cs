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
    /// Get a list of volume exports for existing volumes
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebLun")]
    [OutputType(typeof(Lun))]
    public class GetNebLun : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The unique identifier of a volume export.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// Get volume exports that match the specified LUN ID
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateRange(0, 16384)]
        public long LunId { get; set; }

        /// <summary>
        /// <para type="description">
        /// Get volume exports of the specified NPod
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public NPod NPod { get; set; }

        /// <summary>
        /// <para type="description">
        /// Get volume exports of the specified SPU
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Host Server { get; set; }

        /// <summary>
        /// Get volume exports of the specified SPU
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Spu Spu { get; set; }

        /// <summary>
        /// <para type="description">
        /// Get volume exports of the specified volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Volume Volume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<LunFilter> filters = new List<LunFilter>();

                if (ParameterPresent("Guid"))
                {
                    LunFilter f = new LunFilter();
                    f.LunGuid = new GuidFilter();
                    f.LunGuid.MustEqual = Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("LunId"))
                {
                    LunFilter f = new LunFilter();
                    f.LunId = new IntFilter();
                    f.LunId.MustEqual = LunId;
                    filters.Add(f);
                }

                if (ParameterPresent("Volume"))
                {
                    LunFilter f = new LunFilter();
                    f.VolumeGuid = new GuidFilter();
                    f.VolumeGuid.MustEqual = Volume.Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("NPod"))
                {
                    LunFilter f = new LunFilter();
                    f.NPodGuid = new GuidFilter();
                    f.NPodGuid.MustEqual = NPod.Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("Spu"))
                {
                    LunFilter f = new LunFilter();
                    f.SpuSerial = new StringFilter();
                    f.SpuSerial.MustEqual = Spu.Serial;
                    filters.Add(f);
                }

                if (ParameterPresent("Server"))
                {
                    LunFilter f = new LunFilter();
                    f.HostGuid = new GuidFilter();
                    f.HostGuid.MustEqual = Server.Guid;
                    filters.Add(f);
                }

                // convert to filter
                LunFilter filter = GenerateFilter(filters);

                PageInput page = PageInput.First;
                LunList list = Connection.GetLuns(page, filter, null);

                foreach (Lun item in list.Items)
                    WriteObject(item);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetLuns(page, filter, null);
                    foreach (Lun item in list.Items)
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
        private LunFilter GenerateFilter(List<LunFilter> filters)
        {
            LunFilter result = null;

            foreach (LunFilter filter in filters)
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
    /// Creates a new volume export
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebLun")]
    [OutputType(typeof(Lun))]
    public class NewNebLun : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// If provided, the volume will be exported with ALUA turned off
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Local { get; set; }

        /// <summary>
        /// <para type="description">
        /// An optional LUN ID for the new export
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateRange(0, 16384)]
        public long LunId { get; set; }

        /// <summary>
        /// <para type="description">
        /// Host / server to which the volume shall be exported to. Either
        /// Server or Spu must be specified
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Host Server { get; set; }

        /// <summary>
        /// <para type="description">
        /// SPU to which the volume shall be exported to. Either Spu or
        /// Server must be specified
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Spu Spu { get; set; }

        /// <summary>
        /// <para type="description">
        /// The volume that shall be exported.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Volume Volume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                CreateLunInput input = new CreateLunInput();

                if (ParameterPresent("Server"))
                    input.HostGuids = new Guid[] { Server.Guid };

                if (ParameterPresent("Spu"))
                    input.SpuSerials = new string[] { Spu.Serial };

                if (ParameterPresent("Volume"))
                    input.VolumeGuid = Volume.Guid;

                if (ParameterPresent("LunId"))
                    input.LunId = LunId;

                input.Local = Local.IsPresent;

                Lun newLun = Connection.CreateLun(input);

                WriteObject(newLun);
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
    /// Removes a volume export
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebLun")]
    [OutputType(typeof(void))]
    public class RemoveNebLun : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The unique identifier of the volume export to remove
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
                bool success = Connection.DeleteLun(Guid);
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
}