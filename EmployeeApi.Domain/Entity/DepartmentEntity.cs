namespace EmployeeApi.Domain.Entity;

public class DepartmentEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Phone { get; set; }
    
    public ICollection<EmployeeEntity> Employees { get; set; }
}