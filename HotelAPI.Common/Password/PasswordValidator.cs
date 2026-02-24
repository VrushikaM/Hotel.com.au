namespace HotelAPI.Common.Password
{
	public class PasswordValidator
	{
		public static bool ValidatePassword(string password, out List<string> errorMessages)
		{
			errorMessages = new List<string>();

			if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
			{
				errorMessages.Add("Password must be at least 8 characters long.");
			}

			if (!password.Any(char.IsDigit))
			{
				errorMessages.Add("Password must contain at least one digit.");
			}

			if (!password.Any(char.IsLower))
			{
				errorMessages.Add("Password must contain at least one lowercase letter.");
			}

			if (!password.Any(char.IsUpper))
			{
				errorMessages.Add("Password must contain at least one uppercase letter.");
			}

			if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
			{
				errorMessages.Add("Password must contain at least one special character.");
			}

			return !errorMessages.Any();
		}
	}
}
