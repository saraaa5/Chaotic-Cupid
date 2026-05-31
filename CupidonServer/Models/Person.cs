namespace CupidonServer.Models
{
    public class Person
    {
        public string Username { get; set; }
        public string Town { get; set; }
        public int Age { get; set; }
        public string PhoneNum { get; set; }
        public string ConnectionId { get; set; }
        public bool WaitForResponse { get; set; }
        public HashSet<string> Blocked { get; set; }
    }
}
