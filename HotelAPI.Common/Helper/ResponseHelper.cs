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

	public class ResponseResult<T>
	{
		public string Status { get; }
		public int Code { get; }
		public string Message { get; }
		public T? Data { get; }
		public ExceptionResponse? ExceptionDetails { get; }
		public List<string>? Errors { get; }
		public int? TotalRecords { get; }
		public int? PageNumber { get; }
		public int? PageSize { get; }
		public string? SortBy { get; set; }
		public string? SortOrder { get; set; }
		public int? TotalPages
		{
			get
			{
				if (PageSize.HasValue && PageSize > 0 && TotalRecords.HasValue)
				{
					return (int)Math.Ceiling((double)TotalRecords.Value / PageSize.Value);
				}
				return null;
			}
		}
		public ResponseResult(
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
			Status = status.ToString().ToLower();
			Code = (int)statusCode;
			Message = message;
			Data = data;
			ExceptionDetails = exceptionDetails;
			Errors = errors;
			TotalRecords = totalRecords;
			PageNumber = pageNumber;
			PageSize = pageSize;
			SortBy = sortBy;
			SortOrder = sortOrder;
		}
	}
}
