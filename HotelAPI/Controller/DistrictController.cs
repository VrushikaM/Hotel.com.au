using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("districts")]
	public class DistrictController(IDistrictService districtService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of district based on City Id.
		/// </summary>
		/// <param name="cityId">City identifier (City Id)</param>

		[HttpGet]
		public async Task<IActionResult> GetDistrictsByCity(int cityId, string? searchTerm)
		{
			var result = await districtService.GetDistrictsByCityAsync(cityId, searchTerm);
			return StatusCode(result.Code, result);
		}
	}
}
