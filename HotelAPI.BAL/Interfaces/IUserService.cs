using HotelAPI.Common.Helper;
using HotelAPI.Model.User;

namespace HotelAPI.BAL.Interfaces
{
	public interface IUserService
	{
		Task<ResponseResult<UserCreateResponse>> CreateUserAsync(UserCreateRequest model);
	}
}
