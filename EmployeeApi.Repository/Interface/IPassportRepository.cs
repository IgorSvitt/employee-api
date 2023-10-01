using System.Data;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.Entity;

namespace EmployeeApi.Repository.Interface;

public interface IPassportRepository
{
    public Task<int?> Create(CreatePassportDto entity, IDbTransaction transaction);
    
    public Task<bool> Update(PassportEntity entity, IDbTransaction transaction);
    
    public Task<bool> Delete(int id, IDbTransaction transaction);   
}