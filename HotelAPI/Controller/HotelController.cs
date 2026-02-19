using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/hotel")]
	public class HotelController(IHotelService hotelService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of hotel based on City Id.
		/// </summary>
		/// <param name="cityId">UrlRegistry identifier (City Id)</param>

		[HttpGet("hotelsByCity")]
		public async Task<IActionResult> GetHotelsByCity(int cityId)
		{
			var result = await hotelService.GetHotelsByCityAsync(cityId);
			return StatusCode(result.Code, result);
		}
	}
}
