using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.Common.Password;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Auth;

namespace HotelAPI.DAL.Repositories
{
	public class AuthRepository(ISqlHelper _sqlHelper) : IAuthRepository
	{

		#region AuthenticateAsync
		public async Task<LoginResponse?> AuthenticateAsync(LoginRequest model)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@Username", model.UserName);
			parameters.Add("@Password", model.Password);

			return await _sqlHelper.QueryMultipleAsync(StoredProcedure.LoginAuthentication, async multi =>
			{
				// 1️⃣ User Basic Info
				var user = await multi.ReadFirstOrDefaultAsync<UserLoginResponse>();
				if (user == null)
					return null;

				// 🔐 2️⃣ Verify Password (HASH CHECK)
				if (string.IsNullOrEmpty(user.Password) ||
					!PasswordHasher.VerifyPassword(model.Password!, user.Password))
				{
					return null;
				}

				return new LoginResponse
				{
					User = user
				};
			},
			parameters
			);
		}
		#endregion
	}
}
