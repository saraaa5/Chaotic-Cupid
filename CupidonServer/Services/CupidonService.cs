using CupidonServer.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using CupidonServer.Hubs;

namespace CupidonServer.Services
{
    public class CupidonService : BackgroundService, ICupidonService
    {
        private readonly IPersonService _personService;
        private readonly IHubContext<CupidonHub> _hubContext;

        private static readonly string[] Messages =
        [
        "Radujem se našem susretu!",
        "Želim da se upoznamo.",
        "Nisam zainteresovan/a za upoznavanje."
        ];

        public CupidonService(IPersonService personService, IHubContext<CupidonHub> hubContext)
        {
            _personService = personService;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), ct);
                await SendLoveLetters();
            }
        }

        private async Task SendLoveLetters()
        {
            var all = _personService.GetAll();

            foreach (var reciver in all)
            {
                // he/she needs to response to previous one
                if (reciver.WaitForResponse) continue;

                var person = FindBestLover(reciver);
                if (person == null) continue;

                // generate random message
                var message = Messages[RandomNumberGenerator.GetInt32(Messages.Length)];
                var showPhoneNum = message != "Nisam zainteresovan/a za upoznavanje.";

                _personService.SetWaitForResponse(reciver.ConnectionId, true);

                await _hubContext.Clients.Client(reciver.ConnectionId)
                    .SendAsync("LetterArived", new
                    {
                        person.Username,
                        person.Town,
                        person.Age,
                        Phone = showPhoneNum ? person.PhoneNum : "hidden",
                        Message = message
                    });
            }
        }

        public int CalculateScore(Person sender, Person reciver)
        {
            int score = 0;

            if (sender.Town == reciver.Town)
                score += 30;
            if (Math.Abs(sender.Age - reciver.Age) <= 2)
                score += 20;

            score += RandomNumberGenerator.GetInt32(0, 101);

            return score;
        }

        public Person? FindBestLover(Person reciver)
        {
            return _personService.GetAll()
                .Where(o => o.Username != reciver.Username
                         && !reciver.Blocked.Contains(o.Username))
                .OrderByDescending(o => CalculateScore(o, reciver))
                .FirstOrDefault();
        }
    }
}
