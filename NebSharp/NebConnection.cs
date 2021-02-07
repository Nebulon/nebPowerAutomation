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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NebSharp
{
    /// <summary>
    /// Nebulon connection
    /// </summary>
    public partial class NebConnection
    {
        /// <summary>
        /// Time in milliseconds to wait for a response from UCAPI.
        /// </summary>
        private const int GRAPHQL_WAITTIME_MS = 6 * 1000;

        /// <summary>
        /// Time in milliseconds to wait for resource creation after token
        /// delivery.
        /// </summary>
        private const int TOKEN_WAITTIME_MS = 3 * 1000;

        /// <summary>
        /// Version string for this binary
        /// </summary>
        private readonly string _assemblyVersion;

        /// <summary>
        /// Reusable HTTP client. After login, this HTTP client will carry the session
        /// cookie until the user explicitly logs out.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Custom HTTP Client handler. This is required to dynamically update
        /// Cookies for other paths in the same domain.
        /// </summary>
        private readonly HttpClientHandler _httpHandler;

        /// <summary>
        /// Platform string where this library is used
        /// </summary>
        private readonly string _platform;

        /// <summary>
        /// Full UCAPI Server URI.
        /// By default this is https://ucapi.nebcloud.nebuloninc.com.
        /// </summary>
        private readonly string _server;

        /// <summary>
        /// Instantiate a new connection to Nebulon ON. This will not automatically log in a user.
        /// </summary>
        public NebConnection()
        {
            _httpHandler = new HttpClientHandler();
            _httpHandler.UseCookies = true;
            _httpClient = new HttpClient(_httpHandler);
            _server = "https://ucapi.nebcloud.nebuloninc.com";

            // setup the default logger
            Logger = new ConsoleLogger(LogSeverity.Error);

            // get information about the runtime. We send this to UCAPI for
            // auditing what client was using the API.
            AssemblyName assembly = typeof(NebConnection).Assembly.GetName();

            _assemblyVersion = string.Concat(
                    assembly.Name,
                    "/",
                    assembly.Version
            );

            _platform = string.Concat(
                    Environment.OSVersion.Platform,
                    "/",
                    Environment.OSVersion.Version
            );
        }

        /// <summary>
        /// Log writer. Overwrite this if you need to write to a custom log, e.g.
        /// to a file or to a message queue. By default this will log to the
        /// Console.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Sends a token (received by Nebulon ON) to one or more SPUs that are
        /// specified in the token. It will return true if at least one of the
        /// SPUs in the token accepted token delivery. It returns false if none
        /// are reachable.
        /// </summary>
        /// <param name="tokenResponse">
        /// A valid nebulon token
        /// </param>
        /// <returns>True - if successful; False - if failed</returns>
        public bool DeliverToken(TokenResponse tokenResponse)
        {
            string deliveryResponse = DeliverTokenString(tokenResponse);

            if (deliveryResponse == string.Empty)
                throw new Exception("Unable to delivery token to any SPU");

            if (deliveryResponse == "OK" || deliveryResponse == "\"OK\"")
                return true;

            throw new Exception("Unable to delivery token to any SPU");
        }

        /// <summary>
        /// Delivers the token to SPUs that support recipe engine v2
        /// </summary>
        /// <param name="tokenResponse">
        /// A valid nebulon token
        /// </param>
        /// <returns></returns>
        ///{
        public RecipeRecordIdentifier DeliverTokenV2(TokenResponse tokenResponse)
        {
            string deliveryResponse = DeliverTokenString(tokenResponse);

            if (deliveryResponse == string.Empty)
                throw new Exception("Unable to delivery token to any SPU");

            // parse for recipe engine v2
            try
            {
                JObject json = JObject.Parse(deliveryResponse);

                RecipeRecordIdentifier identifier = new RecipeRecordIdentifier();
                identifier.RecipeGuid = new Guid(json.Value<string>("recipe_uuid_to_wait_on"));
                identifier.NPodGuid = new Guid(json.Value<string>("npod_uuid_to_wait_on"));

                Logger.WriteDebug("  got recipe identifier");

                return identifier;
            }
            catch (JsonReaderException ex)
            {
                Logger.WriteError(string.Concat(
                    "Could not read JSON data '",
                    deliveryResponse,
                    "': ",
                    ex.Message
                ));

                return null;
            }
        }

        /// <summary>
        /// Converts a JObject to a .NET object as per the provided type name
        /// </summary>
        /// <param name="objectData">JObject data to convert</param>
        /// <param name="type">Target type</param>
        /// <returns>Populated object</returns>
        public object ObjectFromJObject(JObject objectData, Type type)
        {
            if (objectData == null)
                return null;

            // initialize empty result object
            object result = Activator.CreateInstance(type);

            // Each property that we're querying in the GraphQL response needs
            // to have a JsonPath attribute. We will only convert the properties
            // that have the JsonPath property set.
            PropertyInfo[] properties = JsonPath.GetJsonPathProperties(type);

            foreach (PropertyInfo property in properties)
            {
                try
                {
                    // there should only be one of these attributes be specified
                    JsonPath attribute = JsonPath.GetJsonPathAttribute(property);

                    string jsonPath = attribute.Path;

                    // depending on the type, we need to handle the conversion
                    // differently.
                    Type propertyType = property.PropertyType;

                    if (!propertyType.IsArray)
                    {
                        JToken element = objectData.SelectToken(jsonPath, false);

                        if (element == null)
                            continue;

                        // numbers, booleans and strings can easily be handled
                        // by just moving over the value.
                        if (propertyType.IsValueType || propertyType == typeof(string))
                        {
                            property.SetValue(result, element.ToObject(property.PropertyType));
                            continue;
                        }

                        // objects (that we support) will be handled recursively
                        JObject elementData = element.Value<JObject>();
                        property.SetValue(result, ObjectFromJObject(elementData, property.PropertyType));
                        continue;
                    }

                    // for an array, we need to know the child type and
                    // instantiate every single element individually and we
                    // need to add "[*]" to the JSON path to the array is
                    // unfolded (if it is not properly set).
                    if (jsonPath.IndexOf("[*]") == -1)
                        jsonPath = string.Concat(jsonPath, @"[*]");

                    Type elementType = property.PropertyType.GetElementType();
                    List<object> tmp = new List<object>();

                    try
                    {
                        IEnumerable<JToken> elements = objectData.SelectTokens(jsonPath, false);

                        if (elements == null)
                        {
                            property.SetValue(result, Array.CreateInstance(elementType, 0));
                            continue;
                        }

                        foreach (var childElement in elements)
                        {
                            // numbers, booleans and strings can easily
                            // be handled by just moving over the value.
                            if (elementType.IsValueType || elementType.Equals(typeof(string)))
                            {
                                tmp.Add(childElement.ToObject(elementType));
                                continue;
                            }

                            JObject childData = childElement.Value<JObject>();
                            tmp.Add(ObjectFromJObject(childData, elementType));
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.WriteDebug(err.Message);
                    }

                    Array test = Array.CreateInstance(elementType, tmp.Count);
                    for (int i = 0; i < test.Length; i++)
                        test.SetValue(tmp[i], i);

                    property.SetValue(result, test);
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Concat(
                        "Error converting property '",
                        property.Name,
                        "' of type '",
                        property.PropertyType.Name,
                        "': ",
                        ex.Message
                    );

                    Logger.WriteWarning(errorMessage);
                }
            }

            return result;
        }

        /// <summary>
        /// Run a mutation against Nebulon ON. This function should be used when
        /// you expect just a single item returned.
        /// </summary>
        /// <typeparam name="T">The Type of the target array</typeparam>
        /// <param name="name">Name of the mutation</param>
        /// <param name="parameters">Parameters for the mutation</param>
        /// <returns>Single item returned by the mutation</returns>
        public T RunMutation<T>(string name, GraphQLParameters parameters = null) where T : new()
        {
            T[] results = RunMutationMany<T>(name, parameters);

            if (results.Length != 1)
                throw new Exception("Unexpected number of results returned");

            return results[0];
        }

        /// <summary>
        /// Run a mutation against Nebulon ON
        /// </summary>
        /// <typeparam name="T">The Type of the target array</typeparam>
        /// <param name="name">Name of the mutation</param>
        /// <param name="parameters">Parameters for the mutation</param>
        /// <returns>List of items returned by the mutation</returns>
        public T[] RunMutationMany<T>(string name, GraphQLParameters parameters = null) where T : new()
        {
            try
            {
                // field types for objects are all marked with a property attribute
                // for all supported Neb* types. Every field that should be queried for
                // must be provided in the class properties as JsonPath attribute.
                string[] fields = GetQueryFields(typeof(T));

                // this composes a new GraphQL method of type mutation.
                GraphQLMethod method = new GraphQLMethod(
                    GraphQLMethodType.Mutation,
                    name,
                    parameters,
                    fields
                );

                // run the task
                object[] taskResults = Run<T>(method);

                // this is the most tricky part here as the type conversion from a generic
                // type to something else doesn't seem to be very reliable. However,
                // this approach seeps to work fine (according to Stack Overflow).
                return CastObjects<T>(taskResults);
            }
            catch (AggregateException errors)
            {
                string errorMessage = "Error executing mutation";

                foreach (Exception err in errors.InnerExceptions)
                    errorMessage = string.Concat(errorMessage, ", ", err.Message);

                Logger.WriteError(errorMessage);

                throw new Exception(errorMessage);
            }
            catch (TimeoutException err)
            {
                Logger.WriteError(err.Message);
                throw new Exception(err.Message);
            }
            catch (Exception err)
            {
                Logger.WriteError(err.Message);
                throw new Exception(err.Message);
            }
        }

        /// <summary>
        /// Run a query against Nebulon ON. This function should be used when
        /// you expect just a single item returned.
        /// </summary>
        /// <typeparam name="T">The Type of the target array</typeparam>
        /// <param name="name">Name of the query</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>Single item returned by the query</returns>
        public T RunQuery<T>(string name, GraphQLParameters parameters = null) where T : new()
        {
            T[] results = RunQueryMany<T>(name, parameters);

            if (results.Length != 1)
                throw new Exception("Unexpected number of results returned");

            return results[0];
        }

        /// <summary>
        /// Run a query against Nebulon ON
        /// </summary>
        /// <typeparam name="T">The Type of the target array</typeparam>
        /// <param name="name">Name of the query</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>List of items returned by the query</returns>
        public T[] RunQueryMany<T>(string name, GraphQLParameters parameters = null) where T : new()
        {
            try
            {
                // field types for objects are all marked with a property attribute
                // for all supported Neb* types. Every field that should be queried for
                // must be provided in the class properties as JsonPath attribute.
                string[] fields = GetQueryFields(typeof(T));

                // this composes a new GraphQL method of type query.
                GraphQLMethod method = new GraphQLMethod(
                    GraphQLMethodType.Query,
                    name,
                    parameters,
                    fields
                );

                // run the task
                object[] taskResults = Run<T>(method);

                // this is the most tricky part here as the type conversion from a generic
                // type to something else doesn't seem to be very reliable. However,
                // this approach seeps to work fine (according to Stack Overflow).
                return CastObjects<T>(taskResults);
            }
            catch (AggregateException errors)
            {
                string errorMessage = "Error executing query";

                foreach (Exception err in errors.InnerExceptions)
                    errorMessage = string.Concat(errorMessage, ", ", err.Message);

                Logger.WriteError(errorMessage);

                throw new Exception(errorMessage);
            }
            catch (TimeoutException err)
            {
                Logger.WriteError(err.Message);
                throw new Exception(err.Message);
            }
            catch (Exception err)
            {
                Logger.WriteError(err.Message);
                throw new Exception(err.Message);
            }
        }

        /// <summary>
        /// Get all GraphQL fields to query for given a Type. The Type needs to
        /// have JsonPath attributes specified for its properties for them to
        /// be included.
        /// </summary>
        /// <param name="inputType">Type of the class</param>
        /// <param name="convert">Convert the fields to GraphQL</param>
        /// <returns>List of (nested) GrapQL fields</returns>
        private static string[] GetQueryFields(Type inputType, bool convert = true)
        {
            // the type could be an array for which we need to know the child
            // types so we make a copy that we can overwrite.
            Type type = inputType.IsArray && inputType.HasElementType
                ? inputType.GetElementType()
                : inputType;

            // Each property that we're querying in the GraphQL response needs
            // to have a JsonPath attribute. This function will return the
            // properties that have an attribute.
            PropertyInfo[] properties = JsonPath.GetJsonPathProperties(type);

            // The JsonPath attribute is a JSONP string, so we need to translate
            // that into a GraphQL query list.
            List<string> fields = new List<string>();

            // we need to cleanup some of the JSONPath specific things from the string
            // for which we use this regular expression.
            Regex regex = new Regex("[^a-zA-Z.0-9]+");

            foreach (PropertyInfo property in properties)
            {
                JsonPath attribute = JsonPath.GetJsonPathAttribute(property);

                // this removes all characters that we don't want
                string cleanName = regex.Replace(attribute.Path, "");

                // depending on the property type we need to handle this
                // differently.
                Type propertyType = property.PropertyType;
                if (propertyType.IsArray && propertyType.HasElementType)
                    propertyType = propertyType.GetElementType();

                // we need to do this recursively, otherwise we can just work
                // with primitives we don't recurse built-in classes.
                if (IsBasicType(propertyType))
                {
                    fields.Add(cleanName);
                    continue;
                }

                string[] childFields = GetQueryFields(property.PropertyType, false);
                foreach (string childField in childFields)
                    fields.Add(string.Concat(cleanName, childField));
            }

            if (convert)
                return GraphQLFieldsFromJsonp(fields.ToArray());

            return fields.ToArray();
        }

        /// <summary>
        /// Sorts, reduces and aggregates a list of JSONP queries into
        /// GraphQL fieldQuery. This is a recursive function and will convert
        /// <code>["b.c", "a", "b.d"]</code>
        /// into
        /// <code>["a","b{c,d}"]</code>
        /// </summary>
        /// <param name="fields">List of JSONP queries</param>
        /// <returns>List of (nested) GrapQL fields</returns>
        private static string[] GraphQLFieldsFromJsonp(string[] fields)
        {
            Dictionary<string, List<string>> sorted =
                new Dictionary<string, List<string>>();

            // JSONP queries are divided via a dot. We need to split at the dot
            // boundary for deduplication of the queried fields and to aggregate
            // child items.
            char[] splitter = new char[] { '.' };

            foreach (string entry in fields)
            {
                // this split operation assumes that the entry may start
                // with a dot. It will simply remove this empty entry
                string[] parts = entry.Split(splitter, 2,
                    StringSplitOptions.RemoveEmptyEntries);

                // although this should not happen, we may get an exception
                // if an invalid entry was provided. Instead we skip over it.
                if (parts.Length == 0)
                    continue;

                // this is just for code readability
                string key = parts[0];

                // the base entry was not seen previously so we need to initialize
                // a dynamic list of child items that we can sort recursively
                // after we've found all base items. We do this even when there
                // are no children. We check for this length later.
                if (!sorted.ContainsKey(key))
                    sorted.Add(key, new List<string>());

                // if there are children that need resolution, we will add them
                // to the base item. With recursion we will sort them after this
                // loop.
                if (parts.Length > 1)
                    sorted[key].Add(parts[1]);
            }

            // initialize the result list
            List<string> result = new List<string>(sorted.Keys.Count);

            foreach (string key in sorted.Keys)
            {
                // if there is just a base value, don't bother looking through
                // the list of children.
                if (sorted[key].Count == 0)
                {
                    result.Add(key);
                    continue;
                }

                // recursively sort the child items
                string[] children = GraphQLFieldsFromJsonp(sorted[key].ToArray());

                // create the field entry
                result.Add(string.Concat(key, "{", string.Join(",", children), "}"));
            }

            return result.ToArray();
        }

        private static bool IsBasicType(Type input)
        {
            if (input.IsValueType || input.Equals(typeof(string)))
                return true;

            if (input.IsArray && input.HasElementType)
                return IsBasicType(input.GetElementType());

            return false;
        }

        /// <summary>
        /// Extract the response body from an HTTP response. This method
        /// uses a global timeout variable defined in GRAPHQL_WAITTIME_MS
        /// </summary>
        /// <param name="response">Response to read the data from</param>
        /// <exception cref="TimeoutException"/>
        /// <returns>HTTP response body as string</returns>
        private static string ReadResponseBodyString(HttpResponseMessage response)
        {
            // read the contents
            Task<string> task = response.Content.ReadAsStringAsync();

            // wait for the execution to finish. task.Wait will return
            // true if it completed within the specified timeout.
            bool timedOut = !task.Wait(GRAPHQL_WAITTIME_MS);
            if (timedOut)
            {
                string msg = string.Format("Reading body from {0} timed out",
                    response.RequestMessage.RequestUri.AbsoluteUri);
                throw new TimeoutException(msg);
            }

            return task.Result.Trim();
        }

        /// <summary>
        /// Convert an array of generic object types securely into a specified
        /// type. If the cast is not successful, this method will use the
        /// type's default value.
        /// </summary>
        /// <typeparam name="T">The Type of the target array</typeparam>
        /// <param name="input">Array of generic objects to cast</param>
        /// <returns></returns>
        private T[] CastObjects<T>(object[] input)
        {
            T[] result = new T[input.Length];

            for (int i = 0; i < result.Length; i++)
            {
                // if everything was done right by the user that called the
                // method with the right type, then this should hit 100% of
                // the time. However, if the user of this function made a
                // mistake, we need to handle it.
                if (input[i] is T)
                {
                    result[i] = (T)input[i];
                    continue;
                }

                // TODO: I think that this will always fail if the first one
                // didn't succeed. We can just replace this with default(T);

                try
                {
                    result[i] = (T)Convert.ChangeType(input[i], typeof(T));
                }
                catch (InvalidCastException castException)
                {
                    Logger.WriteWarning(castException.Message);
                    result[i] = default(T);
                }
            }

            return result;
        }

        private string DeliverOneToken(string token, string ip)
        {
            try
            {
                string uri = string.Concat("https://", ip);

                Logger.WriteDebug(string.Concat("Sending token to ", uri));
                Logger.WriteDebug(string.Concat(" token:    ", token));

                // prepare http content
                HttpContent content = new StringContent(
                    token,
                    Encoding.UTF8,
                    @"application/json"
                );

                // make an asynchronous request and wait for the response
                Task<HttpResponseMessage> task = _httpClient.PostAsync(uri, content);

                // we don't wait forever for this to complete, but only
                // a determined amount of time
                bool timedOut = !task.Wait(TOKEN_WAITTIME_MS);
                if (timedOut)
                {
                    string msg = string.Format("Request to {0} timed out", uri);
                    throw new TimeoutException(msg);
                }

                // read the contents of the token delivery and return it as
                // regular text so that the calling function can interpret as
                // needed
                string responseText = ReadResponseBodyString(task.Result);

                Logger.WriteDebug(string.Concat(" response:  ", task.Result.StatusCode));
                Logger.WriteDebug(string.Concat(" responseText:  ", responseText));

                return responseText;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Delivers the token to SPUs as a generic method.
        /// </summary>
        /// <param name="tokenResponse"></param>
        /// <returns></returns>
        private string DeliverTokenString(TokenResponse tokenResponse)
        {
            string token = tokenResponse.Token;

            // first, send to MustSendTargetDns - need to send to all of them
            foreach (MustSendTargetDns dns in tokenResponse.MustSendTargetDns)
            {
                // first send the token to the control port
                if (DeliverOneToken(token, dns.ControlPortDns) != string.Empty)
                    continue;

                Logger.WriteDebug(" falling back to data ports");

                // if this failed, send the token to the data ports
                bool deliverySuccess = false;

                foreach (string dp in dns.DataPortDns)
                {
                    if (DeliverOneToken(token, dp) != string.Empty)
                    {
                        deliverySuccess = true;
                        break;
                    }
                }

                if (!deliverySuccess)
                    throw new Exception("Unable to deliver token to mandatory SPUs");
            }

            // second, send the token to the remaining SPUs
            Logger.WriteDebug(" token delivery to mandatory SPUs complete");

            List<string> ips = new List<string>();

            if (tokenResponse.TargetIps != null)
                foreach (string ip in tokenResponse.TargetIps)
                    ips.Add(ip);

            if (tokenResponse.TargetDataIps != null)
                foreach (string ip in tokenResponse.TargetDataIps)
                    ips.Add(ip);

            foreach (string ip in ips)
            {
                string result = DeliverOneToken(token, ip);

                if (result != string.Empty)
                {
                    Logger.WriteDebug(" token delivery completed");
                    return result;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Make a blocking HTTP POST request to the specified URI. This
        /// method uses a global timeout variable defined in GRAPHQL_WAITTIME_MS.
        /// </summary>
        /// <param name="uri">URI to call</param>
        /// <param name="content">Contents of the request</param>
        /// <exception cref="TimeoutException"/>
        /// <returns>HttpResponse from the request</returns>
        private HttpResponseMessage Post(string uri, HttpContent content)
        {
            // update any cookies that are needed for this request
            UpdateCookieForPath(new Uri(uri));

            // make an asynchronous request and wait for the response
            Task<HttpResponseMessage> task = _httpClient.PostAsync(uri, content);

            // we don't wait forever for this to complete, but only
            // a determined amount of time
            bool timedOut = !task.Wait(GRAPHQL_WAITTIME_MS);
            if (timedOut)
            {
                string msg = string.Format("Request to {0} timed out", uri);
                throw new TimeoutException(msg);
            }

            return task.Result;
        }

        /// <summary>
        /// Execute a GraphQL method against nebulon on.
        /// </summary>
        /// <typeparam name="T">Type of the object that we expect as a response</typeparam>
        /// <param name="method">GraphQL method</param>
        /// <returns></returns>
        private object[] Run<T>(GraphQLMethod method) where T : new()
        {
            // this could be made static as this URI should never really change.
            // the server doesn't end in a "/", but I think we should eventually
            // check for a malformed server.
            string uri = string.Concat(this._server, "/query");

            Logger.WriteDebug(string.Format(
                "Sending '{0}' to {1}.",
                method,
                uri
            ));

            // prepare the request body. We need to make a JSON body
            // for UCPAI using JSON object with a single property "query".
            JObject jsonObject = new JObject(new JProperty(@"query", method.ToString()));
            string bodyString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            // we want to keep track of how long this request took, so we keep track
            // of the start time. If we later don't care about it anymore, we can
            // delete this.
            DateTime start = DateTime.UtcNow;

            HttpContent content = new StringContent(
                bodyString,
                Encoding.UTF8,
                @"application/json"
            );

            content.Headers.Add("Nebulon-Client-App", _assemblyVersion);
            content.Headers.Add("Nebulon-Client-Platform", _platform);

            HttpResponseMessage response = Post(uri, content);
            string responseString = ReadResponseBodyString(response);

            // this is the earliest we should stop recording the time / duration of the
            // request as this is mostly now on the client side, not the server.
            double durationMs = (DateTime.UtcNow - start).TotalMilliseconds;
            Logger.WriteDebug(string.Format("Got response '{0}' in {1} ms.",
                response.StatusCode,
                durationMs));
            Logger.WriteDebug(string.Concat("Response:", responseString));

            // the UCAPI server response with JSON formatted data
            // with a well known structure. one property is "errors" and is
            // present when the request failed, and another property "data"
            // which is populated when the request succeeds.
            JObject json = JObject.Parse(responseString);

            // check for errors (any HTTP code outside of 2xx). We probably want to still
            // get the response body and bubble that up in the exception
            int statusCode = (int)response.StatusCode;
            if (200 < statusCode || statusCode >= 300 || json.ContainsKey("errors"))
            {
                string errorMessage = string.Format("Request error (HTTP {0})",
                    statusCode);

                if (json.ContainsKey("errors"))
                {
                    // get the errors that are specified in the response JSON
                    // and add them to the error string.
                    IEnumerable<JToken> errors = json.SelectTokens("errors[*].message", true);

                    foreach (JToken error in errors)
                    {
                        // convert the JToken to a string and append it to the
                        // error message
                        string errorString = (string)error.ToObject(typeof(string));
                        errorMessage = string.Concat(errorMessage, ", ", errorString);
                    }
                }

                throw new Exception(errorMessage);
            }

            // read data. This query could fail when the data is not present in the
            // response. So, we're checking for the value to not be null.
            string jsonPathData = string.Concat("$.data.", method.MethodName);
            JToken data = json.SelectToken(jsonPathData, false);

            if (data == null)
                return null;

            if (data.Type != JTokenType.Array)
            {
                if (data.Type == JTokenType.Object)
                {
                    JObject resultData = data.Value<JObject>();
                    return new object[] { ObjectFromJObject(resultData, typeof(T)) };
                }

                // Convert primitive types directly
                return new object[] { data.Value<T>() };
            }

            JArray dataArray = data.Value<JArray>();
            List<object> resultDataArray = new List<object>();

            foreach (JObject item in dataArray)
                resultDataArray.Add(ObjectFromJObject(item, typeof(T)));

            return resultDataArray.ToArray();
        }

        /// <summary>
        /// This method is needed as the HTTP client will not store cookies for a
        /// domain but per path. This will create a new cookie for the specified
        /// path.
        /// </summary>
        /// <param name="uri">The URI for which to set the cookie</param>
        private void UpdateCookieForPath(Uri uri)
        {
            // cancel if the cookies are there
            CookieCollection existingCookies =
                _httpHandler.CookieContainer.GetCookies(uri);

            if (existingCookies.Count > 0)
                return;

            Uri baseUri = new Uri(string.Concat(_server, "/query"));
            CookieCollection cookies =
                _httpHandler.CookieContainer.GetCookies(baseUri);

            foreach (Cookie cookie in cookies)
            {
                Cookie newCookie = new Cookie();
                newCookie.Name = cookie.Name;
                newCookie.Value = cookie.Value;
                newCookie.Domain = cookie.Domain;
                newCookie.Expires = cookie.Expires;
                newCookie.Path = uri.AbsolutePath;

                _httpHandler.CookieContainer.Add(newCookie);
            }
        }
    }
}