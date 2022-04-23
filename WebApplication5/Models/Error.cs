using Newtonsoft.Json;

namespace WebApplication5.Models
{
    public class Error
    {
        public bool Success { get; set; }
        public string Data { get; set; }
        public int StatusCode{ get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
