using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/city")]
	public class CityController(ICityService cityService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of city based on Registry Id.
		/// </summary>
		/// <param name="geoNodeId">UrlRegistry identifier (Registry Id)</param>

		[HttpGet("citiesByUrlRegistry")]
		public async Task<IActionResult> GetCitiesByUrlRegistry(int registryId)
		{
			var result = await cityService.GetCitiesByUrlRegistryAsync(registryId);
			return StatusCode(result.Code, result);
		}
	}
}
