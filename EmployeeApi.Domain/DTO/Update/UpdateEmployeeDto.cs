namespace EmployeeApi.Domain.DTO.Update;

public class UpdateEmployeeDto
{
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Phone { get; set; }

    public int? CompanyId { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public UpdatePassportDto? Passport { get; set; }
}