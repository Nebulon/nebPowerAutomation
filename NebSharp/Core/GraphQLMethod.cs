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

namespace NebSharp
{
    internal class GraphQLMethod
    {
        public GraphQLMethod(GraphQLMethodType methodType, string methodName, GraphQLParameters parameters = null, string[] fields = null)
        {
            MethodType = methodType;
            MethodName = methodName;
            Parameters = parameters;
            Fields = fields;
        }

        public string[] Fields { get; set; }
        public string MethodName { get; set; }
        public GraphQLMethodType MethodType { get; set; }
        public GraphQLParameters Parameters { get; set; }

        public override string ToString()
        {
            string methodType;

            switch (MethodType)
            {
                case GraphQLMethodType.Query:
                    methodType = @"query";
                    break;

                case GraphQLMethodType.Mutation:
                    methodType = @"mutation";
                    break;

                case GraphQLMethodType.Subscription:
                    methodType = @"subscription";
                    break;

                default:
                    throw new ArgumentNullException("MethodType");
            }

            if (string.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException("MethodName");

            string parameterString = "";
            if (Parameters != null)
                parameterString = this.Parameters.ToString();

            string fieldsString = "";
            if (Fields != null && Fields.Length > 0)
                fieldsString = string.Join(",", Fields);

            // no parameters and no field queries specified, e.g. result:
            // query{userCount}
            if (parameterString.Length == 0 && fieldsString.Length == 0)
                return string.Concat(
                    methodType,
                    @"{",
                    MethodName,
                    @"}"
                );

            // no parameters, only field queries specified, e.g. result:
            // query{loginState{org}}
            if (parameterString.Length == 0 && fieldsString.Length > 0)
                return string.Concat(
                    methodType,
                    @"{",
                    MethodName,
                    @"{",
                    fieldsString,
                    @"}",
                    @"}"
                );

            // only parameters, but no field queries specified, e.g. result:
            // mutation{removeVV(uuid:"12345")}
            if (parameterString.Length > 0 && fieldsString.Length == 0)
                return string.Concat(
                    methodType,
                    @"{",
                    MethodName,
                    @"(",
                    parameterString,
                    @")",
                    @"}"
                );

            // both parameters and field queries are specified, e.g. result:
            // query{SPUs(serial:"123456"){serial,version}}
            return string.Concat(
                methodType,
                @"{",
                MethodName,
                @"(",
                parameterString,
                @")",
                @"{",
                fieldsString,
                @"}",
                @"}"
            );
        }
    }
}