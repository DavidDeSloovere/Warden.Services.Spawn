using System.Threading.Tasks;

namespace Warden.Services.Spawn.Services
{
    public interface IWardenHostService
    {
         Task AddWardenAsync(string userId, IWarden warden);
         Task RemoveWardenAsync(string userId, string name);
         Task StartWardenAsync(string userId, string name);
    }
}