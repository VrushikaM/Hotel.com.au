using HotelAPI.BAL.Interfaces;
using HotelAPI.BAL.Services;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

    [ApiController]
    [Route("api/collections")]
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

        /// POST /api/collections  (CREATE)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CollectionUpsertRequest request)
        {
            request.CollectionId = null;   
            var result = await collectionService.UpsertCollectionAsync(request);
            return StatusCode(result.Code, result);
        }

        /// PUT /api/collections/{id}  (UPDATE)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CollectionUpsertRequest request)
        {
            request.CollectionId = id;
            var result = await collectionService.UpsertCollectionAsync(request);
            return StatusCode(result.Code, result);
        }

        /// GET /api/collections/{id}/content
        [HttpGet("{id}/content")]
        public async Task<IActionResult> GetContent(int id)
        {
            var result = await collectionService.GetAsync(id);
            return StatusCode((int)result.Code, result);
        }

        /// POST /api/collections/{id}/content
        [HttpPost("{id}/content")]
        public async Task<IActionResult> CreateContent(int id, [FromBody] CollectionContentRequest request)
        {
            request.CollectionId = id;
            var result = await collectionService.SaveAsync(request);
            return StatusCode((int)result.Code, result);
        }

        /// GET /api/collections/{id}/content/history
        [HttpGet("{id}/content/history")]
        public async Task<IActionResult> GetContentHistory(int id)
        {
            var result = await collectionService.GetHistoryAsync(id);
            return StatusCode((int)result.Code, result);
        }
    }
}
