namespace EmployeeApi.Domain.Entity;

public class EmployeeEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Phone { get; set; }

    public int CompanyId { get; set; }
    
    public int PassportId { get; set; }

    public PassportEntity Passport { get; set; }
    
    public int DepartmentId { get; set; }

    public DepartmentEntity Department { get; set; }
}