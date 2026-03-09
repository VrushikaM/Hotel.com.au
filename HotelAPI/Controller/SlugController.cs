using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("slug")]
	public class SlugController(ISlugService slugService) : ControllerBase
	{
		/// <summary>
		/// Resolves a slug to its corresponding geographic node details
		/// (Country, Region, City, or District).
		/// </summary>
		/// <param name="slug">Slug value to resolve.</param>
		/// <returns>Returns the geographic hierarchy details.</returns>

		[HttpGet]
		public async Task<IActionResult> ResolveSlug([FromQuery] string slug)
		{
			var result = await slugService.ResolveSlugAsync(slug);
			return StatusCode(result.Code, result);
		}
	}
}
