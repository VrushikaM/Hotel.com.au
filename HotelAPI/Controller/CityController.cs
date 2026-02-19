using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/city")]
	public class CityController(ICityService cityService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of city based on Country Id or Region Id.
		/// </summary>
		/// <param name="countryId">Country identifier (Country Id)</param>
		/// <param name="regionId">Region identifier (Region Id)</param>

		[HttpGet("citiesByCountryOrRegion")]
		public async Task<IActionResult> GetCitiesByCountryOrRegion(int countryId, int? regionId)
		{
			var result = await cityService.GetCitiesByCountryOrRegionAsync(countryId, regionId);
			return StatusCode(result.Code, result);
		}
	}
}
