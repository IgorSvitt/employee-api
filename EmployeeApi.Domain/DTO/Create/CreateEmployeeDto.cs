namespace EmployeeApi.Domain.DTO.Create;

public class CreateEmployeeDto
{
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Phone { get; set; }

    public int CompanyId { get; set; }
    
    public int DepartmentId { get; set; }
    
    public CreatePassportDto Passport { get; set; }
}