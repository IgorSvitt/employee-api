namespace EmployeeApi.Domain.Response;

public class ErrorResponse : IErrorResponse
{
    public string Description { get; set; }

    public StatusCode StatusCode { get; set; }
}