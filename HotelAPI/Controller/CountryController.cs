using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/countries")]
	public class CountryController(ICountryService countryService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of all available countries.
		/// </summary>

		[HttpGet]
		public async Task<IActionResult> GetCountryList(string? searchTerm)
		{
			var result = await countryService.GetCountryListAsync(searchTerm);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves country details based on the country URL name.
		/// </summary>
		/// <param name="urlName">Country URL identifier</param>
		/// <param name="alphabet">Optional alphabet filter</param>

		[HttpGet("{urlName}")]
		public async Task<IActionResult> GetCountryByUrl(string urlName, string? alphabet)
		{
			var result = await countryService.GetCountryByUrlAsync(urlName, alphabet);
			return StatusCode(result.Code, result);
		}
	}
}
