using EmployeeApi.Domain.DTO;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Models;
using EmployeeApi.Domain.Response;

namespace EmployeeApi.Service.Interface;

public interface IEmployeeService
{
    public Task<IBaseResponse> Get();
    
    public Task<IBaseResponse> GetByCompanyId(int id);
    
    public Task<IBaseResponse> GetByDepartmentId(int id);
    
    public Task<IBaseResponse> Add(CreateEmployeeDto employeeCreate);
    
    public Task<IBaseResponse> Delete(int id);
    
    public Task<IBaseResponse> Update(int id, UpdateEmployeeDto employeeUpdate);
}