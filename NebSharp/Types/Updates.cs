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

using System;

namespace NebSharp.Types
{
    /// <summary>
    /// Indicates the importance for installing a nebulon package
    /// </summary>
    public enum PackagePriority
    {
        /// <summary>
        /// Indicates a routing installation package
        /// </summary>
        Normal,

        /// <summary>
        /// Indicates a critical update
        /// </summary>
        Critical,
    }

    /// <summary>
    /// Indicates the type of software package
    /// </summary>
    public enum PackageType
    {
        /// <summary>
        /// Baseline package that includes major changes to nebOS
        /// </summary>
        Base,

        /// <summary>
        /// A patch package that resolves a specific issue with nebOS
        /// </summary>
        Patch,
    }

    /// <summary>
    /// A nebulon update package
    ///
    /// <para>
    /// Allows updating nebulon services processing units to a specific nebOS
    /// version. The package information object describes each released nebOS
    /// package.
    /// </para>
    /// </summary>
    public sealed class PackageInfo
    {
        /// <summary>
        /// List of nPod unique identifiers that can install this package
        /// </summary>
        [JsonPath("$.eligiblePods[*].uid", false)]
        public Guid[] EligibleNPodGuids { get; set; }

        /// <summary>
        /// Indicates it the package is deprecated and no longer available
        /// </summary>
        [JsonPath("$.packageDeprecated", true)]
        public bool PackageDeprecated { get; set; }

        /// <summary>
        /// Description for the package
        /// </summary>
        [JsonPath("$.packageDescription", true)]
        public string PackageDescription { get; set; }

        /// <summary>
        /// The name of the nebulon update package
        /// </summary>
        [JsonPath("$.packageName", true)]
        public string PackageName { get; set; }

        /// <summary>
        /// Indicates the importance for installing a nebulon package
        /// </summary>
        [JsonPath("$.packagePriority", true)]
        public PackagePriority PackagePriority { get; set; }

        /// <summary>
        /// The size of the update package in bytes
        /// </summary>
        [JsonPath("$.packageSize", true)]
        public long PackageSizeBytes { get; set; }

        /// <summary>
        /// Type of the nebOS installation package
        /// </summary>
        [JsonPath("$.packageType", true)]
        public PackageType PackageType { get; set; }

        /// <summary>
        /// The patch number if it is a patch
        /// </summary>
        [JsonPath("$.patchNumber", false)]
        public string PatchNumber { get; set; }

        /// <summary>
        /// Describes the prerequisites for the package
        /// </summary>
        [JsonPath("$.prerequisites", true)]
        public string Prerequisites { get; set; }

        /// <summary>
        /// A URL to the release notes of the package
        /// </summary>
        [JsonPath("$.releaseNotesURL", true)]
        public string ReleaseNotesUrl { get; set; }

        /// <summary>
        /// The release date as a UNIX timestamp
        /// </summary>
        [JsonPath("$.releaseUnix", true)]
        public long ReleaseUnix { get; set; }

        /// <summary>
        /// The version number if it is a nebOS package
        /// </summary>
        [JsonPath("$.versionNumber", true)]
        public string VersionNumber { get; set; }
    }

    /// <summary>
    /// Recommended packages for customers
    ///
    /// <para>
    /// Recommended packages indicate the currently recommended packages for
    /// customers given their current version and hardware.
    /// </para>
    /// </summary>
    public sealed class RecommendedPackages
    {
        /// <summary>
        /// Indicates the base version for the recommended package
        /// </summary>
        [JsonPath("$.baseVersion", true)]
        public string BaseVersion { get; set; }

        /// <summary>
        /// Information concerning the update pacakge
        /// </summary>
        [JsonPath("$.packageInfo", false)]
        public PackageInfo PackageInfo { get; set; }

        /// <summary>
        /// The name of the package that is recommended
        /// </summary>
        [JsonPath("$.packageName", true)]
        public string PackageName { get; set; }

        /// <summary>
        /// Indicates the type of SPU for which the package is recommended
        /// </summary>
        [JsonPath("$.spuType", true)]
        public string SpuType { get; set; }
    }

    /// <summary>
    /// An object describing past updates
    /// </summary>
    public sealed class UpdateHistory
    {
        /// <summary>
        /// Date and time when the update completed
        /// </summary>
        [JsonPath("$.finish", false)]
        public DateTime Finish { get; set; }

        /// <summary>
        /// The name of the package that is installed
        /// </summary>
        [JsonPath("$.packageName", true)]
        public string PackageName { get; set; }

        /// <summary>
        /// Date and time when the update started
        /// </summary>
        [JsonPath("$.start", true)]
        public DateTime Start { get; set; }

        /// <summary>
        /// Indicates if the update completed successfully
        /// </summary>
        [JsonPath("$.success", true)]
        public bool Success { get; set; }

        /// <summary>
        /// The identifier of the update
        /// </summary>
        [JsonPath("$.updateID", true)]
        public Guid UpdateGuid { get; set; }
    }

    /// <summary>
    /// An object describing nebOS software packages
    ///
    /// <para>
    /// Describes software packages that are available for installation and
    /// recommended for customers.
    /// </para>
    /// </summary>
    public sealed class UpdatePackages
    {
        /// <summary>
        /// List of available nebulon software packages
        /// </summary>
        [JsonPath("$.available", false)]
        public PackageInfo[] Available { get; set; }

        /// <summary>
        /// The latest available nebulon software package
        /// </summary>
        [JsonPath("$.latest", true)]
        public string Latest { get; set; }

        /// <summary>
        /// List or recommended nebulon software packages
        /// </summary>
        [JsonPath("$.recommended", false)]
        public RecommendedPackages[] Recommended { get; set; }
    }

    /// <summary>
    /// An object describing the current state of an update installation
    /// </summary>
    public sealed class UpdateStateSpu
    {
        /// <summary>
        /// Download progress in percent
        /// </summary>
        [JsonPath("$.downloadProgressPct", true)]
        public long DownloadProgressPercent { get; set; }

        /// <summary>
        /// Contains information about update errors
        /// </summary>
        [JsonPath("$.failureLog", false)]
        public string FailureLog { get; set; }

        /// <summary>
        /// Date and time when the SPU last reported update status
        /// </summary>
        [JsonPath("$.lastChanged", true)]
        public DateTime LastChanged { get; set; }

        /// <summary>
        /// The name of the package that is installed
        /// </summary>
        [JsonPath("$.packageName", true)]
        public string PackageName { get; set; }

        /// <summary>
        /// Indicates if nebOS has completed the restart
        /// </summary>
        [JsonPath("$.restartComplete", true)]
        public bool RestartComplete { get; set; }

        /// <summary>
        /// Indicates if nebOS is restarting
        /// </summary>
        [JsonPath("$.restarting", true)]
        public bool Restarting { get; set; }

        /// <summary>
        /// The serial number for the SPU on which the update is installed
        /// </summary>
        [JsonPath("$.SPUSerial", true)]
        public string SpuSerial { get; set; }

        /// <summary>
        /// Indicates if the SPU has started installing the SPU
        /// </summary>
        [JsonPath("$.startedInstall", true)]
        public bool StartedInstall { get; set; }

        /// <summary>
        /// The identifier of the update
        /// </summary>
        [JsonPath("$.updateID", true)]
        public string UpdateId { get; set; }

        /// <summary>
        /// Indicates if the SPU is waiting for a SPU to complete its update
        /// </summary>
        [JsonPath("$.waitingForSPUSerial", true)]
        public string WaitingForSpuSerial { get; set; }
    }
}