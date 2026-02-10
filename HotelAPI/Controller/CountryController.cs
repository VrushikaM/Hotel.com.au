using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/country")]
	public class CountryController(ICountryService countryService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of all available countries.
		/// </summary>

		[HttpGet("list")]
		public async Task<IActionResult> GetCountryList()
		{
			var result = await countryService.GetCountryListAsync();
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves country details based on the country URL name.
		/// </summary>
		/// <param name="urlName">Country URL identifier</param>
		/// <param name="alphabet">Optional alphabet filter</param>

		[HttpGet("getByUrl/{urlName}")]
		public async Task<IActionResult> GetCountryByUrl(string urlName, string? alphabet)
		{
			var result = await countryService.GetCountryByUrlAsync(urlName, alphabet);
			return StatusCode(result.Code, result);
		}
	}
}
