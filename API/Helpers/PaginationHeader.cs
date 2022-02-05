namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int totalPages, int itemsPerPage, int totalitems)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalitems;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }
}