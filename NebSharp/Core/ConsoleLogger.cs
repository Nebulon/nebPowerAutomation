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

namespace NebSharp.Core
{
    /// <summary>
    /// Implementation of ILogger that writes to the Console output.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Instantiate a new logger that writes to the standard console.
        /// </summary>
        /// <param name="logLevel">Logging level</param>
        public ConsoleLogger(LogSeverity logLevel)
        {
            this.LogLevel = logLevel;
        }

        /// <summary>
        /// Instantiate a new logger that writes to the standard console.
        /// </summary>
        public ConsoleLogger()
        {
            this.LogLevel = LogSeverity.Error;
        }

        /// <summary>
        /// Logging level for this instance of a log writer.
        /// </summary>
        public LogSeverity LogLevel { get; set; }

        /// <summary>
        /// Write a debuging message.
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        public void WriteDebug(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Debug)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, " DEBUG   ", message);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        /// <summary>
        /// Write an error message
        /// </summary>
        /// <param name="message">
        /// The error message to print
        /// </param>
        public void WriteError(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Error)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, " ERROR   ", message);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        /// <summary>
        /// Write a progress update
        /// </summary>
        /// <param name="activity">
        /// The name of the activity that is updated
        /// </param>
        /// <param name="percent">
        /// The current activity status in percent (0-100)
        /// </param>
        public void WriteProgress(string activity, int percent)
        {
            // compile the percentage bar
            string pct = string.Concat(percent, "% ").PadLeft(5);

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat("\r", timestamp, pct, activity);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(output);
            Console.ResetColor();
        }

        /// <summary>
        /// Write a verbose message
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        public void WriteVerbose(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Verbose)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, " VERBOSE ", message);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        /// <summary>
        /// Write a warning message
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        public void WriteWarning(string message)
        {
            // check if the log level is set to the appropriate level
            if ((int)LogLevel > (int)LogSeverity.Warning)
                return;

            // get the current time stamp and compose the output string
            string timestamp = GetTimestamp();
            string output = string.Concat(timestamp, " WARNING ", message);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        /// <summary>
        /// Returns the current date and time in the format [yyyy-MM-dd HH:mm:ss].
        /// This uses the local time zone of the user's computer.
        /// </summary>
        /// <returns>
        /// Current date and time in a standard format.
        /// </returns>
        private string GetTimestamp()
        {
            return string.Concat(
                "[",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "]"
            );
        }
    }
}