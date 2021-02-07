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

namespace NebSharp.Types
{
    /// <summary>
    /// Metadata information that describes UCAPI
    /// </summary>
    public sealed class Metadata
    {
        /// <summary>
        /// Metadata information for role-based access control
        /// </summary>
        [JsonPath("$.rbac", true)]
        public RBACMetadata RBAC { get; set; }

        /// <summary>
        /// Metadata information for support
        /// </summary>
        [JsonPath("$.support", true)]
        public SupportMetadata SupportMetadata { get; set; }
    }

    /// <summary>
    /// Metadata information that describes API options
    /// </summary>
    public sealed class MetaValue
    {
        /// <summary>
        /// Actual value
        /// </summary>
        [JsonPath("$.actual", true)]
        public string Actual { get; set; }

        /// <summary>
        /// Description of the value
        /// </summary>
        [JsonPath("$.description", true)]
        public string Description { get; set; }
    }

    /// <summary>
    /// Metadata information for role-based access control
    /// </summary>
    public sealed class RBACMetadata
    {
        /// <summary>
        /// List possible permissions
        /// </summary>
        [JsonPath("$.permissions", true)]
        public MetaValue[] Permissions { get; set; }

        /// <summary>
        /// List of possible resource types
        /// </summary>
        [JsonPath("$.resourceTypes", true)]
        public MetaValue[] ResourceTypes { get; set; }
    }

    /// <summary>
    /// Metadata information for Support options
    /// </summary>
    public sealed class SupportMetadata
    {
        /// <summary>
        /// List of possible resource types
        /// </summary>
        [JsonPath("$.resourceTypes", true)]
        public MetaValue[] ResourceTypes { get; set; }
    }
}