namespace HotelAPI.Common.Helper
{
	public static class StoredProcedure
	{
		// Country Stored Procedures
		public const string GetCountryList = "Country_GetAll";
		public const string GetContentByCountry = "Content_GetByCountry";
		public const string GetRegionsByCountry = "Regions_GetByCountry";
	}
}