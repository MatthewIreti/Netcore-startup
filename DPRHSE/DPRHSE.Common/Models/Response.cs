namespace DPRHSE.Common.Models
{

    public enum ResultType
    {
        Success = 1,
        Error = 2,
        ValidationError = 3,
        Warning = 4,
        Empty
    }
    public class Response<T> : Response where T: class
    {
        
        public new T Data
        {
            get => base.Data as T;

            set => base.Data = value;
        }
        public static Response<T> Success(T result)
        {
            var response = new Response<T> { ResultType = ResultType.Success, Data = result};

            return response;
        } 
        public static Response<T> Success(T result, int totalCount)
        {
            var response = new Response<T> { ResultType = ResultType.Success, Data = result, TotalCount=totalCount};

            return response;
        }


        public new static Response<T> Failed(string errorMessage)
        {
            var response = new Response<T> { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        public new static Response<T> Empty()
        {
            var response = new Response<T> { ResultType = ResultType.Empty };

            return response;
        }

        public static Response<T> TokenFailure()
        {
            var response = new Response<T> { ResultType = ResultType.Error , Message = "Token retrival error" };

            return response;
        }
    }
    public class Response
    {

        public bool Status => ResultType == ResultType.Success || ResultType == ResultType.Warning;
        public object Data { get; protected set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }

        public ResultType ResultType { get; set; }

        public Response()
        {
            ResultType = ResultType.Success;
        }

        public static Response Success()
        {
            var response = new Response { ResultType = ResultType.Success };

            return response;
        }

        public static Response Failed(string errorMessage)
        {
            var response = new Response { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        public static Response Empty()
        {
            var response = new Response { ResultType = ResultType.Empty };

            return response;
        }

    }
}
