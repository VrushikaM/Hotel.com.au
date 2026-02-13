using HotelAPI.Model.Auth;

namespace HotelAPI.DAL.Interfaces
{
	public interface IAuthRepository
	{
		Task<LoginResponse?> AuthenticateAsync(LoginRequest model);
	}
}
