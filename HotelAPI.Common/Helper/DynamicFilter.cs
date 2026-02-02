namespace HotelAPI.Common.Helper
{
    public class DynamicFilter
    {
        public string? SearchKeyword { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public Dictionary<string, object>? Filters { get; set; }
    }
}
