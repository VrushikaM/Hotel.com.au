using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HotelAPI.DAL.Interfaces
{
	public interface ISqlHelper
	{
		Task<T?> GetSingleAsync<T>(string storedProcedureName, DynamicParameters? parameters = null) where T : class;
		Task<T?> ExecuteScalarAsync<T>(string storedProcedureName, DynamicParameters? parameters = null);
		Task ExecuteAsync(string storedProcedureName, DynamicParameters? parameters = null);
		Task<IEnumerable<T>> QueryAsync<T>(string storedProcedureName, DynamicParameters? parameters = null);
		Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedureName, DynamicParameters? parameters = null);
		Task<int> ExecuteRawSqlAsync(string sql, object? parameters = null);
		Task<TResult> QueryMultipleAsync<TResult>(string storedProcedureName, Func<SqlMapper.GridReader, Task<TResult>> readFunc, DynamicParameters? parameters = null);
		Task<DataSet> ExecuteDataSetAsync(string storedProcedure,SqlParameter[] parameters);
	}
}
