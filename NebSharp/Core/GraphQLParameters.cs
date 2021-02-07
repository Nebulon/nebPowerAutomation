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

using NebSharp.Core;
using NebSharp.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace NebSharp
{
    /// <summary>
    /// Parameter definition for a GraphQL command (mutation or query).
    /// </summary>
    public class GraphQLParameters : Dictionary<string, object>
    {
        /// <summary>
        /// Defines the maximum recursion depth when resolving parameters.
        /// </summary>
        private const int MAX_RECURSION_DEPTH = 10;

        /// <summary>
        /// Adds the specified key and value to the dictionary if the value is 
        /// not null and optional is set to true, the element will not be added.
        /// </summary>
        /// <param name="key">
        /// The key of the element to add
        /// </param>
        /// <param name="value">
        /// The value of the element to add
        /// </param>
        /// <param name="optional">
        /// If true, the element will not be added when the value is null
        /// </param>
        public void Add(string key, object value, bool optional)
        {
            // if the parameter is optional and not defined, don't add it
            // to the list.
            if (optional && value == null)
                return;

            Add(key, value);
        }

        private static string FormatGraphQLValue(object value, int depth = 0)
        {
            // some types require recursion as they can't be directly 
            // converted. There is a maximum depth we're using to avoid stack
            // overflow. For null values we just return an empty string.
            if (value == null || depth > MAX_RECURSION_DEPTH)
                return @"";

            // string is probably the most common one.
            // for strings, we need to add " and escape quotes.
            if (value is string)
            {
                string input = (string)value;
                string output = string.Concat("\"", input.Replace("\"", "\\\""), "\"");
                return output;
            }

            // GUIDs are a special type of string
            if (value is Guid)
            {
                Guid input = (Guid)value;
                string output = string.Concat("\"", input.ToString(), "\"");
                return output;
            }

            // boolean are expected to be lower case.
            if (value is bool?)
            {
                bool? input = (bool?)value;

                if (input.HasValue)
                {
                    string output = input.ToString().ToLowerInvariant();
                    return output;
                }

                return "";
            }

            // boolean are expected to be lower case.
            if (value is bool)
            {
                bool input = (bool)value;
                string output = input.ToString().ToLowerInvariant();
                return output;
            }

            // DateTime values are expected to be in JSON format.
            if (value is DateTime)
            {
                DateTime input = (DateTime)value;
                string output = string.Concat("\"", input.ToString("yyyy-MM-ddTHH:MM:ss.sssZ"), "\"");
                return output;
            }

            // numbers can be directly converted using their internal to string.
            if (value is ValueType)
            {
                if (value is byte ||
                    value is short ||
                    value is int ||
                    value is long ||
                    value is sbyte ||
                    value is ushort ||
                    value is uint ||
                    value is ulong ||
                    value is float ||
                    value is double ||
                    value is decimal)
                {
                    string output = value.ToString();
                    return output;
                }
            }

            // enumerations can be directly converted. This only works if the
            // enumeration in C# is named exactly as in the GraphQL API.
            if (value is Enum)
            {
                string output = value.ToString();
                return output;
            }

            // for arrays we don't care about the array's child types as they 
            // will be handled by a recursive function 
            if (value is Array)
            {
                IEnumerable input = value as IEnumerable;

                List<string> inputList = new List<string>();
                foreach (object element in input)
                {
                    string fValue = FormatGraphQLValue(element, depth + 1);

                    // exclude empty values (strings that are of length 0.
                    // note that if the parameter supplies an empty string,
                    // it will actually be '\"\"' that is returned by
                    // FormatGraphQLValue, so this still works
                    if (string.IsNullOrEmpty(fValue))
                        continue;

                    inputList.Add(fValue);
                }

                // create an output string. We don't need to handle the case 
                // where inputList length is 0 as string.Join does that for us.
                string output = string.Concat(@"[", string.Join(@",", inputList), @"]");
                return output;
            }

            // for GraphQLParameters we also need to apply recursion. If the
            // rest of the code is adhering to a standard, this should only be
            // called once and the recursion is not needed.
            if (value is GraphQLParameters)
            {
                GraphQLParameters input = (GraphQLParameters)value;

                string output = string.Concat(@"{", input.ToString(), @"}");
                return output;
            }

            // here we format every remaining object type. We recursively 
            // include every property that has the JsonPath attribute defined. 
            PropertyInfo[] properties = JsonPath.GetJsonPathProperties(value);

            if (properties.Length > 0)
            {
                List<string> inputList = new List<string>();
                
                foreach (PropertyInfo property in properties)
                {
                    // this will return the first attribute matching. We don't
                    // need to check for null as the properties list is already
                    // filtered for only properties with the JsonPath attribute.
                    JsonPath attr = JsonPath.GetJsonPathAttribute(property);

                    // try formatting the value. If it is null, we won't add it
                    // to the parameter list.
                    string fValue = FormatGraphQLValue(property.GetValue(value), depth + 1);
                    if (string.IsNullOrEmpty(fValue))
                        continue;

                    string element = string.Concat(attr.BasePath, @":", fValue);
                    inputList.Add(element);
                }

                // if there are no property values set, we consider this an 
                // empty parameter set and we return an empty string.
                if (inputList.Count == 0)
                    return @"";

                string output = string.Concat(@"{", string.Join(@",", inputList), @"}");
                return output;
            }

            // Catch all. We should not reach this.
            throw new NebException("unsupported object type supplied");
        }

        /// <summary>
        /// Convert the GraphQLParameter object to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {           
            List<string> inputList = new List<string>();

            foreach (string key in this.Keys)
            {
                // try formatting the value. If it is null, we won't add it
                // to the parameter list.
                string fValue = FormatGraphQLValue(this[key], 0);
                if (string.IsNullOrEmpty(fValue))
                    continue;

                inputList.Add(string.Concat(key, @":", fValue));
            }

            // if none of the properties are set, this is an empty parameter
            // list and we return an empty string.
            if (inputList.Count == 0)
                return @"";

            string output = string.Join(",", inputList);
            return output;  
        }
    }
}