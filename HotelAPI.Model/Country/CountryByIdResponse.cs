namespace HotelAPI.Model.Country
{
	public class CountryByUrlResponse
	{
		public long CountryID { get; set; }
		public string? CountryContent { get; set; }
		public List<CountryDataResponse> CountryData { get; set; } = new();
		public List<HotelDataResponse> HotelData { get; set; } = new();
	}
	public class CountryDataResponse
	{
		public long Id { get; set; }
		public string? ItemName { get; set; }
		public string? UrlName { get; set; }
		public string? Type { get; set; }
	}
	public class HotelDataResponse
	{
		public long Id { get; set; }
		public string? ItemName { get; set; }
		public string? UrlName { get; set; }
		public int HotelCount { get; set; }
		public string? Type { get; set; }
	}
}
