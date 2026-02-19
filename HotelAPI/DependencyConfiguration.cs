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

			//User
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IUserRepository, UserRepository>();

			//Collection
			services.AddScoped<ICollectionService, CollectionService>();
			services.AddScoped<ICollectionRepository, CollectionRepository>();

			//Master Dropdowns
			services.AddScoped<IMasterDropdownService, MasterDropdownService>();
			services.AddScoped<IMasterDropdownRepository, MasterDropdownRepository>();

			// City
			services.AddScoped<ICityRepository, CityRepository>();
			services.AddScoped<ICityService, CityService>();

			// Hotel
			services.AddScoped<IHotelRepository, HotelRepository>();
			services.AddScoped<IHotelService, HotelService>();

			// Region
			services.AddScoped<IRegionRepository, RegionRepository>();
			services.AddScoped<IRegionService, RegionService>();
		}
	}
}
