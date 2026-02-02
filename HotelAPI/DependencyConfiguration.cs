using HotelAPI.BAL.Interfaces;
using HotelAPI.BAL.Services;
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
		}
	}
}