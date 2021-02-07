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

namespace NebSharp
{
    public partial class NebConnection
    {
        /// <summary>
        /// Retrieves a list of recipes
        ///
        /// <para>
        /// Recipes are the result of mutations of mutations or modifications of
        /// on-premises infrastructure. As commands may require some time to
        /// complete, the recipe filter allows the query for their status.
        /// </para>
        /// </summary>
        /// <param name="filter">
        /// A filter object to filter the nPod recipes on the server. If
        /// omitted, the server will return all objects as a paginated response.
        /// </param>
        /// <returns>
        /// A paginated list of nPod recipes
        /// </returns>
        public RecipeRecordList GetNPodRecipes(NPodRecipeFilter filter)
        {
            // setup parameters
            GraphQLParameters parameters = new GraphQLParameters();
            parameters.Add("filter", filter, false);

            return RunQuery<RecipeRecordList>(@"getNPodRecipes", parameters);
        }
    }
}