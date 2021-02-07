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
    /// A filter object to filter on Guid values
    ///
    /// <para>
    /// Allows filtering on <c>Guid</c> value types. The filter allows only one
    /// property to be specified. If filtering on multiple properties is
    /// needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public class GuidFilter
    {
        /// <summary>
        /// Allows concatenation with another GuidFilter with a logical <c>AND</c>
        /// </summary>
        [JsonPath("$.and")]
        public GuidFilter And { get; set; }

        /// <summary>
        /// Matches if a Guid is on of a provided list of values
        /// </summary>
        [JsonPath("$.in")]
        public Guid[] In { get; set; }

        /// <summary>
        /// Filters Guids by being an exact match
        /// </summary>
        [JsonPath("$.equals")]
        public Guid? MustEqual { get; set; }

        /// <summary>
        /// Filters Guids by not being an exact match
        /// </summary>
        [JsonPath("$.notEquals")]
        public Guid? MustNotEqual { get; set; }

        /// <summary>
        /// Allows concatenation with another GuidFilter with a logical <c>OR</c>
        /// </summary>
        [JsonPath("$.or")]
        public GuidFilter Or { get; set; }
    }

    /// <summary>
    /// Defines filtering options for the Integer type.
    ///
    /// <para>
    /// Allows filtering on <c>Integer</c> value types. The filter allows only one
    /// property to be specified. If filtering on multiple properties is
    /// needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public sealed class IntFilter
    {
        /// <summary>
        /// Allows concatenation with another int filer with a logical <c>AND</c>
        /// </summary>
        [JsonPath("$.and")]
        public IntFilter And { get; set; }

        /// <summary>
        /// Filters values with a <c>&gt;</c> comparison
        /// </summary>
        [JsonPath("$.greaterThan")]
        public long? GreaterThan { get; set; }

        /// <summary>
        /// Filters values with a <c>&#8805;</c> comparison
        /// </summary>
        [JsonPath("$.greaterThanEquals")]
        public long? GreaterThanEquals { get; set; }

        /// <summary>
        /// Matches if an Integer is on of a provided list of values
        /// </summary>
        [JsonPath("$.in")]
        public long[] In { get; set; }

        /// <summary>
        /// Filters values with a <c>&lt;</c> comparison
        /// </summary>
        [JsonPath("$.lessThan")]
        public long? LessThan { get; set; }

        /// <summary>
        /// Filters values with a <c>&#8804;</c> comparison
        /// </summary>
        [JsonPath("$.lessThanEquals")]
        public long? LessThanEquals { get; set; }

        /// <summary>
        /// Filters values with a <c>==</c> comparison
        /// </summary>
        [JsonPath("$.equals")]
        public long? MustEqual { get; set; }

        /// <summary>
        /// Filters values with a <c>!=</c> comparison
        /// </summary>
        [JsonPath("$.notEquals")]
        public long? MustNotEqual { get; set; }

        /// <summary>
        /// Allows concatenation with another int filer with a logical <c>OR</c>
        /// </summary>
        [JsonPath("$.or")]
        public IntFilter Or { get; set; }
    }

    /// <summary>
    /// A filter object to filter items based on String values
    ///
    /// <para>
    /// Allows filtering on <c>str</c> value types. The filter allows only one
    /// property to be specified.If filtering on multiple properties is
    /// needed, use the <c>And</c> and <c>Or</c> options to concatenate
    /// multiple filters.
    /// </para>
    /// </summary>
    public class StringFilter
    {
        /// <summary>
        /// Allows concatenation with another string filer with a logical <c>AND</c>
        /// </summary>
        [JsonPath("$.and")]
        public StringFilter And { get; set; }

        /// <summary>
        /// Matches if a string begins with a provided value
        /// </summary>
        [JsonPath("$.beginsWith")]
        public string BeginsWith { get; set; }

        /// <summary>
        /// Matches if a string contains a provided value
        /// </summary>
        [JsonPath("$.contains")]
        public string Contains { get; set; }

        /// <summary>
        /// Matches if a string ends with a provided value
        /// </summary>
        [JsonPath("$.endsWith")]
        public string EndsWith { get; set; }

        /// <summary>
        /// Matches if a string is on of a provided list of values
        /// </summary>
        [JsonPath("$.in")]
        public string[] In { get; set; }

        /// <summary>
        /// Filters strings by an exact match
        /// </summary>
        [JsonPath("$.equals")]
        public string MustEqual { get; set; }

        /// <summary>
        /// "Filters strings by not being an exact match
        /// </summary>
        [JsonPath("$.notEquals")]
        public string MustNotEqual { get; set; }

        /// <summary>
        /// Matches if a string does not contain a provided value
        /// </summary>
        [JsonPath("$.notContains")]
        public string NotContains { get; set; }

        /// <summary>
        /// Allows concatenation with another string filer with a logical <c>OR</c>
        /// </summary>
        [JsonPath("$.or")]
        public StringFilter Or { get; set; }

        /// <summary>
        /// Matches by use of a regular expression
        /// </summary>
        [JsonPath("$.regex")]
        public string Regex { get; set; }
    }
}