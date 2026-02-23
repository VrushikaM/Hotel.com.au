using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

	[ApiController]
	[Route("api/collection")]
	public class CollectionController(ICollectionService collectionService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of collections with optional filtering by status, country, region, and city.
		/// Supports hierarchical filtering where city has highest priority,
		/// followed by region, then country. If no filters are provided,
		/// all collections are returned.
		/// </summary>
		/// <param name="status">Optional collection status (e.g., Published, Draft).</param>
		/// <param name="countryId">Optional country identifier for filtering country-level collections.</param>
		/// <param name="regionId">Optional region identifier for filtering region-level collections.</param>
		/// <param name="cityId">Optional city identifier for filtering city-level collections.</param>
		/// <returns>Returns the filtered list of collections.</returns>

		[HttpGet("list")]
		public async Task<IActionResult> GetCollectionList([FromQuery] string? status, [FromQuery] int? countryId, [FromQuery] int? regionId, [FromQuery] int? cityId)
		{
			var result = await collectionService.GetCollectionListAsync(status, countryId, regionId, cityId);
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

		[HttpPost("save")]
		public async Task<IActionResult> Save(CollectionContentRequest request)
		{
			var result = await collectionService.SaveAsync(request);
			return StatusCode((int)result.Code, result);
		}

		[HttpGet("{collectionId}")]
		public async Task<IActionResult> Get(int collectionId)
		{
			var result = await collectionService.GetAsync(collectionId);
			return StatusCode((int)result.Code, result);
		}

		[HttpGet("history/{collectionId}")]
		public async Task<IActionResult> GetHistory(int collectionId)
		{
			var result = await collectionService.GetHistoryAsync(collectionId);
			return StatusCode((int)result.Code, result);
		}
	}
}
