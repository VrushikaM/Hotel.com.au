using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.User;

namespace HotelAPI.DAL.Repositories
{
	public class UserRepository(ISqlHelper _sqlHelper) : IUserRepository
	{
		#region CreateUserAsync
		public async Task<UserCreateResponse?> CreateUserAsync(UserCreateRequest model)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@FirstName", model.FirstName);
			parameters.Add("@LastName", model.LastName);
			parameters.Add("@UserName", model.UserName);
			parameters.Add("@Password", model.Password);
			parameters.Add("@Email", model.Email);

			return await _sqlHelper.QueryFirstOrDefaultAsync<UserCreateResponse>(
				StoredProcedure.CreateUser,
				parameters
			);
		}
		#endregion
	}
}
