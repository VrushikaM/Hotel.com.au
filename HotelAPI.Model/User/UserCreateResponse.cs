namespace HotelAPI.Model.User
{
	public class UserCreateResponse
	{
		public int ResultCode { get; set; }
		public int? UserId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? UserName { get; set; }
		public string? Email { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
