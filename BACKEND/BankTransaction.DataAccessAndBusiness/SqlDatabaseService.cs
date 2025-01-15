using Microsoft.Data.SqlClient;
using System.Data;

namespace BankTransaction.DataAccessAndBusiness;
public class SqlDatabaseService
{
    private readonly string _connectionString;

    public SqlDatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> ExecuteQueryAsync(string query)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<DataTable> ExecuteQueryWithParamsAsync(string query, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }
}
