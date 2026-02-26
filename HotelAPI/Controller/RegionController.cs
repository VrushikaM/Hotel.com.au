using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("regions")]
	public class RegionController(IRegionService regionService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of region based on Country Id.
		/// </summary>
		/// <param name="countryId">Country identifier (Country Id)</param>

		[HttpGet]
		public async Task<IActionResult> GetRegionsByCountry(int countryId, string? searchTerm)
		{
			var result = await regionService.GetRegionsByCountryAsync(countryId,searchTerm);
			return StatusCode(result.Code, result);
		}
	}
}
