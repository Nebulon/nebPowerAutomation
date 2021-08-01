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
    /// Get a list of existing volumes, snapshots, or clones
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebVolume")]
    [OutputType(typeof(Volume))]
    public class GetNebVolume : NebPSCmdlet
    {
        /// <summary>
        /// Filter volumes by a specific unique identifier
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public Guid Guid { get; set; }

        /// <summary>
        /// <para type="description">
        /// When provided, also snapshots are included in the listing
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter IncludeSnapshots { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter volumes by a specific volume name
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// The nPod in which the volume is created
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public NPod NPod { get; set; }

        /// <summary>
        /// <para type="description">
        /// When provided, only snapshots will be listed
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter OnlySnapshots { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filter snapshots by their parent volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public Volume ParentVolume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<VolumeFilter> filters = new List<VolumeFilter>();

                VolumeFilter snapsFilter = new VolumeFilter();
                snapsFilter.BaseOnly = !OnlySnapshots.IsPresent && !IncludeSnapshots.IsPresent;
                filters.Add(snapsFilter);

                if (OnlySnapshots.IsPresent)
                {
                    VolumeFilter f1 = new VolumeFilter();
                    f1.SnapshotsOnly = true;
                    filters.Add(f1);
                }

                if (ParameterPresent("Guid"))
                {
                    VolumeFilter f = new VolumeFilter();
                    f.Guid = new GuidFilter();
                    f.Guid.MustEqual = Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("Name"))
                {
                    VolumeFilter f = new VolumeFilter();
                    f.Name = new StringFilter();
                    f.Name.MustEqual = Name;
                    filters.Add(f);
                }

                if (ParameterPresent("NPod"))
                {
                    VolumeFilter f = new VolumeFilter();
                    f.NPodGuid = new GuidFilter();
                    f.NPodGuid.MustEqual = NPod.Guid;
                    filters.Add(f);
                }

                if (ParameterPresent("ParentVolume"))
                {
                    VolumeFilter f = new VolumeFilter();
                    f.ParentGuid = new GuidFilter();
                    f.ParentGuid.MustEqual = ParentVolume.Guid;
                    filters.Add(f);

                    // make sure that snapshots are included
                    foreach (VolumeFilter ef in filters)
                    {
                        if (ef.BaseOnly.HasValue)
                            ef.BaseOnly = false;
                    }
                }

                // convert to filter
                VolumeFilter filter = GenerateFilter(filters);

                // Compile a sort direction from the provided input
                // Default sort direction is Ascending
                VolumeSort sort = new VolumeSort();
                sort.Name = SortDirection.Ascending;

                PageInput page = PageInput.First;
                VolumeList list = Connection.GetVolumes(page, filter, sort);

                foreach (Volume item in list.Items)
                    WriteObject(item);

                while (list.More)
                {
                    // advance the page
                    page.Page = page.Page + 1;

                    list = Connection.GetVolumes(page, filter, sort);
                    foreach (Volume item in list.Items)
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
        private VolumeFilter GenerateFilter(List<VolumeFilter> filters)
        {
            VolumeFilter result = null;

            foreach (VolumeFilter filter in filters)
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
    /// Creates a clone from a snapshot or volume.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebClone")]
    [OutputType(typeof(void))]
    public class NewNebClone : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The name for the new clone
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// The volume or snapshot from which to create a clone
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Volume Volume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                Volume clone = Connection.CreateClone(
                    Name,
                    Volume.Guid
                );

                WriteObject(clone);
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
    /// Creates a read only snapshot of a volume.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebSnapshot")]
    [OutputType(typeof(void))]
    public class NewNebSnapshot : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The time in seconds in which the snapshot will be automatically
        /// deleted after its creation
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public long ExpirationSeconds { get; set; }

        /// <summary>
        /// <para type="description">
        /// A name pattern to use for the new snapshot
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public string NamePattern { get; set; }

        /// <summary>
        /// <para type="description">
        /// The time in seconds for how long the snapshot can't be deleted
        /// after its creation
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public long RetentionSeconds { get; set; }

        /// <summary>
        /// <para type="description">
        /// The volume from which to create a snapshot
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Volume Volume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                Volume[] snapshots = Connection.CreateSnapshot(
                    new[] { Volume.Guid },
                    new[] { NamePattern },
                    ExpirationSeconds,
                    RetentionSeconds
                );

                foreach(Volume snapshot in snapshots)
                    WriteObject(snapshot);
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
    /// Creates a new volume
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebVolume")]
    [OutputType(typeof(Volume))]
    public class NewNebVolume : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Optionally the SPU serial number on which to place the backup
        /// copy of a mirrored volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public string BackupSpuSerial { get; set; }

        /// <summary>
        /// <para type="description">
        /// Allows overwriting capacity utilization warnings and force the
        /// creation of a volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// <para type="description">
        /// Creates the volume as a mirrored volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Mirrored { get; set; }

        /// <summary>
        /// <para type="description">
        /// The name of the new volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">
        /// The nPod in which the volume shall be created
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull]
        public NPod NPod { get; set; }

        /// <summary>
        /// <para type="description">
        /// Optionally the SPU serial number on which to place the primary
        /// copy of a mirrored volume
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateNotNull]
        public string OwnerSpuSerial { get; set; }

        /// <summary>
        /// <para type="description">
        /// The size of the volume in bytes
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public long SizeBytes { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                CreateVolumeInput input = new CreateVolumeInput();
                input.Name = Name;
                input.NPodGuid = NPod.Guid;
                input.SizeBytes = SizeBytes;
                input.Mirrored = Mirrored.IsPresent;
                input.OwnerSpuSerial = OwnerSpuSerial;
                input.BackupSpuSerial = BackupSpuSerial;
                input.Force = Force.IsPresent;

                Volume volume = Connection.CreateVolume(input);
                WriteObject(volume);
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
    /// Deletes a volume.
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebVolume")]
    [OutputType(typeof(void))]
    public class RemoveNebVolume : NebPSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The volume, snapshot or clone to delete
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Volume Volume { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                if (ParameterPresent("Volume"))
                {
                    bool success = Connection.DeleteVolume(Volume.Guid);
                    if (!success)
                        throw new Exception("Deletion failed");
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
    }
}