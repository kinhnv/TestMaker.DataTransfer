namespace DataTransfer.Api.Models
{
    public class PostBody<T>
    {
        public T? After { get; set; }

        public T? Before { get; set; }
    }
}
