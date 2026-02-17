namespace HotelAPI.Model.Auth
{
	public class LoginResponse
	{
		public string? Token { get; set; }
		public UserLoginResponse? User { get; set; }
		public List<string>? Errors { get; set; }
	}
	public class UserLoginResponse
	{
		public long UserId { get; set; }
		public string? Password { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? UserName { get; set; }
		public string? Email { get; set; }
		public long RoleId { get; set; }
		public string? RoleName { get; set; }
	}
}
