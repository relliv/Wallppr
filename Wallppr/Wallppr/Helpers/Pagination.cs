using Wallppr.Models.Common;
using Wallppr.Models.Common.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Wallppr.Helpers
{
    // source : https://jasonwatmore.com/post/2018/10/17/c-pure-pagination-logic-in-c-aspnet
    public class Pagination
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public ObservableCollection<Page> Pages { get; set; }

        /// <summary>
        /// Pagination creator
        /// </summary>
        /// <param name="totalItems">Total item per page</param>
        /// <param name="currentPage">Current page number</param>
        /// <param name="pageLimit">Total item count</param>
        /// <param name="maxPages">Page count for listing</param>
        public Pagination(int totalItems, int currentPage = 1, int pageLimit = 10, int maxPages = 10)
        {
            Pages = new ObservableCollection<Page>();

            if (currentPage >= (maxPages / 2))
            {
                Pages.Add(new Page
                {
                    PageNumber = 1,
                    PageType = PageType.Start
                });
            }

            Pages.Add(new Page
            {
                PageNumber = currentPage - 1 >= 1 ? currentPage - 1 : 1,
                PageType = PageType.Previous
            });

            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageLimit);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageLimit;
            var endIndex = Math.Min(startIndex + pageLimit - 1, totalItems - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage);

            foreach (var page in pages)
            {
                if (page == currentPage)
                {
                    Pages.Add(new Page
                    {
                        PageNumber = page,
                        PageType = PageType.Current
                    });
                }
                else
                {
                    Pages.Add(new Page
                    {
                        PageNumber = page,
                        PageType = PageType.Normal
                    });
                }
            }

            Pages.Add(new Page
            {
                PageNumber = currentPage + 1 <= endPage ? currentPage + 1 : endPage,
                PageType = PageType.Next
            });

            if (currentPage <= (maxPages / 2) + 2 && currentPage != endPage)
            {
                Pages.Add(new Page
                {
                    PageNumber = totalPages,
                    PageType = PageType.End
                });
            }

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageLimit;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}