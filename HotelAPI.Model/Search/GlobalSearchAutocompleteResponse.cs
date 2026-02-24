namespace HotelAPI.Model.Search
{
	public class GlobalSearchResponse
	{
		public int Id { get; set; }
		public string DisplayText { get; set; } = string.Empty;
		public string? UrlName { get; set; }
		public string Type { get; set; } = string.Empty;
	}
}
