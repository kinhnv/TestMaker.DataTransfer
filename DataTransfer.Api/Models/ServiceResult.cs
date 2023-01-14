namespace DataTransfer.Api.Models
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }

        public List<string>? Error { get; set; }

        public int? Code { get; set; }
    }
}
