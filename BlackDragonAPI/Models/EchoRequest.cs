using System.Net.Security;

namespace BlackDragonAPI.Models
{
    public class EchoRequest
    {
        public string Message { get; set; } = "Hello";
        public int Count { get; set; } = 0;
        public bool Truth { get; set; } = false;
    }
}
