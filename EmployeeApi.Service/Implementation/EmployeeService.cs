using EmployeeApi.Domain.DTO;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Models;
using EmployeeApi.Domain.Response;
using EmployeeApi.Repository.Interface;
using EmployeeApi.Service.Interface;

namespace EmployeeApi.Service.Implementation;

public class EmployeeService : IEmployeeService
{

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<IBaseResponse> Get()
    {
        var response = new BaseResponse<List<Employee>>();
        try
        {
            var employees = await _employeeRepository.Get();

            if (employees.Count == 0)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Employees not found",
                };
            }

            response.Data = employees;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IBaseResponse> GetByCompanyId(int id)
    {
        var response = new BaseResponse<List<Employee>>();

        try
        {
            var employees = await _employeeRepository.GetByCompanyId(id);

            if (employees.Count == 0)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Employees not found",
                };
            }

            response.Data = employees;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                StatusCode = StatusCode.BadRequest,
                Description = e.Message
            };
        }
    }

    public async Task<IBaseResponse> GetByDepartmentId(int id)
    {
        var response = new BaseResponse<List<Employee>>();

        try
        {
            var department = await _departmentRepository.GetById(id);

            if (department == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not found",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var employees = await _employeeRepository.GetByDepartmentId(id);

            if (employees.Count == 0)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Employees not found",
                };
            }

            response.Data = employees;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                StatusCode = StatusCode.BadRequest,
                Description = e.Message
            };
        }
    }

    public async Task<IBaseResponse> Add(CreateEmployeeDto employeeCreate)
    {
        var response = new BaseResponse<int?>();

        try
        {
            var department = await _departmentRepository.GetById(employeeCreate.DepartmentId);

            if (department == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not found",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var employeeId = await _employeeRepository.Create(employeeCreate);

            if (employeeId == null)
            {
                return new ErrorResponse()
                {
                    Description = "Employee not created",
                    StatusCode = StatusCode.BadRequest
                };
            }

            response.Data = employeeId;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                StatusCode = StatusCode.NotFound,
                Description = e.Message
            };
        }
    }

    public async Task<IBaseResponse> Delete(int id)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var employee = await _employeeRepository.GetById(id);

            if (employee == null)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Employee not found"
                };
            }

            var isDelete = await _employeeRepository.Delete(employee.Id, employee.Passport.Id);

            if (!isDelete)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Employee not deleted"
                };
            }

            response.Data = isDelete;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                StatusCode = StatusCode.BadRequest,
                Description = e.Message
            };
        }
    }

    public async Task<IBaseResponse> Update(int id, UpdateEmployeeDto employeeUpdate)
    {
        var response = new BaseResponse<Employee>();

        try
        {
            var employee = await _employeeRepository.GetById(id);

            if (employee == null)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Employee not found"
                };
            }
            
            employeeUpdate.DepartmentId ??= employee.Department.Id;
            
            var department = await _departmentRepository.GetById(employeeUpdate.DepartmentId);

            if (department == null)
            {
                return new ErrorResponse()
                {
                    Description = "Department not found",
                    StatusCode = StatusCode.BadRequest
                };
            }
            
            employeeUpdate.CompanyId ??= employee.CompanyId;
            employeeUpdate.Name ??= employee.Name;
            employeeUpdate.Surname ??= employee.Surname;
            employeeUpdate.Phone ??= employee.Phone;
            employeeUpdate.Passport ??= new UpdatePassportDto()
            {
                Type = employee.Passport.Type,
                Number = employee.Passport.Number
            };

            var result = await _employeeRepository.Update(id, employeeUpdate);

            if (result == null)
            {
                return new ErrorResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Employee not updated"
                };
            }

            response.Data = result;
            response.StatusCode = StatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponse()
            {
                StatusCode = StatusCode.BadRequest,
                Description = e.Message
            };
        }
    }
}