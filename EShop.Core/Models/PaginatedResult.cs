namespace EShop.Core.Models
{
    public abstract class PaginatedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min((CurrentPage + 1) * PageSize, RowCount); }
        }

        public string Search { get; set; }

        public string SortLabel { get; set; }

        public int SortDirection { get; set; }

        public bool ShowPrevious => CurrentPage + 1 > 1;
        public bool ShowNext => CurrentPage + 1 < (PageCount);
        public bool ShowFirst => CurrentPage + 1 != 1;
        public bool ShowLast => CurrentPage != (PageCount - 1);
    }

    public class PaginatedResult<T> : PaginatedResultBase
    {
        public IList<T> Data { get; set; }

        public PaginatedResult()
        {
            Data = new List<T>();
        }

        public PaginatedResult(List<T> data, int currentPage, int pageSize, int totalRecords)
        {
            Data = data;
            CurrentPage = currentPage;
            PageSize = pageSize;
            RowCount = totalRecords;

            var totalPages = ((double)totalRecords / (double)pageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            PageCount = roundedTotalPages;

            if (CurrentPage > roundedTotalPages)
                CurrentPage = 1;
        }
    }
}
