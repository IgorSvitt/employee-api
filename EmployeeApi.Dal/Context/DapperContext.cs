using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EmployeeApi.Dal.Context;

public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString(name:"DefaultConnection");
        return new NpgsqlConnection(connectionString);
    }
}