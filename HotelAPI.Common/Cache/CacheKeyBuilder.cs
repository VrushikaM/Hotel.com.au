namespace HotelAPI.Common.Cache
{
	public class CacheKeyBuilder
	{
		public static string CountryList(string? searchTerm)
			=> $"country:list:{(string.IsNullOrWhiteSpace(searchTerm) ? "all" : searchTerm.Trim().ToLowerInvariant())}";

		public static string CountryByUrl(string normalizedUrl, string? normalizedAlphabet)
			=> $"country:url:{normalizedUrl}:{normalizedAlphabet ?? "all"}";

		public static string CitiesByCountryOrRegion(int countryId, int? regionId, string? searchTerm)
			=> $"city:list:{countryId}:{regionId ?? 0}:{(string.IsNullOrWhiteSpace(searchTerm) ? "all" : searchTerm.Trim().ToLowerInvariant())}";

		public static string RegionsByCountry(int countryId, string? searchTerm)
			=> $"region:list:{countryId}:{(string.IsNullOrWhiteSpace(searchTerm) ? "all" : searchTerm.Trim().ToLowerInvariant())}";

		public static string RegionByUrl(string urlName)
						=> $"region:url:{urlName}";

		public static string DistrictsByCity(int cityId, string? searchTerm)
			=> $"district:list:{cityId}:{(string.IsNullOrWhiteSpace(searchTerm) ? "all" : searchTerm.Trim().ToLowerInvariant())}";

		public static string HotelsByGeoNode(string geoNodeType, int geoNodeId, string? searchTerm)
			=> $"hotel:list:{geoNodeType}:{geoNodeId}:{(string.IsNullOrWhiteSpace(searchTerm) ? "all" : searchTerm.Trim().ToLowerInvariant())}";

		public static string CollectionList(string? status, string? geoNodeType, int? sourceId, int pageNumber, int pageSize)
			=> $"collection:list:{status?.Trim().ToLowerInvariant() ?? "all"}:{geoNodeType?.Trim().ToLowerInvariant() ?? "all"}:{sourceId ?? 0}:{pageNumber}:{pageSize}";

		public static string CollectionById(int collectionId)
			=> $"collection:byId:{collectionId}";

		public static string CollectionPreviewHotels(int collectionId)
			=> $"collection:previewHotels:{collectionId}";
	}
}
