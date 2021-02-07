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
    /// Defines available date and time format options
    /// </summary>
    public enum DateFormat
    {
        /// <summary>
        /// Mon Jan _2 15:04:05 2006
        /// </summary>
        ANSIC,

        /// <summary>
        /// Mon Jan _2 15:04:05 MST 2006
        /// </summary>
        UNIX_DATE,

        /// <summary>
        /// Mon Jan 02 15:04:05 -0700 2006
        /// </summary>
        RUBY_DATE,

        /// <summary>
        /// 02 Jan 06 15:04 MST
        /// </summary>
        RFC822,

        /// <summary>
        /// 02 Jan 06 15:04 -0700
        /// </summary>
        RFC822_Z,

        /// <summary>
        /// Monday, 02-Jan-06 15:04:05 MST
        /// </summary>
        RFC850,

        /// <summary>
        /// Mon, 02 Jan 2006 15:04:05 MST
        /// </summary>
        RFC1123,

        /// <summary>
        /// Mon, 02 Jan 2006 15:04:05 -0700
        /// </summary>
        RFC1123_Z,

        /// <summary>
        /// 2006-01-02T15:04:05Z07:00
        /// </summary>
        RFC3339,

        /// <summary>
        /// 2006-01-02T15:04:05.999999999Z07:00
        /// </summary>
        RFC3339_NANO,

        /// <summary>
        /// 3:04PM
        /// </summary>
        KITCHEN,

        /// <summary>
        /// Jan _2 15:04:05
        /// </summary>
        STAMP,

        /// <summary>
        /// Jan _2 15:04:05.000
        /// </summary>
        STAMP_MILLI,

        /// <summary>
        /// Jan _2 15:04:05.000000
        /// </summary>
        STAMP_MICRO,

        /// <summary>
        /// Jan _2 15:04:05.000000000
        /// </summary>
        STAMP_NANO,
    }

    /// <summary>
    /// Enumeration of available resource types in nebulon ON
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// The resource type is not known
        /// </summary>
        Unknown,

        /// <summary>
        /// A datacenter location information resource
        /// </summary>
        Datacenter,

        /// <summary>
        /// A server or host resource
        /// </summary>
        Host,

        /// <summary>
        /// A physical drive or physical disk resource
        /// </summary>
        Disk,

        /// <summary>
        /// A nPod resource
        /// </summary>
        Pod,

        /// <summary>
        /// A group of nPods
        /// </summary>
        PodGroup,

        /// <summary>
        /// A room or lab in a datacenter
        /// </summary>
        Lab,

        /// <summary>
        /// A rack in a datacenter row
        /// </summary>
        Rack,

        /// <summary>
        /// A row in a datacenter
        /// </summary>
        Row,

        /// <summary>
        /// A point-in-time checkpoint of a storage volume
        /// </summary>
        Snapshot,

        /// <summary>
        /// A services processing unit
        /// </summary>
        SPU,

        /// <summary>
        /// A virtual machine
        /// </summary>
        VM,

        /// <summary>
        /// A storage volume
        /// </summary>
        Volume,

        /// <summary>
        /// A network interface
        /// </summary>
        NetworkInterface,
    }

    /// <summary>
    /// Defines sorting direction
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sort items in ascending order
        /// </summary>
        Ascending,

        /// <summary>
        /// Sort items in descending order
        /// </summary>
        Descending,
    }

    /// <summary>
    /// Defines input properties for pagination
    ///
    /// Allows specifying which page to return from the server for API calls that
    /// support pagination. It allows to specify the page number and the quantity
    /// of items to return in the page. Default values for a page are page number
    /// <c>1</c>, and <c>100</c> items per page.
    /// </summary>
    public sealed class PageInput
    {
        private long _count = 100;

        private long _page = 1;

        /// <summary>
        /// Constructs a new PageInput object.
        /// </summary>
        /// <param name="page">
        /// The page number. Defaults to 1
        /// </param>
        /// <param name="count">
        /// The maximum number of items to include in a page. Defaults to 100
        /// </param>
        /// <exception cref="ArgumentException">
        /// Raised when provided values are out of range.
        /// </exception>
        public PageInput(long page = 1, long count = 100)
        {
            Page = page;
            Count = count;
        }

        /// <summary>
        /// The default first page
        /// </summary>
        public static PageInput First
        {
            get
            {
                return new PageInput();
            }
        }

        /// <summary>
        /// Specifies the number of items for each page. Defaults to <b>100</b>.
        /// </summary>
        [JsonPath("$.count", false)]
        public long Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("count must be larger than 0");

                _count = value;
            }
        }

        /// <summary>
        /// Specifies the page to return. Pages start at index <b>1</b>.
        /// </summary>
        [JsonPath("$.page", true)]
        public long Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("page must be larger than 0");

                _page = value;
            }
        }
    }

    /// <summary>
    /// Base object for a paginated server response
    ///
    /// Contains a list of objects and information for pagination.
    /// By default a single page includes a maximum of `100` items
    /// unless specified otherwise in the paginated query.
    ///
    /// Consumers should always check for the property ``more`` as per default
    /// the server does not return the full list of alerts but only one page.
    /// </summary>
    public class PageList<T>
    {
        /// <summary>
        /// The number of items on the server matching the provided filter
        /// </summary>
        [JsonPath("$.filteredCount", true)]
        public long FilteredCount { get; set; }

        /// <summary>
        /// List of items in the pagination list
        /// </summary>
        [JsonPath("$.items", true)]
        public T[] Items { get; set; }

        /// <summary>
        /// Indicates if there are more items on the server
        /// </summary>
        [JsonPath("$.more", true)]
        public bool More { get; set; }

        /// <summary>
        /// The total number of items on the server
        /// </summary>
        [JsonPath("$.totalCount", true)]
        public long TotalCount { get; set; }
    }
}