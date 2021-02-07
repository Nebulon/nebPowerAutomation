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
        /// Create a new nPod template
        /// </summary>
        /// <param name="input">
        /// An input object that describes the new nPod template
        /// </param>
        /// <returns>The created nPod template</returns>
        public NPodTemplate CreateNPodTemplate(CreateNPodTemplateInput input)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, true);

            return RunMutation<NPodTemplate>(@"createNPodTemplate", parameters);
        }

        /// <summary>
        /// Delete an existing nPod template tree
        ///
        /// <para>
        /// This deletes an nPod template and all associated versions will
        /// become unavailable for nPod provisioning if the <c>ParentGuid</c>
        /// is supplied, otherwise the specific version is deleted.
        /// </para>
        /// </summary>
        /// <param name="nPodTemplateGuid">
        /// The unique identifier of the nPod template tree. The <c>ParentGuid</c>
        /// property of the nPod template should be used for deletion.
        /// </param>
        /// <returns></returns>
        public bool DeleteNPodTemplate(Guid nPodTemplateGuid)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("parentUUID", nPodTemplateGuid, true);

            // run mutation
            return RunMutation<bool>(@"deleteNPodTemplate", parameters);
        }

        /// <summary>
        /// Retrieve a list of provisioned nPod templates
        /// </summary>
        /// <param name="page">
        /// The requested page from the server. This is an optional argument
        /// and if omitted the server will default to returning the first page
        /// with a maximum of <c>100</c> items.
        /// </param>
        /// <param name="filter">
        /// A filter object to filter the nPod templates on the server. If
        /// omitted, the server will return all objects as a paginated response.
        /// </param>
        /// <param name="sort">
        /// A sort definition object to sort the nPod template objects on
        /// supported properties. If omitted objects are returned in the order
        /// as they were created in.
        /// </param>
        /// <returns>A paginated list of nPod templates</returns>
        public NPodTemplateList GetNPodTemplates(
            PageInput page,
            NPodTemplateFilter filter,
            NPodTemplateSort sort)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("page", page, true);
            parameters.Add("filter", filter, true);
            parameters.Add("sort", sort, true);

            return RunQuery<NPodTemplateList>(@"getNPodTemplates", parameters);
        }

        /// <summary>
        /// Update an existing nPod template.
        /// <para>
        /// Every change to a nPod template will create a new version of the
        /// template and generate a new unique identifier (Guid). The parent /
        /// original nPod template is accessible via the nPod template
        /// <c>ParentGuid</c> property.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// An input object that describes the changes to the nPod template
        /// </param>
        /// <returns>The created nPod template</returns>
        public NPodTemplate UpdateNPodTemplate(UpdateNPodTemplateInput input)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("input", input, true);

            return RunMutation<NPodTemplate>(@"updateNPodTemplate", parameters);
        }
    }
}