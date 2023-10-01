using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Models;
using EmployeeApi.Domain.Response;
using EmployeeApi.Repository.Interface;
using EmployeeApi.Service.Interface;

namespace EmployeeApi.Service.Implementation;

public class DepartmentService: IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    
    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }
    
    public async Task<IBaseResponse> Get()
    {
        var response = new BaseResponse<List<DepartmentWithId>?>();
        
        try
        {
            var departments = await _departmentRepository.Get();

            if (departments.Count == 0)
            {
                return new ErrorResponse()
                {
                    Description = "No departments found",
                    StatusCode = StatusCode.NotFound
                };
            }
            
            response.Data = departments;
            response.StatusCode = StatusCode.OK;

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                Description = e.Message,
                StatusCode = StatusCode.BadRequest
            };
        }
    }

    public async Task<IBaseResponse> Add(CreateDepartmentDto departmentCreate)
    {
        var response = new BaseResponse<int?>();
        
        try
        {
            var department = new Department()
            {
                Name = departmentCreate.Name,
                Phone = departmentCreate.Phone
            };

            var id = await _departmentRepository.Create(department);

            if (id == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not added",
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            response.Data = id;
            response.StatusCode = StatusCode.OK;

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                Description = e.Message,
                StatusCode = StatusCode.BadRequest
            };
        }
    }

    public async Task<IBaseResponse> Update(int id, UpdateDepartmentDto departmentUpdate)
    {
        var response = new BaseResponse<Department>();
        
        try
        {
            
            var departmentFromDb = await _departmentRepository.GetById(id);
            
            if (departmentFromDb == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not found",
                    StatusCode = StatusCode.NotFound
                };
            }
            
            departmentUpdate.Name ??= departmentFromDb.Name;
            departmentUpdate.Phone ??= departmentFromDb.Phone;
            
            var department = new Department()
            {
                Name = departmentUpdate.Name,
                Phone = departmentUpdate.Phone
            };

            var updatedDepartment = await _departmentRepository.Update(id, department);

            if (updatedDepartment == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not updated",
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            response.Data = updatedDepartment;
            response.StatusCode = StatusCode.OK;

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                Description = e.Message,
                StatusCode = StatusCode.BadRequest
            };
        }
    }

    public async Task<IBaseResponse> Delete(int id)
    {
        var response = new BaseResponse<bool>();
        
        try
        {
            var departmentFromDb = await _departmentRepository.GetById(id);
            
            if (departmentFromDb == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not found",
                    StatusCode = StatusCode.NotFound
                };
            }
            var idDelete = await _departmentRepository.Delete(id);
            
            if (idDelete == false)
            {
                return new ErrorResponse()
                {
                    Description = "Department not deleted",
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            response.Data = true;
            response.StatusCode = StatusCode.OK;

            return response;

        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                Description = e.Message,
                StatusCode = StatusCode.BadRequest
            };
        }
    }
}