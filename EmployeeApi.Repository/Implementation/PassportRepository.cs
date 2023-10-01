using System.Data;
using Dapper;
using EmployeeApi.Dal.Context;
using EmployeeApi.Domain.DTO;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.Entity;
using EmployeeApi.Domain.Models;
using EmployeeApi.Repository.Interface;

namespace EmployeeApi.Repository.Implementation;

public class PassportRepository : IPassportRepository
{
    private readonly DapperContext _context;

    public PassportRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int?> Create(CreatePassportDto entity, IDbTransaction transaction)
    {
        string insertPassportSql = @"
                INSERT INTO Passport (Type, Number)
                VALUES (@Type, @Number)
                RETURNING Id;";

        try
        {
            int newPassportId = await transaction.Connection.QuerySingleAsync<int>(insertPassportSql, new
                {
                    entity.Type,
                    entity.Number
                },
                transaction);
            return newPassportId;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> Update(PassportEntity entity, IDbTransaction transaction)
    {
        var query = "UPDATE Passport SET Type = @Type, Number = @Number WHERE Id = @Id";
        try
        {
            var result = await transaction.Connection.ExecuteAsync(query, new
            {
                entity.Type,
                entity.Number,
                entity.Id
            }, transaction);
            return result > 0;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<bool> Delete(int id, IDbTransaction transaction)
    {
        var query = "DELETE FROM Passport WHERE Id = @Id";
        try
        {
            var result = await transaction.Connection.ExecuteAsync(query, new {Id = id}, transaction);
            return result > 0;
        }
        catch (Exception)
        {
            return false;
        }
           
    }
}