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
    /// An input object to create a new nPod group
    ///
    /// <para>
    /// Allows creation of a new nPod group object in nebulon ON. A nPod
    /// group allows logical grouping of nPods into security domains. Each nPod
    /// group can receive custom security policies.
    /// </para>
    /// </summary>
    public sealed class CreateNPodGroupInput
    {
        /// <summary>
        /// The name of the nPod group
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// The note for the nPod group
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }

    /// <summary>
    /// A group of nPods
    ///
    /// <para>
    /// A nPod group allows logical grouping of nPods into security domains.
    /// Each nPod group can receive custom security policies and contain
    /// multiple nPods.
    /// </para>
    /// </summary>
    public sealed class NPodGroup
    {
        /// <summary>
        /// The unique identifier for the nPod group
        /// </summary>
        [JsonPath("$.uuid", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Number of hosts (servers) in this nPod group
        /// </summary>
        [JsonPath("$.hostCount", true)]
        public long HostCount { get; set; }

        /// <summary>
        /// The name of the nPod group
        /// </summary>
        [JsonPath("$.name", true)]
        public string Name { get; set; }

        /// <summary>
        /// An optional note for the nPod group
        /// </summary>
        [JsonPath("$.note", true)]
        public string Note { get; set; }

        /// <summary>
        /// Number of nPods in this nPod group
        /// </summary>
        [JsonPath("$.nPodCount", true)]
        public long NPodCount { get; set; }

        /// <summary>
        /// List of nPod unique identifiers in this nPod group
        /// </summary>
        [JsonPath("$.nPods[*].uuid", false)]
        public Guid[] NPodGuids { get; set; }

        /// <summary>
        /// Number of services processing units in this nPod group
        /// </summary>
        [JsonPath("$.spuCount", true)]
        public long SpuCount { get; set; }
    }

    /// <summary>
    /// A filter object to filter nPod groups.
    ///
    /// <para>
    /// Allows filtering for specific nPod groups in nebulon ON. The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public sealed class NPodGroupFilter
    {
        /// <summary>
        /// Allows concatenation of multiple filters via logical AND
        /// </summary>
        [JsonPath("$.and", false)]
        public NPodGroupFilter And { get; set; }

        /// <summary>
        /// Filter based on nPod group unique identifier
        /// </summary>
        [JsonPath("$.uuid", false)]
        public GuidFilter Guid { get; set; }

        /// <summary>
        /// Filter based on nPod group name
        /// </summary>
        [JsonPath("$.name", false)]
        public StringFilter Name { get; set; }

        /// <summary>
        /// Allows concatenation of multiple filters via logical OR
        /// </summary>
        [JsonPath("$.or", false)]
        public NPodGroupFilter Or { get; set; }
    }

    /// <summary>
    /// Paginated nPod group list object
    ///
    /// <para>
    /// Contains a list of nPod group objects and information for
    /// pagination. By default a single page includes a maximum of <c>100</c>
    /// items unless specified otherwise in the paginated query.
    /// </para>
    /// <para>
    /// Consumers should always check for the property <c>More</c> as per default
    /// the server does not return the full list of alerts but only one page.
    /// </para>
    /// </summary>
    public sealed class NPodGroupList : PageList<NPodGroup>
    {
    }

    /// <summary>
    /// A sort object for nPod groups
    ///
    /// <para>
    /// Allows sorting nPod groups on common properties. The sort object allows
    /// only one property to be specified
    /// </para>
    /// </summary>
    public sealed class NPodGroupSort
    {
        /// <summary>
        /// Sort direction for <c>Name</c> property
        /// </summary>
        [JsonPath("$.name", false)]
        public SortDirection Name { get; set; }
    }

    /// <summary>
    /// An input object to update nPod group properties
    ///
    /// <para>
    /// Allows updating of an existing nPod group object in nebulon ON. A nPod
    /// group allows logical grouping of nPods into security domains. Each nPod
    /// group can receive custom security policies.
    /// </para>
    /// </summary>
    public sealed class UpdateNPodGroupInput
    {
        /// <summary>
        /// The new name of the nPod group
        /// </summary>
        [JsonPath("$.name", false)]
        public string Name { get; set; }

        /// <summary>
        /// The new note for the nPod group
        /// </summary>
        [JsonPath("$.note", false)]
        public string Note { get; set; }
    }
}