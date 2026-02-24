namespace HotelAPI.Common.Helper
{
	public static class StoredProcedure
	{
		// Country Stored Procedures
		public const string GetCountryList = "Country_GetAll";
		public const string GetCountryByUrl = "Country_GetByUrl";

		// Search Stored Procedures
		public const string GlobalSearch = "Search_Global";

		// Auth Stored Procedures
		public const string LoginAuthentication = "Auth_Login";

		// User Stored Procedures
		public const string CreateUser = "User_Register";

		// Collection Stored Procedures
		public const string GetCollectionList = "Collection_GetList";
		public const string UpsertCollection = "Collection_Save";
		public const string SaveCollectionContent = "CollectionContent_Save";
		public const string GetCollectionContent = "CollectionContent_Get";
		public const string GetCollectionContentHistory = "CollectionContent_GetHistory";
		public const string UpsertCollectionRules = "CollectionRules_AddOrUpdate";
		public const string GetCollectionRules = "CollectionRules_GetById";
		public const string ChangeCollectionStatus = "Collection_ChangeStatus";
		public const string UpsertCollectionCuration = "CollectionCuration_Save";

		// Master Dropdown Stored Procedure
		public const string MasterDropdowns = "MasterDropdown_Get";

		// City Stored Procedure
		public const string GetCitiesByCountryOrRegion = "City_GetByCountryOrRegion";

		// Hotel Stored Procedure
		public const string GetHotelsByCity = "Hotel_GetByCity";

		// Region Stored Procedure
		public const string GetRegionsByCountry = "Region_GetByCountry";
	}
}
