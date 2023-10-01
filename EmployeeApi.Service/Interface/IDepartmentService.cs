using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Response;

namespace EmployeeApi.Service.Interface;

public interface IDepartmentService
{
    public Task<IBaseResponse> Get();
    
    public Task<IBaseResponse> Add(CreateDepartmentDto departmentCreate);
    
    public Task<IBaseResponse> Update(int id, UpdateDepartmentDto departmentUpdate);
    
    public Task<IBaseResponse> Delete(int id);
}