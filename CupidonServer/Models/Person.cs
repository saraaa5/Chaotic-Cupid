namespace CupidonServer.Models
{
    public class Person
    {
        public string Username { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PhoneNum { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public bool WaitForResponse { get; set; } = false;
        public HashSet<string> Blocked { get; set; } = new HashSet<string>();
    }
}
