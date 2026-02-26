using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("users")]
	public class UserController(IUserService userService) : ControllerBase
	{
		/// <summary>
		/// Creates a new user.
		/// </summary>
		/// <param name="model">User creation request</param>

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest model)
		{
			var result = await userService.CreateUserAsync(model);
			return StatusCode(result.Code, result);
		}
	}
}
