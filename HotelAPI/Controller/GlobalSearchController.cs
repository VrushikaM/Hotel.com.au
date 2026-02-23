using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class GlobalSearchController(IGlobalSearchService globalSearchService) : ControllerBase
	{
		/// <summary>
		/// Global  search (City, Region, District, Hotel, etc.)
		/// </summary>
		/// <param name="q">Search text (minimum 2 characters)</param>

		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] string q)
		{
			var result = await globalSearchService.SearchAsync(q);
			return StatusCode(result.Code, result);
		}
	}
}
