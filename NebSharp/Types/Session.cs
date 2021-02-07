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
    /// Result of a login request
    /// </summary>
    public class LoginResults
    {
        /// <summary>
        /// Indicates if a user in the org has accepted the EULA
        /// </summary>
        [JsonPath("$.eulaAccepted", true)]
        public bool EulaAccepted { get; set; }

        /// <summary>
        /// Describes when the session will expire
        /// </summary>
        [JsonPath("$.expiration", true)]
        public string Expiration { get; set; }

        /// <summary>
        /// A message describing the login result
        /// </summary>
        [JsonPath("$.message", true)]
        public string Message { get; set; }

        /// <summary>
        /// The name of the organization the user logged in
        /// </summary>
        [JsonPath("$.organizationName")]
        public string Organization { get; set; }

        /// <summary>
        /// User preferences associated with the logged in user
        /// </summary>
        [JsonPath("$.userPreferences", true)]
        public UserPreferences Preferences { get; set; }

        /// <summary>
        /// Indicates if the login was successful
        /// </summary>
        [JsonPath("$.success", true)]
        public bool Success { get; set; }

        /// <summary>
        /// The unique identifier of the user that logged in
        /// </summary>
        [JsonPath("$.userUID", true)]
        public Guid UserGuid { get; set; }
    }

    /// <summary>
    /// Represents the session state of a user
    /// </summary>
    public class LoginState
    {
        /// <summary>
        /// Describes when the session expires
        /// </summary>
        [JsonPath("$.expiration", true)]
        public string Expiration { get; set; }

        /// <summary>
        /// Name of the organization for this session
        /// </summary>
        [JsonPath("$.organization", true)]
        public string Organization { get; set; }

        /// <summary>
        /// The user identifier of the user for this session
        /// </summary>
        [JsonPath("$.userUID", true)]
        public Guid UserGuid { get; set; }

        /// <summary>
        /// The name of the user for this session
        /// </summary>
        [JsonPath("$.username", true)]
        public string Username { get; set; }
    }
}