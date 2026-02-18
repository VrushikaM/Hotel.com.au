using static HotelAPI.Common.Helper.Enum;

namespace HotelAPI.Common.Helper
{
	public static class ResponseHelper<T>
	{
		public static ResponseResult<T> Success(string message, T data, StatusCode statusCode = StatusCode.OK)
		{
			return new ResponseResult<T>(ResponseStatus.Success, message, data, statusCode: statusCode);
		}

		public static ResponseResult<T> Success(string message, StatusCode statusCode = StatusCode.OK)
		{
			return new ResponseResult<T>(ResponseStatus.Success, message, statusCode: statusCode);
		}
		public static ResponseResult<T> Success(T data, string message = "Success")
		{
			return new ResponseResult<T>(
				ResponseStatus.Success,
				message,
				data
			);
		}
		public static ResponseResult<T> Error(
			string message,
			List<string>? errors = null,
			Exception? exception = null,
			bool isWarning = false,
			StatusCode statusCode = StatusCode.BAD_REQUEST)
		{
			var exceptionResponse = exception != null
				? new ExceptionResponse(exception, isWarning, message, statusCode)
				: null;

			return new ResponseResult<T>(
				ResponseStatus.Fail,
				message,
				default,
				exceptionResponse,
				errors,
				statusCode
			);
		}
	}

	public class ResponseResult<T>(
	ResponseStatus status,
	string message,
	T? data = default,
	ExceptionResponse? exceptionDetails = null,
	List<string>? errors = null,
	StatusCode statusCode = StatusCode.OK,
	int? totalRecords = null,
	int? pageNumber = null,
	int? pageSize = null,
	string? sortBy = null,
	string? sortOrder = null)
	{
		public string Status { get; } = status.ToString().ToLower();
		public int Code { get; } = (int)statusCode;
		public string Message { get; } = message;
		public T? Data { get; } = data;
		public ExceptionResponse? ExceptionDetails { get; } = exceptionDetails;
		public List<string>? Errors { get; } = errors;
		public int? TotalRecords { get; } = totalRecords;
		public int? PageNumber { get; } = pageNumber;
		public int? PageSize { get; } = pageSize;
		public string? SortBy { get; set; } = sortBy;
		public string? SortOrder { get; set; } = sortOrder;

		public int? TotalPages =>
			PageSize.HasValue && PageSize > 0 && TotalRecords.HasValue
				? (int)Math.Ceiling((double)TotalRecords.Value / PageSize.Value)
				: null;
	}
}
