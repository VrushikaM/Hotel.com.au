using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/hotels")]
	public class HotelController(IHotelService hotelService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of hotel based on City Id.
		/// </summary>
		/// <param name="cityId">City identifier (City Id)</param>
		/// <param name="search">Hotel name search keyword (Search)</param>

		[HttpGet]
		public async Task<IActionResult> GetHotelsByCity(int? cityId, string? search)
		{
			var result = await hotelService.GetHotelsByCityAsync(cityId, search);
			return StatusCode(result.Code, result);
		}
	}
}
