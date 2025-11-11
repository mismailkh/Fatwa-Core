namespace FATWA_DOMAIN.Models.BaseModels
{
    public class GridPagination
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; } = 1;
        public bool isDataSorted { get; set; }
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
    }
    public class GridMetadata
    {
        public int TotalCount { get; set; }
    }
}
