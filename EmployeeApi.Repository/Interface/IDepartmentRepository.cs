using EmployeeApi.Domain.Models;

namespace EmployeeApi.Repository.Interface;

public interface IDepartmentRepository
{
    public Task<Department?> GetById(int? id);
    
    public Task<List<DepartmentWithId>?> Get();
    
    public Task<int> Create(Department department);
    
    public Task<Department> Update(int id, Department department);
    
    public Task<bool> Delete(int id);
}