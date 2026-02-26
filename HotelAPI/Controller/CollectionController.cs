using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

	[ApiController]
	[Route("collections")]
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

		[HttpGet]
		public async Task<IActionResult> GetCollectionList([FromQuery] string? status, [FromQuery] int? countryId, [FromQuery] int? regionId, [FromQuery] int? cityId)
		{
			var result = await collectionService.GetCollectionListAsync(status, countryId, regionId, cityId);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Creates a new collection.
		/// If `CollectionId` is provided in the request body, it will be ignored and a new collection is created.
		/// </summary>
		/// <param name="request">The collection details to create.</param>
		/// <returns>Returns the created collection with assigned CollectionId.</returns>

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CollectionUpsertRequest request)
		{
			request.CollectionId = null;
			var result = await collectionService.UpsertCollectionAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Updates an existing collection identified by the provided `id`.
		/// The `CollectionId` in the request body is overridden with the URL `id`.
		/// </summary>
		/// <param name="id">Identifier of the collection to update.</param>
		/// <param name="request">Updated collection details.</param>
		/// <returns>Returns the updated collection information.</returns>

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] CollectionUpsertRequest request)
		{
			request.CollectionId = id;
			var result = await collectionService.UpsertCollectionAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves all content items for the specified collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <returns>Returns the list of content items for the collection.</returns>

		[HttpGet("{id}/content")]
		public async Task<IActionResult> GetContent(int id)
		{
			var result = await collectionService.GetAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Adds a new content item to the specified collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <param name="request">Content details to add.</param>
		/// <returns>Returns the newly added content item.</returns>

		[HttpPost("{id}/content")]
		public async Task<IActionResult> UpsertContent(int id, [FromBody] CollectionContentRequest request)
		{
			request.CollectionId = id;
			var result = await collectionService.UpsertContentAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves the history of content changes for the specified collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <returns>Returns a list of historical content changes with timestamps and user info (if available).</returns>

		[HttpGet("{id}/content/history")]
		public async Task<IActionResult> GetContentHistory(int id)
		{
			var result = await collectionService.GetHistoryAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Saves a collection rules based on the provided request details.
		/// </summary>
		/// <param name="request">The collection rules information to save.</param>
		/// <returns>Returns the saved collection rules and operation status.</returns>

		[HttpPost("rules")]
		public async Task<IActionResult> UpsertRules([FromBody] CollectionRuleRequest request)
		{
			var result = await collectionService.UpsertRulesAsync(request);
			return StatusCode(result.Code, result);
		}
		
		/// <summary>
		/// Retrieves a specific collection rules by its identifier.
		/// </summary>
		/// <param name="id">Identifier of the collection rules.</param>
		/// <returns>Returns the collection rules details if found, along with operation status.</returns>

		[HttpGet("rules/{id}")]
		public async Task<IActionResult> GetRulesById(int id)
		{
			var result = await collectionService.GetRulesByIdAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Changes the status of a collection to Draft or Publish.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <param name="action">Status action (Draft or Publish).</param>
		/// <returns>Returns the updated collection identifier.</returns>

		[HttpPost("{id}/status")]
		public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string action)
		{
			var result = await collectionService.ChangeStatusAsync(id, action);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Saves pinned and excluded hotels for a collection.
		/// </summary>
		/// <param name="request">Pinned and excluded hotel details.</param>
		/// <returns>Returns CollectionId and created ExclusionIds.</returns>

		[HttpPost("curations")]
		public async Task<IActionResult> UpsertCurations([FromBody] CollectionCurationRequest request)
		{
			var result = await collectionService.UpsertCurationsAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves pinned and excluded hotels for a specific collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <returns>
		/// Returns pinned hotels (with Position & PinType) 
		/// and excluded hotels (with ChainID & Reason).
		/// </returns>
		[HttpGet("curations/{id}")]
		public async Task<IActionResult> GetCurationsById(int id)
		{
			var result = await collectionService.GetCurationsByIdAsync(id);
			return StatusCode(result.Code, result);
		}
	}
}
