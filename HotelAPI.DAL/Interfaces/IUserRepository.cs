using HotelAPI.Model.User;

namespace HotelAPI.DAL.Interfaces
{
	public interface IUserRepository
	{
		Task<UserCreateResponse?> CreateUserAsync(UserCreateRequest model);
	}
}
