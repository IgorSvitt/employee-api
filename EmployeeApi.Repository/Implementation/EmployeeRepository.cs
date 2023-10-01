using System.Data;
using Dapper;
using EmployeeApi.Dal.Context;
using EmployeeApi.Domain.DTO.Create;
using EmployeeApi.Domain.DTO.Update;
using EmployeeApi.Domain.Entity;
using EmployeeApi.Domain.Models;
using EmployeeApi.Repository.Interface;

namespace EmployeeApi.Repository.Implementation;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperContext _context;
    private readonly IPassportRepository _passportRepository;

    public EmployeeRepository(DapperContext context, IPassportRepository passportRepository)
    {
        _context = context;
        _passportRepository = passportRepository;
    }

    public async Task<Employee?> Update(int id, UpdateEmployeeDto entity)
    {
        var query = @"
            UPDATE Employee SET name = @Name, surname = @Surname, phone = @Phone, 
            company_id = @CompanyId, department_id = @DepartmentId 
            WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var employee = await GetById(id);
                    var passport = new PassportEntity()
                    {
                        Number = entity.Passport.Number,
                        Type = entity.Passport.Type,
                        Id = employee.Passport.Id
                    };
                    var updatedPassport = await _passportRepository.Update(passport, transaction);
                    
                    var updated = await connection.ExecuteAsync(query, new
                    {
                        entity.Name,
                        entity.Surname,
                        entity.Phone,
                        entity.CompanyId,
                        entity.DepartmentId,
                        Id = id
                    });
                    if (updated <= 0 || !updatedPassport) return null;
                    
                    transaction.Commit();
                    
                    var newEntity = await GetById(id);
                    return new Employee()
                    {
                        Id = newEntity.Id,
                        Name = newEntity.Name,
                        Surname = newEntity.Surname,
                        Phone = newEntity.Phone,
                        CompanyId = newEntity.CompanyId,
                        Department = new Department()
                        {
                            Phone = newEntity.Department.Phone,
                            Name = newEntity.Department.Name
                        },
                        Passport = new Passport()
                        {
                            Number = newEntity.Passport.Number,
                            Type = newEntity.Passport.Type
                        },
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }

    public async Task<List<Employee>?> Get()
    {
        var query = @"
            SELECT e.Id, e.Name, e.Surname, e.Phone,e.Company_Id AS CompanyId, e.Department_Id AS DepartmentId, e.Passport_Id AS PassportId,
            p.Id, p.Type, p.Number, d.Id, d.Name, d.Phone FROM Employee e
            INNER JOIN Passport p ON e.Passport_Id = p.Id
            INNER JOIN Department d ON e.Department_Id = d.Id";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, Employee>
            (query, (e, p, d) =>
            {
                e.Passport = p;
                e.Department = d;
                return new Employee()
                {
                    Department = new Department()
                    {
                        Name = e.Department.Name,
                        Phone = e.Department.Phone
                    },
                    Passport = new Passport()
                    {
                        Type = e.Passport.Type,
                        Number = e.Passport.Number
                    },
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Phone = e.Phone,
                    CompanyId = e.CompanyId,
                };
            }, splitOn: "Id, Id");
            return result.ToList();
        }
    }

    public async Task<int?> Create(CreateEmployeeDto entity)
    {
        using (var connection = _context.CreateConnection())
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var sql =
                        "INSERT INTO Employee (name, surname, phone, company_Id, passport_id, department_id) " +
                        "VALUES (@Name, @Surname, @Phone, @CompanyId, @Passport_id, @Department_Id) " +
                        "RETURNING Id;";

                    var passportId = await _passportRepository.Create(entity.Passport, transaction);
                    if (passportId == null)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    var param = new DynamicParameters();
                    param.Add("Name", entity.Name, DbType.String);
                    param.Add("Surname", entity.Surname, DbType.String);
                    param.Add("Phone", entity.Phone, DbType.String);
                    param.Add("CompanyId", entity.CompanyId, DbType.Int32);
                    param.Add("Passport_Id", passportId, DbType.Int32);
                    param.Add("Department_Id", entity.DepartmentId, DbType.Int32);

                    var id = await transaction.Connection.QuerySingleAsync<int>(sql, param, transaction);

                    transaction.Commit();

                    return id;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<List<Employee>?> GetByDepartmentId(int id)
    {
        var query = @"
            SELECT e.Id, e.Name, e.Surname, e.Phone,e.Company_Id AS CompanyId, e.Department_Id AS DepartmentId, e.Passport_Id AS PassportId,
            p.Id, p.Type, p.Number, d.Id, d.Name, d.Phone FROM Employee e
            INNER JOIN Passport p ON e.Passport_Id = p.Id
            INNER JOIN Department d ON e.Department_Id = d.Id 
            WHERE e.Department_Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, Employee>
            (query, (e, p, d) =>
            {
                e.Passport = p;
                e.Department = d;
                return new Employee()
                {
                    Department = new Department()
                    {
                        Name = e.Department.Name,
                        Phone = e.Department.Phone
                    },
                    Passport = new Passport()
                    {
                        Type = e.Passport.Type,
                        Number = e.Passport.Number
                    },
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Phone = e.Phone,
                    CompanyId = e.CompanyId,
                };
            }, param: new {id});
            return result.ToList();
        }
    }

    public async Task<bool> Delete(int id, int passportId)
    {
        var query = @" 
                DELETE FROM Employee
                WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var result = await transaction.Connection.ExecuteAsync(query, new {id}, transaction);
                    var passportDelete = await _passportRepository.Delete(passportId, transaction);
                    if (!(result > 0 && passportDelete))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }

    public async Task<EmployeeEntity?> GetById(int id)
    {
        var query = @"
            SELECT e.Id, e.Name, e.Surname, e.Phone,e.Company_Id AS CompanyId, e.Department_Id AS DepartmentId, e.Passport_Id AS PassportId,
            p.Id, p.Type, p.Number, d.Id, d.Name, d.Phone FROM Employee e
            INNER JOIN Passport p ON e.Passport_Id = p.Id
            INNER JOIN Department d ON e.Department_Id = d.Id
            WHERE e.Id = @Id";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>
            (query, (e, p, d) =>
            {
                e.Passport = p;
                e.Department = d;
                return e;
            }, param: new {id});
            return result.FirstOrDefault();
        }
    }

    public async Task<List<Employee>?> GetByCompanyId(int id)
    {
        var query = @"
            SELECT e.Id, e.Name, e.Surname, e.Phone,e.Company_Id AS CompanyId, e.Department_Id AS DepartmentId, e.Passport_Id AS PassportId,
            p.Id, p.Type, p.Number, d.Id, d.Name, d.Phone FROM Employee e
            INNER JOIN Passport p ON e.Passport_Id = p.Id
            INNER JOIN Department d ON e.Department_Id = d.Id 
            WHERE e.company_id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, Employee>
            (query, (e, p, d) =>
            {
                e.Passport = p;
                e.Department = d;
                return new Employee()
                {
                    Department = new Department()
                    {
                        Name = e.Department.Name,
                        Phone = e.Department.Phone
                    },
                    Passport = new Passport()
                    {
                        Type = e.Passport.Type,
                        Number = e.Passport.Number
                    },
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Phone = e.Phone,
                    CompanyId = e.CompanyId,
                };
            }, param: new {id});
            return result.ToList();
        }
    }
}