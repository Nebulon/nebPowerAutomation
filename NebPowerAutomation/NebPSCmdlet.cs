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
using System;
using System.Management.Automation;

namespace NebPowerAutomation
{
    /// <summary>
    /// A Nebulon PowerShell Cmdlet
    /// </summary>
    public class NebPSCmdlet : PSCmdlet
    {
        /// <summary>
        /// Keeps track of the logger. If true, the logger is configured.
        /// </summary>
        private bool _isLogSetup = false;

        /// <summary>
        /// Log level for the PowerShell logger
        /// </summary>
        public LogSeverity LogLevel { get; set; }

        /// <summary>
        /// A nebulon ON connection to use for the command execution.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        protected NebConnection Connection { get; set; }

        /// <summary>
        /// Performns initialization of the execution
        /// </summary>
        protected override void BeginProcessing()
        {
            LoadConnection();
            LoadLogging();
        }

        /// <summary>
        /// Check if a specific property was present during invocation
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property
        /// </param>
        /// <returns></returns>
        protected bool ParameterPresent(string propertyName)
        {
            return MyInvocation.BoundParameters.ContainsKey(propertyName);
        }

        /// <summary>
        /// Allows writing an exception to the console
        /// </summary>
        /// <param name="ex">
        /// An exception to unpack and write to the console
        /// </param>
        protected void WriteError(Exception ex)
        {
            ErrorRecord record = new ErrorRecord(
                ex,
                ex.GetType().ToString(),
                ErrorCategory.NotSpecified,
                null);

            WriteError(record);
        }

        /// <summary>
        /// Sets the nebulon connection if it is set globally in the
        /// PowerShell session and fails if no connection is found.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If no connection is established
        /// </exception>
        private void LoadConnection()
        {
            if (Connection == null)
                Connection = (NebConnection)SessionState
                    .PSVariable
                    .GetValue("NebulonConnection");

            if (Connection == null)
                throw new ArgumentException("Please login first");
        }

        /// <summary>
        /// Configures the PowerShell log with the requested verbosity level
        /// </summary>
        private void LoadLogging()
        {
            if (!_isLogSetup)
            {
                Connection.Logger = new PowerShellLogger(this);
                _isLogSetup = true;
            }

            if (Connection.Logger == null)
                return;

            // set the default logging level so that there is always a level
            // defined. We set it by default to the highest level.
            Connection.Logger.LogLevel = LogSeverity.Error;

            // add support for the default parameter '-Verbose'
            if (ParameterPresent("Verbose"))
                Connection.Logger.LogLevel = LogSeverity.Verbose;

            // add support for the default parameter '-Debug'
            if (ParameterPresent("Debug"))
                Connection.Logger.LogLevel = LogSeverity.Debug;
        }
    }
}