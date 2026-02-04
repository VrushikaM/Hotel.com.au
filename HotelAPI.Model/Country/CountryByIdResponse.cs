namespace HotelAPI.Model.Country
{
	public class CountryByUrlNameResponse
	{
		public long CountryID { get; set; }
		public string? CountryContent { get; set; }
		public List<CountryDataResponse> CountryData { get; set; } = new();
	}
	public class CountryDataResponse
	{
		public long Id { get; set; }
		public string? ItemName { get; set; }
		public string? UrlName { get; set; }
		public string? Type { get; set; }
	}
}
