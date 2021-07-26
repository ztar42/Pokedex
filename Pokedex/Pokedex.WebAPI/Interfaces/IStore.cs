using System.Threading.Tasks;

namespace Pokedex.WebAPI.Interfaces
{
    public interface IStore<T>
    {
        public Task<T> GetByNameAsync(string name);
    }
}
