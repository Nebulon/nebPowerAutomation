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
    /// Get a list of nPod templates
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebNPodTemplate", DefaultParameterSetName = "All")]
    [OutputType(typeof(NPodTemplate))]
    public class GetNPodTemplate : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Filter templates by the specified application name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "Name")]
        public string Application { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter templates by the specified template unique identifier
        /// </para>
        /// </summary>

        [Parameter(Mandatory = false, ParameterSetName = "Guid")]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// If specified, includes all previous versions of templates
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter IncludeVersions { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter templates by the specified template name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter templates for only Nebulon provided templates
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool NebulonTemplate { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter templates by the specified operating system name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "Name")]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<NPodTemplateFilter> filters = new List<NPodTemplateFilter>();

                if (ParameterPresent("Guid"))
                {
                    NPodTemplateFilter f = new NPodTemplateFilter();
                    f.Guid = new GuidFilter();
                    f.Guid.MustEqual = Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("Application"))
                {
                    NPodTemplateFilter f = new NPodTemplateFilter();
                    f.Application = new StringFilter();
                    f.Application.MustEqual = Application;
                    filters.Add(f);
                }

                if (ParameterPresent("OperatingSystem"))
                {
                    NPodTemplateFilter f = new NPodTemplateFilter();
                    f.OperatingSystem = new StringFilter();
                    f.OperatingSystem.MustEqual = OperatingSystem;
                    filters.Add(f);
                }

                if (ParameterPresent("Name"))
                {
                    NPodTemplateFilter f = new NPodTemplateFilter();
                    f.Name = new StringFilter();
                    f.Name.MustEqual = Name;
                    filters.Add(f);
                }

                if (ParameterPresent("NebulonTemplate"))
                {
                    NPodTemplateFilter f = new NPodTemplateFilter();
                    f.NebulonTempalte = NebulonTemplate;
                    filters.Add(f);
                }

                NPodTemplateFilter versionFilter = new NPodTemplateFilter();
                versionFilter.OnlyLastVersion = !IncludeVersions.IsPresent;
                filters.Add(versionFilter);

                // convert to filter
                NPodTemplateFilter filter = GenerateFilter(filters);

                PageInput page = PageInput.First;
                NPodTemplateList list = Connection.GetNPodTemplates(page, filter, null);

                foreach (NPodTemplate item in list.Items)
                    WriteObject(item);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetNPodTemplates(page, filter, null);
                    foreach (NPodTemplate item in list.Items)
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
        private NPodTemplateFilter GenerateFilter(List<NPodTemplateFilter> filters)
        {
            NPodTemplateFilter result = null;

            foreach (NPodTemplateFilter filter in filters)
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

        /// <summary>
        /// <para type="synopsis">
        /// Creates a new application template
        /// </para>
        /// </summary>
        [Cmdlet(VerbsCommon.New, "NebNPodTemplate")]
        [OutputType(typeof(NPodTemplate))]
        public class NewNPodTemplate : NebPSCmdlet
        {
            /// <summary>
            /// <para type="description">
            /// Name of the application
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string Application { get; set; }

            /// <summary>
            /// <para type="description">
            /// URL to a boot image
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string BootImageURL { get; set; }

            /// <summary>
            /// <para type="description">
            /// If specified a boot volume will be provisioned
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public bool BootVolume { get; set; }

            /// <summary>
            /// <para type="description">
            /// Size of the boot volume
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long BootVolumeSizeBytes { get; set; }

            /// <summary>
            /// <para type="description">
            /// Indicates if volumes shall be mirrored
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public bool MirroredVolume { get; set; }

            /// <summary>
            /// <para type="description">
            /// Name of the application tempalte
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public string Name { get; set; }

            /// <summary>
            /// <para type="description">
            /// Optional note for the template
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string Note { get; set; }

            /// <summary>
            /// <para type="description">
            /// Name of the operating system
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public string OperatingSystem { get; set; }

            /// <summary>
            /// <para type="description">
            /// Anticipated savings factor for data
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public double SavingFactor { get; set; }

            /// <summary>
            /// <para type="description">
            /// Indicates if volumes shall be shared between all hosts in the nPod
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public bool SharedLun { get; set; }

            /// <summary>
            /// <para type="description">
            /// List of snapshot schedule templates to associate with this template
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public Guid[] SnapshotScheduleTemplateGuids { get; set; }

            /// <summary>
            /// <para type="description">
            /// Allows specifying the number of volumes to create (only supported when using local volumes)
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long VolumeCount { get; set; }

            /// <summary>
            /// <para type="description">
            /// Allows specifying the size of volumes to create
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long VolumeSizeBytes { get; set; }

            /// <summary>
            /// Performs execution of the command
            /// </summary>
            protected override void ProcessRecord()
            {
                try
                {
                    CreateNPodTemplateInput input = new CreateNPodTemplateInput();
                    if (ParameterPresent("Name"))
                        input.Name = Name;
                    if (ParameterPresent("SavingFactor"))
                        input.SavingFactor = SavingFactor;
                    if (ParameterPresent("MirroredVolume"))
                        input.MirroredVolume = MirroredVolume;
                    if (ParameterPresent("BootVolume"))
                        input.BootVolume = BootVolume;
                    if (ParameterPresent("OperatingSystem"))
                        input.OperatingSystem = OperatingSystem;
                    if (ParameterPresent("VolumeSizeBytes"))
                        input.VolumeSizeBytes = VolumeSizeBytes;
                    if (ParameterPresent("SharedLun"))
                        input.SharedLun = SharedLun;
                    if (ParameterPresent("BootVolumeSizeBytes"))
                        input.BootVolumeSizeBytes = BootVolumeSizeBytes;
                    if (ParameterPresent("BootImageURL"))
                        input.BootImageURL = BootImageURL;
                    if (ParameterPresent("Application"))
                        input.Application = Application;
                    if (ParameterPresent("Note"))
                        input.Note = Note;
                    if (ParameterPresent("SnapshotScheduleTemplateGuids"))
                        input.SnapshotScheduleTemplateGuids = SnapshotScheduleTemplateGuids;
                    if (ParameterPresent("VolumeCount"))
                        input.VolumeCount = VolumeCount;

                    NPodTemplate template = Connection.CreateNPodTemplate(input);
                    WriteObject(template);
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
        /// Delete an application template
        /// </para>
        /// </summary>
        [Cmdlet(VerbsCommon.Remove, "NebNPodTemplate")]
        [OutputType(typeof(void))]
        public class RemoveNPodTemplate : NebPSCmdlet
        {
            /// <summary>
            /// <para type="description">
            /// The unique identifier of the template to delete
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
            public Guid Guid { get; set; }

            /// <summary>
            /// Performs execution of the command
            /// </summary>
            protected override void ProcessRecord()
            {
                try
                {
                    bool success = Connection.DeleteNPodTemplate(Guid);
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
        /// Allows updating properties of an application template
        /// </para>
        /// </summary>
        [Cmdlet(VerbsCommon.Set, "NebNPodTemplate")]
        [OutputType(typeof(NPodTemplate))]
        public class SetNPodTemplate : NebPSCmdlet
        {
            /// <summary>
            /// <para type="description">
            /// Name of the application
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string Application { get; set; }

            /// <summary>
            /// <para type="description">
            /// URL to a boot image
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string BootImageURL { get; set; }

            /// <summary>
            /// <para type="description">
            /// If set to true, a boot volume will be provisioned
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public bool BootVolume { get; set; }

            /// <summary>
            /// <para type="description">
            /// Size of the boot volume in bytes
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long BootVolumeSizeBytes { get; set; }

            /// <summary>
            /// <para type="description">
            /// Indicates if the volume shall be mirrored
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public bool MirroredVolume { get; set; }

            /// <summary>
            /// <para type="description">
            /// Name of the application template to update
            /// </para>
            /// </summary>
            [Parameter(Mandatory = true)]
            public string Name { get; set; }

            /// <summary>
            /// <para type="description">
            /// Optional note for the application template
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string Note { get; set; }

            /// <summary>
            /// <para type="description">
            /// Name of the operating system
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public string OperatingSystem { get; set; }

            /// <summary>
            /// <para type="description">
            /// Anticipated savings factor
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public double SavingFactor { get; set; }

            /// <summary>
            /// <para type="description">
            /// Indicates if the volumes shall be shared between all hosts in the nPod
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public bool SharedLun { get; set; }

            /// <summary>
            /// <para type="description">
            /// List of snapshot schedules associated with the template
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public Guid[] SnapshotScheduleTemplateGuids { get; set; }

            /// <summary>
            /// <para type="description">
            /// Allows specifying the number of volumes to create (only supported when using local volumes)
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long VolumeCount { get; set; }

            /// <summary>
            /// <para type="description">
            /// Allows specifying the size of volumes to create
            /// </para>
            /// </summary>
            [Parameter(Mandatory = false)]
            public long VolumeSizeBytes { get; set; }

            /// <summary>
            /// Performs execution of the command
            /// </summary>
            protected override void ProcessRecord()
            {
                try
                {
                    UpdateNPodTemplateInput input = new UpdateNPodTemplateInput();
                    if (ParameterPresent("Name"))
                        input.Name = Name;
                    if (ParameterPresent("SavingFactor"))
                        input.SavingFactor = SavingFactor;
                    if (ParameterPresent("MirroredVolume"))
                        input.MirroredVolume = MirroredVolume;
                    if (ParameterPresent("BootVolume"))
                        input.BootVolume = BootVolume;
                    if (ParameterPresent("OperatingSystem"))
                        input.OperatingSystem = OperatingSystem;
                    if (ParameterPresent("VolumeSizeBytes"))
                        input.VolumeSizeBytes = VolumeSizeBytes;
                    if (ParameterPresent("SharedLun"))
                        input.SharedLun = SharedLun;
                    if (ParameterPresent("BootVolumeSizeBytes"))
                        input.BootVolumeSizeBytes = BootVolumeSizeBytes;
                    if (ParameterPresent("BootImageURL"))
                        input.BootImageURL = BootImageURL;
                    if (ParameterPresent("Application"))
                        input.Application = Application;
                    if (ParameterPresent("Note"))
                        input.Note = Note;
                    if (ParameterPresent("SnapshotScheduleTemplateGuids"))
                        input.SnapshotScheduleTemplateGuids = SnapshotScheduleTemplateGuids;
                    if (ParameterPresent("VolumeCount"))
                        input.VolumeCount = VolumeCount;

                    NPodTemplate template = Connection.UpdateNPodTemplate(input);
                    WriteObject(template);
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
}