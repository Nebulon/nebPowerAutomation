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
    /// Used in mutations for on-premises infrastructure via security triangle.
    ///
    /// <para>
    /// Represents a definition of SPUs that a security token needs to be sent to.
    /// </para>
    /// </summary>
    public sealed class MustSendTargetDns
    {
        /// <summary>
        /// The DNS name of the SPU's control port
        /// </summary>
        [JsonPath("$.controlPortDNS", true)]
        public string ControlPortDns { get; set; }

        /// <summary>
        /// List of DNS names of the SPU's data ports
        /// </summary>
        [JsonPath("$.dataPortDNS", true)]
        public string[] DataPortDns { get; set; }
    }

    /// <summary>
    /// Token response when creating a new nPod
    /// </summary>
    public sealed class NPodTokenResponse
    {
        /// <summary>
        /// List of errors and warnings associated with the mutation
        /// </summary>
        [JsonPath("$.IssuesRes", false)]
        public Issues Issues { get; set; }

        /// <summary>
        /// Token that needs to be delivered to on-premises SPUs
        /// </summary>
        [JsonPath("$.tokenResp", false)]
        public TokenResponse TokenResponse { get; set; }
    }

    /// <summary>
    /// Used in mutations for on-premises infrastructure via security triangle
    ///
    /// <para>
    /// Represents a response for a mutation that alters the customers'
    /// on-premises infrastructure and requires the completion of the security
    /// triangle.
    /// </para>
    /// </summary>
    public sealed class TokenResponse
    {
        /// <summary>
        /// List of errors and warnings associated with the mutation
        /// </summary>
        [JsonPath("$.issues", false)]
        public Issues[] Issues { get; set; }

        /// <summary>
        /// List of data IP addresses of SPUs involved in the mutation
        /// </summary>
        [JsonPath("$.mustSendTargetDNS", true)]
        public MustSendTargetDns[] MustSendTargetDns { get; set; }

        /// <summary>
        /// List of data IP addresses of SPUs involved in the mutation
        /// </summary>
        [JsonPath("$.dataTargetIPs", false)]
        public string[] TargetDataIps { get; set; }

        /// <summary>
        /// List of control IP addresses of SPUs involved in the mutation
        /// </summary>
        [JsonPath("$.targetIPs", true)]
        public string[] TargetIps { get; set; }

        /// <summary>
        /// Token that needs to be delivered to on-premises SPUs
        /// </summary>
        [JsonPath("$.token", true)]
        public string Token { get; set; }

        /// <summary>
        /// Unique identifier of the resource that is about to be created
        /// </summary>
        [JsonPath("$.waitOn", true)]
        public Guid WaitOn { get; set; }
    }
}