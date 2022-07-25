namespace Exercise2.Models
{
    public class APIResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
    }
}
