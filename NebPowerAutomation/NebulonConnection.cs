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

using NebPowerAutomation.Core;
using NebSharp;
using NebSharp.Types;
using System;
using System.Management.Automation;
using System.Net;

namespace NebPowerAutomation
{
    /// <summary>
    /// <para type="synopsis">
    /// Get information about the currently used nebulon ON connection
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NebulonConnectionState")]
    [OutputType(typeof(LoginState))]
    public class GetLoginStatus : NebPSCmdlet
    {
        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                LoginState state = Connection.GetSessionState();
                WriteObject(state);
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
    /// Creates a new connection to nebulon ON
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NebulonConnection")]
    [OutputType(typeof(NebConnection))]
    public class NewNebulonConnection : PSCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// A credential object to login to nebulon ON
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "Credentials", ValueFromPipeline = true)]
        public PSCredential Credential { get; set; }

        /// <summary>
        /// <para type="description">
        /// The username to use for login
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "UserName")]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">
        /// The password to use for login
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                // create a new connection. This will overwrite any existing
                // connection that is already established
                NebConnection connection = new NebConnection();
                connection.Logger = new PowerShellLogger(this);

                // add support for the default parameter '-Verbose'
                if (MyInvocation.BoundParameters.ContainsKey("Verbose"))
                    connection.Logger.LogLevel = LogSeverity.Verbose;

                // add support for the default parameter '-Debug'
                if (MyInvocation.BoundParameters.ContainsKey("Debug"))
                    connection.Logger.LogLevel = LogSeverity.Debug;

                NetworkCredential credential = ParameterSetName == @"UserName"
                    ? new NetworkCredential(UserName, Password)
                    : new NetworkCredential(Credential.UserName, Credential.Password);

                LoginResults loginResults = connection.Login(credential.UserName, credential.Password);

                WriteVerbose(loginResults.Message);
                WriteVerbose(loginResults.Organization);

                // login was successful so we can store the connection as
                // a global variable
                SessionState.PSVariable.Set(
                    new PSVariable("NebulonConnection",
                    connection,
                    ScopedItemOptions.AllScope));

                WriteObject(connection);
            }
            catch (AggregateException exceptions)
            {
                foreach (Exception ex in exceptions.InnerExceptions)
                {
                    ErrorRecord record = new ErrorRecord(
                        ex,
                        ex.GetType().ToString(),
                        ErrorCategory.NotSpecified,
                        null);

                    WriteError(record);
                }
            }
            catch (Exception ex)
            {
                ErrorRecord record = new ErrorRecord(
                    ex,
                    ex.GetType().ToString(),
                    ErrorCategory.NotSpecified,
                    null);

                WriteError(record);
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">
    /// Disconnect and delete the currently used Nebulon ON connection
    /// </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "NebulonConnection")]
    [OutputType(typeof(void))]
    public class RemoveNebulonConnection : NebPSCmdlet
    {
        /// <summary>
        /// Performs execution of the command
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                // first logout
                Connection.Logout();

                // remove the session from the global session store
                SessionState.PSVariable.Remove("NebulonConnection");
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