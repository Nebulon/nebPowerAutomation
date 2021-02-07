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

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Allows querying for the current login state
        /// </summary>
        /// <returns>Session state</returns>
        public LoginState GetSessionState()
        {
            return RunQuery<LoginState>(@"loginStatus");
        }

        /// <summary>
        /// Login to nebulon ON
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Login request result</returns>
        public LoginResults Login(string username, string password)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add(@"username", username);
            parameters.Add(@"password", password);

            return RunMutation<LoginResults>(@"login", parameters);
        }

        /// <summary>
        /// Logout from nebulon ON
        /// <returns>If the logout request was successful</returns>
        /// </summary>
        public bool Logout()
        {
            return RunMutation<bool>(@"logout");
        }
    }
}