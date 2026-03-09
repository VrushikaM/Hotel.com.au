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
		public const string GetCollectionList = "Collection_GetListByGeoNode";
		public const string GetCollectionById = "Collection_Edit";
		public const string UpsertCollection = "Collection_Save";
		public const string UpsertCollectionContent = "CollectionContent_Save";
		public const string UpsertCollectionRules = "CollectionRules_AddOrUpdate";
		public const string ChangeCollectionStatus = "Collection_ChangeStatus";
		public const string UpsertCollectionCurations = "CollectionCuration_Save";
		public const string CloneCollection = "Collection_Clone";
		public const string CollectionDelete = "Collection_Delete";
		public const string CollectionPreviewHotels = "Collection_PreviewHotels";

		// Master Dropdown Stored Procedure
		public const string MasterDropdowns = "MasterDropdown_Get";

		// City Stored Procedure
		public const string GetCitiesByCountryOrRegion = "City_GetByCountryOrRegion";

		// Hotel Stored Procedure
		public const string GetHotelsByGeoNode = "Hotel_GetByGeoNode";

		// Region Stored Procedure
		public const string GetRegionsByCountry = "Region_GetByCountry";

		// District Stored Procedure
		public const string GetDistrictsByCity = "District_GetByCity";
	
		// Slug Stored Procedure
		public const string GeoNodeResolveSlug = "GeoNode_ResolveSlug";
	}
}
