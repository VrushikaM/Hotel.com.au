using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Collection;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

	[ApiController]
	[Route("api/collection")]
	public class CollectionController(ICollectionService collectionService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of all available collections.
		/// </summary>

		[HttpGet("list")]
		public async Task<IActionResult> GetCollectionList([FromQuery] string? status, [FromQuery] int? geoNodeId)
		{
			var result = await collectionService.GetCollectionListAsync(status, geoNodeId);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Create or Update Collection
		/// </summary>
		[HttpPost("upsert")]
		public async Task<IActionResult> UpsertCollection([FromBody] CollectionUpsertRequest request)
		{
			var result = await collectionService.UpsertCollectionAsync(request);
			return StatusCode(result.Code, result);
		}
	}
}
