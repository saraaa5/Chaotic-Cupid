using CupidonServer.Models;

namespace CupidonServer.Services
{
    public interface IPersonService
    {
        void Add(Person person);
        List<Person> GetAll();
        Person? GetByUsername(string username);
        Person? GetByConnectionId(string connectionId);
        void Block(string connectionId, string usernameToBlock);
        void SetWaitForResponse(string connectionId, bool value);
        void RemoveByConnectionId(string connectionId);
    }
}
