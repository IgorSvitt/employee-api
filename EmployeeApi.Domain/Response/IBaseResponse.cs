
namespace EmployeeApi.Domain.Response;

public class BaseResponse<T>: IOkResponse<T>
{
    
    public StatusCode StatusCode { get; set; }
    
    public T Data { get; set; }
}

public interface IBaseResponse
{
    public StatusCode StatusCode { get; set; }
}

public interface IOkResponse<T>: IBaseResponse
{
    public T Data { get; }
}

public interface IErrorResponse : IBaseResponse
{
    public string Description { get; }
}