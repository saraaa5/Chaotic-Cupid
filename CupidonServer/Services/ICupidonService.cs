using CupidonServer.Models;

namespace CupidonServer.Services
{
    public interface ICupidonService
    {
        int CalculateScore(Person sender, Person reciver);
        Person? FindBestLover(Person sender);
    }
}
