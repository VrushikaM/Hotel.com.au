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
			// SqlHelper 
			services.AddScoped<ISqlHelper, SqlHelper>();

			// Country
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<ICountryService, CountryService>();

			// Cache
			services.AddScoped<ICacheService, MemoryCacheService>();

			// Search 
			services.AddScoped<IGlobalSearchRepository, GlobalSearchRepository>();
			services.AddScoped<IGlobalSearchService, GlobalSearchService>();

			// Auth
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IAuthRepository, AuthRepository>();
		}
	}
}