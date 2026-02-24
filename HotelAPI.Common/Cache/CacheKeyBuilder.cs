namespace HotelAPI.Common.Cache
{
	public class CacheKeyBuilder
	{
		public static string CountryByUrl(string normalizedUrl, string? normalizedAlphabet)
			=> $"country:url:{normalizedUrl}:{normalizedAlphabet ?? "all"}";

		public static string CitiesByCountryOrRegion(int countryId, int? regionId)
			=> $"city:list:{countryId}:{regionId ?? 0}";

		public static string RegionsByCountry(int countryId)
			=> $"region:list:{countryId}";

		public static string HotelsByCity(int? cityId, string? search)
			=> $"hotel:list:{cityId ?? 0}:{search?.Trim().ToLowerInvariant() ?? "all"}";

		public static string CollectionList(string? status, int? countryId, int? regionId, int? cityId)
			=> $"collection:list:{status?.Trim().ToLowerInvariant() ?? "all"}:{countryId ?? 0}:{regionId ?? 0}:{cityId ?? 0}";

		public static string CollectionContent(int collectionId)
			=> $"collection:content:{collectionId}";

		public static string CollectionHistory(int collectionId)
			=> $"collection:history:{collectionId}";
	}
}
