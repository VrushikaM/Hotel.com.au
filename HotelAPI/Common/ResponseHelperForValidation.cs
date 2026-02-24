using FluentValidation;
using HotelAPI.Common.Helper;

namespace HotelAPI.Common
{
	public static class ResponseHelperForValidation
	{
		public static async Task<List<string>?> GetValidationErrorsAsync<T>(T request, IValidator<T> validator)
		{
			var validationResult = await validator.ValidateAsync(request);
			if (validationResult.IsValid)
				return null;

			return validationResult.Errors.Select(e => e.ErrorMessage).ToList();
		}
		public static IResult ValidationError<T>(List<string> errors)
		{
			var response = ResponseHelper<T>.Error(
				"Validation failed",
				errors,
				statusCode: StatusCode.FORBIDDEN
			);

			return Results.Json(response, statusCode: StatusCodes.Status403Forbidden);
		}
	}
}
