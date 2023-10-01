using EmployeeApi.Domain.DTO;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Entity;
using EmployeeApi.Domain.Models;

namespace EmployeeApi.Repository.Interface;

public interface IEmployeeRepository
{
    public Task<EmployeeEntity?> GetById(int id);
    
    public Task<List<Employee>?> GetByCompanyId(int id);
    
    public Task<int?> Create(CreateEmployeeDto entity);
    
    public Task<List<Employee>?> GetByDepartmentId(int id);
    
    public Task<Employee?> Update(int id, UpdateEmployeeDto entity);
    
    public Task<List<Employee>?> Get();
    
    public Task<bool> Delete(int id, int passportId);
}