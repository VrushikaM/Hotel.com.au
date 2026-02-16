using Azure;
using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.Common.Password;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.User;

namespace HotelAPI.BAL.Services
{
	public class UserService(IUserRepository _userRepository) : IUserService
	{
		public async Task<ResponseResult<UserCreateResponse>> CreateUserAsync(UserCreateRequest model)
		{
			try
			{
				// 1️⃣ Basic Validation
				if (string.IsNullOrWhiteSpace(model.UserName) ||
					string.IsNullOrWhiteSpace(model.Password) ||
					string.IsNullOrWhiteSpace(model.Email))
				{
					return ResponseHelper<UserCreateResponse>.Error(
						"Username, Email and Password are required",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 2️⃣ Password Format Validation
				if (!PasswordValidator.ValidatePassword(model.Password, out var errors))
				{
					return ResponseHelper<UserCreateResponse>.Error(
						"Invalid password format",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 3️⃣ Hash Password Before Sending To DB
				model.Password = PasswordHasher.HashPassword(model.Password);

				// 4️⃣ Call Repository
				var result = await _userRepository.CreateUserAsync(model);

				if (result == null)
				{
					return ResponseHelper<UserCreateResponse>.Error(
						"User creation failed",
						statusCode: StatusCode.INTERNAL_SERVER_ERROR
					);
				}

				// 5️⃣ Handle SP ResultCode
				switch (result.ResultCode)
				{
					case 0:
						return ResponseHelper<UserCreateResponse>.Error(
							"Username already exists",
							statusCode: StatusCode.CONFLICT_OCCURS
						);

					case -1:
						return ResponseHelper<UserCreateResponse>.Error(
							"Email already exists",
							statusCode: StatusCode.CONFLICT_OCCURS
						);

					case 1:
						return ResponseHelper<UserCreateResponse>.Success(
							"User created successfully",
							result,
							statusCode: StatusCode.CREATED
						);

					default:
						return ResponseHelper<UserCreateResponse>.Error(
							"Unknown error occurred",
							statusCode: StatusCode.INTERNAL_SERVER_ERROR
						);
				}
			}
			catch (Exception ex)
			{
				return ResponseHelper<UserCreateResponse>.Error(
					"User creation failed",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
