using CupidonServer.Models;
using CupidonServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace CupidonServer.Hubs
{
    public class CupidonHub : Hub
    {
        private readonly IPersonService _personService;

        public CupidonHub(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task InitSinglePerson(string username, string town, int age, string phoneNum)
        {
            if (_personService.GetByUsername(username) != null)
            {
                await Clients.Caller.SendAsync("Error", "you neeed to choose new username");
                return;
            }

            var osoba = new Person
            {
                Username = username,
                Town = town,
                Age = age,
                PhoneNum = phoneNum,
                ConnectionId = Context.ConnectionId
            };

            _personService.Add(osoba);
            await Clients.Caller.SendAsync("Registered", $"you have registered as {username}!");
            Console.WriteLine($"[SERVER] {username} signed in from {town}, {age} years old");
        }

        public async Task Block(string usernameToBlock)
        {
            var caller = _personService.GetByConnectionId(Context.ConnectionId);
            if (caller == null) return;

            if (_personService.GetByUsername(usernameToBlock) == null)
            {
                await Clients.Caller.SendAsync("Error", $"user {usernameToBlock} is non existent.");
                return;
            }

            _personService.Block(Context.ConnectionId, usernameToBlock);
            await Clients.Caller.SendAsync("Blocked", $"{usernameToBlock} is blocked.");
            Console.WriteLine($"[SERVER] {caller.Username} has blocked {usernameToBlock}");
        }

        public async Task AcceptLetter()
        {
            _personService.SetWaitForResponse(Context.ConnectionId, false);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _personService.RemoveByConnectionId(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
