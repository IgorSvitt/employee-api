namespace EmployeeApi.Domain.Response;

public enum StatusCode
{
    NotFound = 404,
    OK = 200,
    InternalServerError = 500,
    BadRequest = 400,
    NoContent = 204
}