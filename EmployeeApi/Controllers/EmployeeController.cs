using EmployeeApi.Domain.DTO;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Models;
using EmployeeApi.Domain.Response;
using EmployeeApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _employeeService.Get();
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    
    [HttpGet("company/{id}", Name = "GetByCompanyId")]
    public async Task<IActionResult> GetByCompanyId([FromRoute]int id)
    {
        var response = await _employeeService.GetByCompanyId(id);
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    [HttpGet("department/{id}", Name = "GetByDepartmentId")]
    public async Task<IActionResult> GetByDepartmentId([FromRoute]int id)
    {
        var response = await _employeeService.GetByDepartmentId(id);
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CreateEmployeeDto employeeCreate)
    {
        var response = await _employeeService.Add(employeeCreate);
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var response = await _employeeService.Delete(id);
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateEmployeeDto employeeUpdate)
    {
        var response = await _employeeService.Update(id, employeeUpdate);
        var code = (int) response.StatusCode;

        return new ObjectResult(response)
        {
            StatusCode = code
        };
    }
    
    
}