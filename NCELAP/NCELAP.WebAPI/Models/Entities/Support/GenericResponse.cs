using System;
namespace NCELAP.WebAPI.Models.Entities.Support
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
