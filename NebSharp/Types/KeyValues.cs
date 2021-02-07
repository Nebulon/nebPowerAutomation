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
    /// An input object to delete a key value entry
    /// </summary>
    public class DeleteKeyValueInput
    {
        /// <summary>
        /// Metadata key
        /// </summary>
        [JsonPath("$.key", true)]
        public string Key { get; set; }

        /// <summary>
        /// "nPod Group identifier
        /// </summary>
        [JsonPath("$.nPodGroupUUID", true)]
        public Guid NPodGroupGuid { get; set; }

        /// <summary>
        /// Identifier of the resource for the key value entry
        /// </summary>
        [JsonPath("$.resourceUUID", true)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Type of resource for the key value data
        /// </summary>
        [JsonPath("$.resourceType", true)]
        public ResourceType ResourceType { get; set; }
    }

    /// <summary>
    /// Key value pair
    ///
    /// <para>
    /// Represents metadata assigned to various resources in nebulon ON in the form
    /// of key value pairs.This metadata can be used by customers to add arbitrary
    /// text information to resources that are not part of the default resource
    /// properties.
    /// </para>
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        /// Metadata key
        /// </summary>
        [JsonPath("$.key", true)]
        public string Key { get; set; }

        /// <summary>
        /// Metadata value
        /// </summary>
        [JsonPath("$.value", true)]
        public string Value { get; set; }
    }

    /// <summary>
    /// A filter object to filter key value objects.
    ///
    /// <para>
    /// Allows filtering for specific key value objects in nebulon ON.The
    /// filter allows only one property to be specified. If filtering on multiple
    /// properties is needed, use the <c>And</c> and <c>Or</c> options to
    /// concatenate multiple filters.
    /// </para>
    /// </summary>
    public class KeyValueFilter
    {
        /// <summary>
        /// Filter based on the key name
        /// </summary>
        [JsonPath("$.keyName", false)]
        public StringFilter KeyName { get; set; }

        /// <summary>
        /// Filter based on the associated nPod group
        /// </summary>
        [JsonPath("$.nPodGroupUUID", true)]
        public Guid NPodGroupGuid { get; set; }

        /// <summary>
        /// Filter based on the associated Resource
        /// </summary>
        [JsonPath("$.resourceUUID", true)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Filter based on the associated resource type
        /// </summary>
        [JsonPath("$.resourceType", true)]
        public ResourceType ResourceType { get; set; }
    }

    /// <summary>
    /// List of key value pairs
    /// </summary>
    public class KeyValueList
    {
        /// <summary>
        /// Indicates the total amount of items defined on the server
        /// that match a filter input object.
        /// </summary>
        [JsonPath("$.filteredCount", true)]
        public long FilteredCount { get; set; }

        /// <summary>
        /// Contains the items returned in the page
        /// </summary>
        [JsonPath("$.items", true)]
        public KeyValue[] Items { get; set; }

        /// <summary>
        /// Indicates the total amount of items defined on the server
        /// </summary>
        [JsonPath("$.totalCount", true)]
        public long TotalCount { get; set; }
    }

    /// <summary>
    /// An input object to create or update a key value entry.
    ///
    /// <para>
    /// Allows adding metadata to various resources in nebulon ON in the form of
    /// key value pairs. This metadata can be used by customers to add arbitrary
    /// text information to resources that are not part of the default resource
    /// properties.
    /// </para>
    /// </summary>
    public class UpsertKeyValueInput
    {
        /// <summary>
        /// Metadata key
        /// </summary>
        [JsonPath("$.key", true)]
        public string Key { get; set; }

        /// <summary>
        /// nPod Group identifier
        /// </summary>
        [JsonPath("$.nPodGroupUUID", true)]
        public Guid NPodGroupId { get; set; }

        /// <summary>
        /// "Identifier of the resource for the key value entry
        /// </summary>
        [JsonPath("$.resourceUUID", true)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Type of resource for the key value data
        /// </summary>
        [JsonPath("$.resourceType", true)]
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Metadata value
        /// </summary>
        [JsonPath("$.value", true)]
        public string Value { get; set; }
    }
}