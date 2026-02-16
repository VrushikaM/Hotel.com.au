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

				// 2️⃣ Roles
				var roles = (await multi.ReadAsync<RolesResponse>()).ToList();

				// 3️⃣ Pages
				var pages = (await multi.ReadAsync<PagesResponse>()).ToList();

				return new LoginResponse
				{
					User = user,
					Roles = roles,
					Pages = pages,
					HasRole = roles.Any(),
					HasAccess = pages.Any()
				};
			},
			parameters
			);
		}
		#endregion
	}
}
