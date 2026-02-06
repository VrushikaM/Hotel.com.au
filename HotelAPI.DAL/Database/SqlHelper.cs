using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HotelAPI.DAL.Database
{
	public class SqlHelper : ISqlHelper
	{
		private readonly string _connectionString;

		public SqlHelper(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString(ConstantData.HotelConnectionString)
				?? throw new InvalidOperationException(
					$"Connection string '{ConstantData.HotelConnectionString}' not found.");
		}

		private IDbConnection CreateConnection()
			=> new SqlConnection(_connectionString);

		public async Task<T?> GetSingleAsync<T>(
			string storedProcedureName,
			DynamicParameters? parameters = null
		) where T : class
		{
			using var connection = CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<T>(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);
		}

		public async Task<T?> ExecuteScalarAsync<T>(
			string storedProcedureName,
			DynamicParameters? parameters = null
		)
		{
			using var connection = CreateConnection();
			return await connection.ExecuteScalarAsync<T?>(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);
		}

		public async Task ExecuteAsync(
			string storedProcedureName,
			DynamicParameters? parameters = null
		)
		{
			using var connection = CreateConnection();
			await connection.ExecuteAsync(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);
		}

		public async Task<IEnumerable<T>> QueryAsync<T>(
			string storedProcedureName,
			DynamicParameters? parameters = null
		)
		{
			using var connection = CreateConnection();
			return await connection.QueryAsync<T>(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);
		}

		public async Task<T?> QueryFirstOrDefaultAsync<T>(
			string storedProcedureName,
			DynamicParameters? parameters = null
		)
		{
			using var connection = CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<T>(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);
		}

		public async Task<int> ExecuteRawSqlAsync(
			string sql,
			object? parameters = null
		)
		{
			using var connection = CreateConnection();
			return await connection.ExecuteAsync(
				sql,
				parameters,
				commandType: CommandType.Text);
		}

		public async Task<TResult> QueryMultipleAsync<TResult>(
			string storedProcedureName,
			Func<SqlMapper.GridReader, Task<TResult>> readFunc,
			DynamicParameters? parameters = null
		)
		{
			using var connection = CreateConnection();
			using var multi = await connection.QueryMultipleAsync(
				storedProcedureName,
				parameters,
				commandType: CommandType.StoredProcedure);

			return await readFunc(multi);
		}
	}
}
