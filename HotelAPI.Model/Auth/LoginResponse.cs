namespace HotelAPI.Model.Auth
{
	public class LoginResponse
	{
		public string? Token { get; set; }
		public UserLoginResponse? User { get; set; }
		public List<RolesResponse> Roles { get; set; } = new List<RolesResponse>();
		public List<PagesResponse>? Pages { get; set; } = new List<PagesResponse>();
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
	}
	public class RolesResponse
	{
		public long RoleId { get; set; }
		public string? RoleName { get; set; }
	}
	public class PagesResponse
	{
		public long PageId { get; set; }
		public string? PageName { get; set; }
		public string? Url { get; set; }
	}
}
