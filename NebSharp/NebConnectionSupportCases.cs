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

using NebSharp.Types;
using System;

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Allows creation of a new support case
        /// </summary>
        /// <param name="supportCase">
        /// A support case input object
        /// </param>
        /// <returns>The created support case</returns>
        public SupportCase CreateSupportCase(CreateSupportCaseInput supportCase)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", supportCase, true);

            return RunMutation<SupportCase>(@"createSupportCase", parameters);
        }

        /// <summary>
        /// Retrieves a list of support cases
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional
        /// argument and if omitted the server will default to returning the
        /// first page with a maximum of `100` items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter support cases on the
        /// server.If omitted, the server will return all objects as a
        /// paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort support case objects
        /// on supported properties.If omitted objects are returned in the
        /// order as they were created in.
        /// </param>
        /// <returns>A paginated list of support cases</returns>
        public SupportCaseList GetSupportCases(
            PageInput page = null,
            SupportCaseFilter filter = null,
            SupportCaseSort sort = null)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<SupportCaseList>(@"getSupportCases", parameters);
        }

        /// <summary>
        /// Allows updating an existing support case
        /// </summary>
        /// <param name="number">
        /// The case number of the support case to update
        /// </param>
        /// <param name="supportCase">
        /// An input object that describes all fields to update
        /// </param>
        /// <returns>The updated support case</returns>
        public SupportCase UpdateSupportCase(string number, CreateSupportCaseInput supportCase)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("caseNumber", number, true);
            parameters.Add("input", supportCase, true);

            return RunMutation<SupportCase>(@"updateSupportCase", parameters);
        }

        /// <summary>
        /// Allows uploading and attaching files to a support case
        /// </summary>
        /// <param name="number">
        /// The case number of the support case to update
        /// </param>
        /// <param name="filePath">
        /// The absolute path to the file to upload
        /// </param>
        /// <returns>The updated support case</returns>
        public SupportCase UploadSupportCaseAttachment(string number, string filePath)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("caseNumber", number, true);
            parameters.Add("attachment", filePath, true);

            throw new NotImplementedException("Currently not implemented");

            // return RunMutation<SupportCase>(@"uploadSupportCaseAttachment", parameters);
        }
    }
}