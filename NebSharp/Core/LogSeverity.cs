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

namespace NebSharp
{
    /// <summary>
    /// Severity of a message written to the log
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// Debug information
        /// </summary>
        Debug = 0x0,

        /// <summary>
        /// Verbose information
        /// </summary>
        Verbose = 0x1,

        /// <summary>
        /// Warnings
        /// </summary>
        Warning = 0x2,

        /// <summary>
        /// Errors
        /// </summary>
        Error = 0x3
    }
}