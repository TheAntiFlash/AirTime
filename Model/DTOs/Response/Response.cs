namespace Model.DTOs.Response;

public class Response<T>
{
    public class Success : Response<T>
    {
        public T Data { get; set; }

        public Success(T data)
        {
            this.Data = data;
        }
    }

    public class Failure : Response<T>
    {
        public Exception? E { get; set; }

        public Failure(Exception e)
        {
            this.E = e;
        }
    }
}