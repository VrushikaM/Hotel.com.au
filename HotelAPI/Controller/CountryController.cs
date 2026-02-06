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

		[HttpGet("getByUrl/{urlName}")]
		public async Task<IActionResult> GetCountryByUrl(string urlName, string? alphabet)
		{
			var result = await _countryService.GetCountryByUrlAsync(urlName, alphabet);
			return StatusCode(result.Code, result);
		}
	}
}
