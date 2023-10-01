using Dapper;
using EmployeeApi.Dal.Context;
using EmployeeApi.Domain.Models;
using EmployeeApi.Repository.Interface;

namespace EmployeeApi.Repository.Implementation;

public class DepartmentRepository: IDepartmentRepository
{
    private readonly DapperContext _context;
    
    public DepartmentRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<Department?> GetById(int? id)
    {
        var query = "SELECT * FROM Department WHERE Id = @Id";
        
        using(var connection = _context.CreateConnection())
        {
            var result = await connection.QueryFirstOrDefaultAsync<Department>(query, new { Id = id });
            return result;
        }
    }

    public async Task<List<DepartmentWithId>?> Get()
    {
        var query = "SELECT * FROM Department";
        
        using(var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DepartmentWithId>(query);
            return result.ToList();
        }
    }

    public async Task<int> Create(Department department)
    {
        string query = @"
                INSERT INTO Department (Name, Phone)
                VALUES (@Name, @Phone)
                RETURNING Id;";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleAsync<int>(query, new
            {
                department.Name,
                department.Phone
            });
            return result;
        }
    }

    public async Task<Department> Update(int id, Department department)
    {
        var query = @"
                UPDATE Department
                SET Name = @Name,
                    Phone = @Phone
                WHERE Id = @Id
                RETURNING *;";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleAsync<Department>(query, new
            {
                department.Name,
                department.Phone,
                Id = id
            });
            return result;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var query = @"
                DELETE FROM Department
                WHERE Id = @Id;";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.ExecuteAsync(query, new
            {
                Id = id
            });
            return result > 0;
        }
    }
}