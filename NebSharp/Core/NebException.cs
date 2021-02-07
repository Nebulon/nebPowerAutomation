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
    /// Wrapper for standard System.Exception for nebulon specific errors.
    /// </summary>
    public class NebException : Exception
    {
        /// <summary>
        /// Instanciate a new NebException
        /// </summary>
        public NebException() : base() { }

        /// <summary>
        /// Instanciate a new NebException
        /// </summary>
        /// <param name="message">Message describing the error</param>
        public NebException(string message) : base(message) { }

        /// <summary>
        /// Instanciate a new NebException
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="innerException"></param>
        public NebException(string message, Exception innerException) : base(message, innerException) { }
    }
}