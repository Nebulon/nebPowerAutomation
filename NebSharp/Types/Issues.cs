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
    /// An issue instance
    ///
    /// <para>
    /// Issue instances describe issues with certain nebulon ON commands as part
    /// of a pre-execution step.Issues can be of type warning or errors.
    /// Warnings may be ignored (although not recommended), whereas errors are
    /// blocking users from continuing with the main execution.
    /// </para>
    /// </summary>
    public sealed class IssueInstance
    {
        /// <summary>
        /// Error or warning message of the issue instance
        /// </summary>
        [JsonPath("$.message", true)]
        public string Message { get; set; }

        /// <summary>
        /// List of SPU serial numbers associated with the issue instance
        /// </summary>
        [JsonPath("$.spuSerials", true)]
        public string[] SpuSerials { get; set; }
    }

    /// <summary>
    /// List of issue instances
    ///
    /// <para>
    /// Issues describe issues with certain nebulon ON commands as part
    /// of a pre-execution step.Issues can be of type warning or errors.
    /// Warnings may be ignored (although not recommended), whereas errors are
    /// blocking users from continuing with the main execution.
    /// </para>
    /// </summary>
    public sealed class Issues
    {
        /// <summary>
        /// List or errors. Errors need to be resolved before continuing
        /// </summary>
        [JsonPath("$.errors", true)]
        public IssueInstance[] Errors { get; set; }

        /// <summary>
        /// List of warnings. Warnings can be ignored although not recommended
        /// </summary>
        [JsonPath("$.warnings", true)]
        public IssueInstance[] Warnings { get; set; }

        /// <summary>
        /// Utility method to check for issues
        ///
        /// <para>
        /// Method checks if there are any warnings or errors present in the
        /// list of issues. If there are any issues of type error, an exception
        /// is raised. If there are any warnings and <c>ignoreWarnings</c> is
        /// set to <c>false</c> an exception is thrown, if set to <c>true</c>,
        /// warnings are ignored.
        /// </para>
        /// </summary>
        /// <param name="ignoreWarnings">
        ///  If set to <c>true</c> warnings are silently ignored. By default
        ///  warnings are not ignored
        /// </param>
        public void AssertNoIssues(bool ignoreWarnings)
        {
            // check for errors
            int errorCount = this.Errors.Length;
            if (errorCount > 0)
            {
                string msg = $"validation failed with {errorCount} errors: ";
                foreach (IssueInstance error in this.Errors)
                {
                    msg += error.Message;

                    if (error.SpuSerials != null && error.SpuSerials.Length > 0)
                    {
                        msg += string.Concat(" (", string.Join(", ", error.SpuSerials), ") ");
                    }
                }

                throw new Exception(msg);
            }

            // check for warnings
            int warningCount = this.Warnings.Length;
            if (warningCount > 0 && !ignoreWarnings)
            {
                string msg = $"validation failed with {errorCount} warnings: ";
                foreach (IssueInstance warning in this.Warnings)
                {
                    msg += warning.Message;

                    if (warning.SpuSerials != null && warning.SpuSerials.Length > 0)
                    {
                        msg += string.Concat(" (", string.Join(", ", warning.SpuSerials), ") ");
                    }
                }

                throw new Exception(msg);
            }
        }
    }
}