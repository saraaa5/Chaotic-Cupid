namespace CupidonServer.Services;

using CupidonServer.Models;

    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons = new();

        public void Add(Person person) => _persons.Add(person);

        public List<Person> GetAll() => _persons;

        public Person? GetByUsername(string username) =>
            _persons.FirstOrDefault(p => p.Username == username);

        public Person? GetByConnectionId(string connectionId) =>
            _persons.FirstOrDefault(p => p.ConnectionId == connectionId);

        public void Block(string connectionId, string usernameToBlock)
        {
            var person = GetByConnectionId(connectionId);
            person?.Blocked.Add(usernameToBlock);
        }

        public void SetWaitForResponse(string connectionId, bool value)
        {
            var person = GetByConnectionId(connectionId);
            if (person != null)
                person.WaitForResponse = value;
        }

        public void RemoveByConnectionId(string connectionId)
        {
            _persons.RemoveAll(o => o.ConnectionId == connectionId);
        }
    }
