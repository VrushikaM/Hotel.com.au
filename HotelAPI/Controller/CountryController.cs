using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/country")]
	public class CountryController : ControllerBase
	{
		private readonly ICountryService _countryService;

		public CountryController(ICountryService countryService)
		{
			_countryService = countryService;
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetCountryList()
		{
			var result = await _countryService.GetCountryListAsync();
			return StatusCode(result.Code, result);
		}

		[HttpGet("get-content-by-country-id/{countryId:long}")]
		public async Task<IActionResult> GetContentByCountryId(long countryId)
		{
			var result = await _countryService.GetContentByCountryAsync(countryId);
			return StatusCode(result.Code, result);
		}

		[HttpGet("get-regions-by-country-id/{countryId:long}")]
		public async Task<IActionResult> GetRegionsByCountryId(long countryId)
		{
			var result = await _countryService.GetRegionsByCountryAsync(countryId);
			return StatusCode(result.Code, result);
		}
	}
}
