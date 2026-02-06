using HotelAPI.BAL.Interfaces;
using HotelAPI.BAL.Services;
using HotelAPI.Common.Cache;
using HotelAPI.DAL.Database;
using HotelAPI.DAL.Interfaces;
using HotelAPI.DAL.Repositories;

namespace HotelAPI
{
	public static class DependencyConfiguration
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			// Register SqlHelper 
			services.AddScoped<ISqlHelper, SqlHelper>();

			// Repositories
			services.AddScoped<ICountryRepository, CountryRepository>();

			// Services
			services.AddScoped<ICountryService, CountryService>();

			// CacheService 
			services.AddScoped<ICacheService, MemoryCacheService>();

			// Search 
			services.AddScoped<IGlobalSearchRepository, GlobalSearchRepository>();
			services.AddScoped<IGlobalSearchService, GlobalSearchService>();


		}
	}
}