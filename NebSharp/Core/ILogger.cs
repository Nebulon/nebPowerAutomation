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


namespace NebSharp.Core
{
    /// <summary>
    /// Interface definition for log output.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logging level for this instance of a log writer.
        /// </summary>
        LogSeverity LogLevel { get; set; }

        /// <summary>
        /// Write a debuging message.
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        void WriteDebug(string message);

        /// <summary>
        /// Write a verbose message
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        void WriteVerbose(string message);

        /// <summary>
        /// Write a warning message
        /// </summary>
        /// <param name="message">
        /// The message to print
        /// </param>
        void WriteWarning(string message);

        /// <summary>
        /// Write an error message
        /// </summary>
        /// <param name="message">
        /// The error message to print
        /// </param>
        void WriteError(string message);

        /// <summary>
        /// Write a progress update
        /// </summary>
        /// <param name="activity">
        /// The name of the activity that is updated
        /// </param>
        /// <param name="percent">
        /// The current activity status in percent (0-100)
        /// </param>
        void WriteProgress(string activity, int percent);
    }
}
