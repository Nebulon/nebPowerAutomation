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
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NebSharp.Types
{
    /// <summary>
    /// Allows specifying the JSONPath expression for a class property for
    /// serialization and deserialization of a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class JsonPath : Attribute
    {
        /// <summary>
        /// Constructs a new JsonPath attribute
        /// </summary>
        /// <param name="path">
        /// JSONPath, e.g. "$.key"
        /// </param>
        /// <param name="mandatory">
        /// Indicates if the value in the specified path must be provided
        /// </param>
        public JsonPath(string path, bool mandatory = false)
        {
            // this needs to be a JSONP string, which start with "$."
            if (!path.StartsWith(@"$."))
                throw new ArgumentException("Invalid JSONP string provided", "path");

            Path = path;
            Mandatory = mandatory;
        }

        /// <summary>
        /// Base path for the variable.
        /// For e.g. <b>$.name.something</b> it will be <b>name</b>.
        /// </summary>
        public string BasePath
        {
            get
            {
                // the JSON path string is split by either a '.' or a '[*]'.
                // Since the base path is only the very first segment, we don't
                // need to worry about any other character.
                string[] splitPath = Path.Split(
                    new char[] { '.', '[' },
                    StringSplitOptions.RemoveEmptyEntries
                );

                if (splitPath.Length == 1)
                    throw new ArgumentException(@"Base path can't be determined");

                return splitPath[1];
            }
        }

        /// <summary>
        /// The JSON Path property as a GraphQL hierarchy
        /// </summary>
        public string GraphQLName
        {
            get
            {
                // remove all unsupported characters first.
                Regex unsupportedChars = new Regex("[^a-zA-Z.0-9]+");
                string cleanPath = unsupportedChars.Replace(Path, "");

                // the graphQL name is what would be provided by a query, including
                // all nested properties. At every '.' a new nesting level starts.
                string[] splitPath = cleanPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                // convert the segments to a GraphQL hierarchy. We start from the
                // inner-most element and wrap it as we get lower in the
                // hierarchy.
                string result = "";

                for (int i = splitPath.Length - 1; i >= 0; i--)
                {
                    if (result.Length == 0)
                    {
                        result = splitPath[i];
                        continue;
                    }

                    result = string.Concat(splitPath[i], "{", result, "}");
                }

                return result;
            }
        }

        /// <summary>
        /// Indicates if the parameter must be set
        /// </summary>
        public bool Mandatory { get; }

        /// <summary>
        /// JSON path for the variable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Get the JsonPath attribute from a property. If there are multiple
        /// attributes specified, only the first attribute is returned. If
        /// no attribute is found, then null is returned.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static JsonPath GetJsonPathAttribute(PropertyInfo propertyInfo)
        {
            JsonPath[] attributes = (JsonPath[])propertyInfo.GetCustomAttributes(typeof(JsonPath), false);

            if (attributes.Length == 0)
                return null;

            return attributes[0];
        }

        /// <summary>
        /// Gets the list of class properties that have a JsonPath attribute.
        /// </summary>
        /// <param name="value">
        /// The object from which to retrieve the properties
        /// </param>
        /// <returns></returns>
        public static PropertyInfo[] GetJsonPathProperties(object value)
        {
            // we need to format differently based on its type and we will look at
            // the most common cases first so we don't need to make too many
            // comparisons
            Type valueType = value.GetType();
            return GetJsonPathProperties(valueType);
        }

        /// <summary>
        /// Gets the list of class properties that have a JsonPath attribute.
        /// </summary>
        /// <param name="valueType">
        /// The type from which to retrieve the properties
        /// </param>
        /// <returns></returns>
        public static PropertyInfo[] GetJsonPathProperties(Type valueType)
        {
            List<PropertyInfo> propertyList = new List<PropertyInfo>();

            foreach (PropertyInfo property in valueType.GetProperties())
            {
                JsonPath[] attributes = (JsonPath[])property.GetCustomAttributes(typeof(JsonPath), false);

                if (attributes.Length == 0)
                    continue;

                propertyList.Add(property);
            }

            return propertyList.ToArray();
        }
    }
}