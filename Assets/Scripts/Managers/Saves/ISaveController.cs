using System.Threading.Tasks;

namespace Unbowed.Managers.Saves
{
    public interface ISaveController
    {
        public Task<T> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T value);
    }
}