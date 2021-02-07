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

using NebSharp;
using NebSharp.Core;
using System;
using System.Management.Automation;

namespace NebPowerAutomation.Core
{
    internal class PowerShellLogger : ILogger
    {
        private Cmdlet _cmdlet;

        /// <summary>
        /// Instantiate a new logger that writes to PowerShell
        /// </summary>
        /// <param name="cmdlet"></param>
        public PowerShellLogger(Cmdlet cmdlet)
        {
            _cmdlet = cmdlet;

            LogLevel = LogSeverity.Error;
        }

        /// <summary>
        /// Instantiate a new logger that writes to PowerShell
        /// </summary>
        /// <param name="cmdlet"></param>
        /// <param name="logLevel"></param>
        public PowerShellLogger(Cmdlet cmdlet, LogSeverity logLevel)
        {
            _cmdlet = cmdlet;

            LogLevel = logLevel;
        }

        public LogSeverity LogLevel { get; set; }

        public void WriteDebug(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Debug)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, message);

            _cmdlet.WriteDebug(output);
        }

        public void WriteError(string error)
        {
            Exception ex = new Exception(error);

            ErrorRecord record = new ErrorRecord(
                ex,
                "Error",
                ErrorCategory.NotSpecified,
                null);

            _cmdlet.WriteError(record);
        }

        public void WriteProgress(string activity, int percent)
        {
            ProgressRecord progressRecord = new ProgressRecord(
                0x0,
                activity,
                activity);

            progressRecord.PercentComplete = percent;
            _cmdlet.WriteProgress(progressRecord);
        }

        public void WriteVerbose(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Verbose)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, message);

            _cmdlet.WriteVerbose(output);
        }

        public void WriteWarning(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Warning)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, message);

            _cmdlet.WriteWarning(output);
        }

        /// <summary>
        /// Returns the current date and time in the format [yyyy-MM-dd HH:mm:ss].
        /// This uses the local time zone of the user's computer.
        /// </summary>
        /// <returns>Current date and time in a standard format.</returns>
        private string GetTimestamp()
        {
            return string.Concat("[", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "] ");
        }
    }
}