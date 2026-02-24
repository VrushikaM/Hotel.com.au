using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.Common.Password;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelAPI.BAL.Services
{
	public class AuthService(IAuthRepository _authRepository, IConfiguration _configuration) : IAuthService
	{
		public async Task<ResponseResult<LoginResponse>> LoginAsync(LoginRequest model)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(model.UserName) ||
					string.IsNullOrWhiteSpace(model.Password))
				{
					return ResponseHelper<LoginResponse>.Error(
						"Username and password are required",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				if (!PasswordValidator.ValidatePassword(model.Password, out var errors))
				{
					return ResponseHelper<LoginResponse>.Error(
						"Invalid password format",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				var result = await _authRepository.AuthenticateAsync(model);

				if (result == null)
				{
					return ResponseHelper<LoginResponse>.Error(
						"Invalid username or password",
						statusCode: StatusCode.UNAUTHORIZED
					);
				}

				var token = GenerateJwtToken(result);

				var response = new LoginResponse
				{
					Token = token,
					User = result.User
				};

				return ResponseHelper<LoginResponse>.Success(
					"Login successful",
					response
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<LoginResponse>.Error(
					"Login failed",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public Task<ResponseResult<string>> LogoutAsync()
		{
			try
			{
				return Task.FromResult(
					ResponseHelper<string>.Success("Logout successful")
				);
			}
			catch (Exception ex)
			{
				return Task.FromResult(ResponseHelper<string>.Error(
					"Logout failed",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR)
				);
			}
		}

		private string GenerateJwtToken(LoginResponse result)
		{
			var jwtKey = _configuration["Jwt:Key"];

			if (string.IsNullOrEmpty(jwtKey))
				throw new Exception("JWT Key is not configured.");

			var key = Encoding.ASCII.GetBytes(jwtKey);

			var user = result.User!;

			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new(ClaimTypes.Name, user.UserName ?? string.Empty),
				new(ClaimTypes.Role, user.RoleName ?? string.Empty),
				new(ClaimTypes.Email, user.Email ?? string.Empty)
			};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(24),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature
				)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
