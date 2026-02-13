using HotelAPI.Common.Helper;
using HotelAPI.Model.Auth;

namespace HotelAPI.BAL.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseResult<LoginResponse>> LoginAsync(LoginRequest model);
	}
}
