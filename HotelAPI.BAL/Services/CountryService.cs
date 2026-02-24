using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Services
{
	public class CountryService(ICountryRepository _countryRepository, ICacheService _cache) : ICountryService
	{
		public async Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync(string? searchTerm)
		{
			try
			{
				var cacheKey = CacheKeyBuilder.CountryList(searchTerm);

				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _countryRepository.GetCountryListAsync(searchTerm),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<CountryListResponse>();

				return ResponseHelper<IEnumerable<CountryListResponse>>.Success(
					"Country list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
					"Failed to fetch country list",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<CountryByUrlResponse>> GetCountryByUrlAsync(string urlName, string? alphabet)
		{
			string? normalizedAlphabet = null;

			if (string.IsNullOrWhiteSpace(urlName))
			{
				return ResponseHelper<CountryByUrlResponse>.Error(
					"UrlName is required.",
					statusCode: StatusCode.BAD_REQUEST
				);
			}

			if (!string.IsNullOrWhiteSpace(alphabet))
			{
				alphabet = alphabet.Trim().ToLowerInvariant();

				if (alphabet.Length != 1 || !char.IsLetter(alphabet[0]))
				{
					return ResponseHelper<CountryByUrlResponse>.Error(
							"Alphabet must be a single letter (a-z).",
							statusCode: StatusCode.BAD_REQUEST
					);
				}

				normalizedAlphabet = alphabet;
			}

			var normalizedUrl = urlName.Trim().ToLowerInvariant();

			var cacheKey = CacheKeyBuilder.CountryByUrl(normalizedUrl, normalizedAlphabet);

			try
			{
				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _countryRepository.GetCountryByUrlAsync(normalizedUrl, normalizedAlphabet),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CountryByUrlResponse>.Error(
						"No country found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CountryByUrlResponse>.Success(
					"Country fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CountryByUrlResponse>.Error(
					"Failed to fetch country",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
