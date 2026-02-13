namespace HotelAPI.Common.Helper
{
	public static class StoredProcedure
	{
		// Country Stored Procedures
		public const string GetCountryList = "Country_GetAll";
		public const string GetCountryByUrl = "Country_GetByUrl";

		// Search Stored Procedures
		public const string GlobalSearch = "dbo.Global_Search";

		// Auth Stored Procedures
		public const string LoginAuthentication = "Sp_LoginAuthentication";
	}
}