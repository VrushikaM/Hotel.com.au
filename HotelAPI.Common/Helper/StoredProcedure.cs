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

		// User Stored Procedures
		public const string CreateUser = "Sp_RegisterUser";

		// Collection Stored Procedures
		public const string GetCollectionList = "Collection_GetList_";
		public const string UpsertCollection = "Save_Collection";

		// Master Dropdown Stored Procedure
		public const string MasterDropdowns = "Sp_MasterDropdowns";

		// City Stored Procedure
		public const string GetCitiesByUrlRegistry = "Sp_GetCityByUrlRegistry";
	}
}
