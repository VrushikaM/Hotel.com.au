namespace HotelAPI.Model.Country
{
	public class CountryByIdResponse
	{
		public long CountryID { get; set; }
	}
	public class ContentByCountryResponse : CountryByIdResponse
	{
		public string? Content { get; set; }
	}
	public class RegionListByCountryResponse
	{
		public long RegionID { get; set; }
		public string? RegionName { get; set; }
	}
}
