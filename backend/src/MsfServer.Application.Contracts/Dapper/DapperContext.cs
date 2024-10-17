using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using System.Data.SqlClient;

namespace MsfServer.Application.Contracts.Dapper
{
    public class DapperContext(string connectionString) : IDisposable
    {
        private readonly string _connectionString = connectionString ??
            throw new CustomException(StatusCodes.Status500InternalServerError, "Không kết nối được với cơ sở dữ liệu.");
        private SqlConnection? _connection;

        public SqlConnection GetOpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
