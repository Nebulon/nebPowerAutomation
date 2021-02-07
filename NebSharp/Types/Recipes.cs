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
    /// Status of the recipe execution
    /// </summary>
    public enum RecipeState
    {
        /// <summary>
        /// Recipe is queued for execution
        /// </summary>
        Queued,

        /// <summary>
        /// Recipe is running
        /// </summary>
        Running,

        /// <summary>
        /// Recipe execution completed successfully
        /// </summary>
        Completed,

        /// <summary>
        /// Recipe execution completed with a failure
        /// </summary>
        Failed,

        /// <summary>
        /// Recipe execution timed out
        /// </summary>
        Timeout,

        /// <summary>
        /// Recipe execution is being canceled
        /// </summary>
        Cancelling,

        /// <summary>
        /// Recipe execution completed by a cancelation
        /// </summary>
        Cancelled,
    }

    /// <summary>
    /// Type of recipe
    /// </summary>
    public enum RecipeType
    {
        /// <summary>
        /// Recipe type is unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Claim SPU
        /// </summary>
        Claim,

        /// <summary>
        /// Create volume
        /// </summary>
        CreateVolume,

        /// <summary>
        /// Create nPod
        /// </summary>
        CreatePod,

        /// <summary>
        /// Validate nPod
        /// </summary>
        ValidatePod,

        /// <summary>
        /// Confirm nPod
        /// </summary>
        ConfirmPod,

        /// <summary>
        /// Create snapshot
        /// </summary>
        CreateSnapshot,

        /// <summary>
        /// Create snapshot via a schedule
        /// </summary>
        CreateScheduledSnapshot,

        /// <summary>
        /// Update SPU or nPod
        /// </summary>
        Update,

        /// <summary>
        /// Abort SPU or nPod update
        /// </summary>
        AbortUpdate,

        /// <summary>
        /// Remove snapshot schedule
        /// </summary>
        RemoveSnapshotSchedule,

        /// <summary>
        /// Send SPU debug information to nebulon ON
        /// </summary>
        SendSPUDebugInfo,

        /// <summary>
        /// Run test
        /// </summary>
        RunTest,

        /// <summary>
        /// Wipe nPod
        /// </summary>
        WipePod,

        /// <summary>
        /// Delete volume
        /// </summary>
        DeleteVolume,

        /// <summary>
        /// Set vCenter credentials
        /// </summary>
        SetVSphereCredentials,

        /// <summary>
        /// Reset organization
        /// </summary>
        ResetOrganization,

        /// <summary>
        /// Locate / ping SPU
        /// </summary>
        PingSPU,

        /// <summary>
        /// Create LUN
        /// </summary>
        CreateLUN,

        /// <summary>
        /// Delete LUN
        /// </summary>
        DeleteLUN,

        /// <summary>
        /// Set Proxy Server information
        /// </summary>
        SetProxy,

        /// <summary>
        /// Set network time protocol server
        /// </summary>
        SetNTP,

        /// <summary>
        /// Update physical drive
        /// </summary>
        UpdatePhysicalDrive,

        /// <summary>
        /// Set timezone
        /// </summary>
        SetTimezone,

        /// <summary>
        /// Clone volume
        /// </summary>
        CloneVolume,

        /// <summary>
        /// Locate physical drive
        /// </summary>
        LocatePhysicalDrive,

        /// <summary>
        /// Replace SPU
        /// </summary>
        ReplaceSPU,

        /// <summary>
        /// Secure erase SPU
        /// </summary>
        SecureEraseSPU,
    }

    /// <summary>
    /// A filter object to filter recipes.
    ///
    /// <para>
    /// Allows filtering active and completed recipes. Recipes are the result
    /// of mutations of mutations or modifications of on-premises
    /// infrastructure. As commands may require some time to complete, the
    /// recipe filter allows the query for their status.
    /// </para>
    /// </summary>
    public sealed class NPodRecipeFilter
    {
        /// <summary>
        /// Filter based on completion status
        /// </summary>
        [JsonPath("$.completed", false)]
        public bool Completed { get; set; }

        /// <summary>
        /// Filter based on nPod unique identifier
        /// </summary>
        [JsonPath("$.nPodUUID", false)]
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// Filter based on nPod recipe unique identifier
        /// </summary>
        [JsonPath("$.recipeUUID", false)]
        public Guid RecipeGuid { get; set; }
    }

    /// <summary>
    /// A recipe record
    ///
    /// <para>
    /// A recipe record describes the status of the execution of a mutation that
    /// affects on-premises infrastructure. Status information of such mutations
    /// are recorded in recipe records.
    /// </para>
    /// </summary>
    public sealed class RecipeRecord
    {
        /// <summary>
        /// A unique identifier that can be used to cancel recipe execution
        /// </summary>
        [JsonPath("$.cancelRecipeUUID", true)]
        public Guid CancelGuid { get; set; }

        /// <summary>
        /// Serial number of the SPU that coordinates the execution of the
        /// recipe within a nPod
        /// </summary>
        [JsonPath("$.coordinatorSPUSerial", true)]
        public string CoordinatorSpuSerial { get; set; }

        /// <summary>
        /// The unique identifier for the nPod that this recipe is associated
        /// with
        /// </summary>
        [JsonPath("$.nPodUUID", true)]
        public Guid CreatedAfter { get; set; }

        /// <summary>
        /// The unique identifier of the recipe record
        /// </summary>
        [JsonPath("$.recipeUUID", true)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Date and time when the recipe was last updated
        /// </summary>
        [JsonPath("$.lastUpdate", true)]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Date and time when the recipe started execution
        /// </summary>
        [JsonPath("$.start", true)]
        public DateTime Start { get; set; }

        /// <summary>
        /// Current state of recipe execution
        /// </summary>
        [JsonPath("$.state", true)]
        public RecipeState State { get; set; }

        /// <summary>
        /// Textual description of the recipe execution status
        /// </summary>
        [JsonPath("$.status", true)]
        public string Status { get; set; }

        /// <summary>
        /// The type of recipe that is executed
        /// </summary>
        [JsonPath("$.type", true)]
        public RecipeType Type { get; set; }
    }

    /// <summary>
    /// A reference to a recipe
    ///
    /// <para>
    /// This object is created and returned to a caller for recipe engine
    /// version 2 calls to allow the caller to query for the status of
    /// recipe execution
    /// </para>
    /// </summary>
    public sealed class RecipeRecordIdentifier
    {
        /// <summary>
        /// The unique identifier of the nPod the recipe is executed for
        /// </summary>
        public Guid NPodGuid { get; set; }

        /// <summary>
        /// The unique identifier of the recipe generated
        /// </summary>
        public Guid RecipeGuid { get; set; }
    }

    /// <summary>
    /// Paginated nPod recipe list object
    ///
    /// Contains a list of nPod recipes objects. At this point all recipes
    /// are returned in a single page.
    /// </summary>
    public sealed class RecipeRecordList
    {
        /// <summary>
        /// A cursor that can be used (in the future) for pagination
        /// </summary>
        [JsonPath("$.cursor", false)]
        public string cursor { get; set; }

        /// <summary>
        /// List of nPod recipies
        /// </summary>
        [JsonPath("$.items", true)]
        public RecipeRecord[] Items { get; set; }
    }
}