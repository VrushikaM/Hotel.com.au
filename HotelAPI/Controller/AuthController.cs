using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("auth")]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		/// <summary>
		/// Authenticates user and returns JWT token
		/// </summary>

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest model)
		{
			var result = await authService.LoginAsync(model);
			return StatusCode(result.Code, result);
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var result = await authService.LogoutAsync();
			return StatusCode(result.Code, result);
		}
	}
}
