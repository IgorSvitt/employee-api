using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    
    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _departmentService.Get();
        
        return new ObjectResult(response)
        {
            StatusCode = (int) response.StatusCode
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CreateDepartmentDto departmentCreate)
    {
        var response = await _departmentService.Add(departmentCreate);
        
        return new ObjectResult(response)
        {
            StatusCode = (int) response.StatusCode
        };
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateDepartmentDto departmentUpdate)
    {
        var response = await _departmentService.Update(id, departmentUpdate);
        
        return new ObjectResult(response)
        {
            StatusCode = (int) response.StatusCode
        };
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _departmentService.Delete(id);
        
        return new ObjectResult(response)
        {
            StatusCode = (int) response.StatusCode
        };
    }
}