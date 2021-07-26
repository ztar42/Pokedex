using System.Threading.Tasks;

namespace Pokedex.WebAPI.Interfaces
{
    public interface ITranslator<T>
    {
        public Task<T> TranslateAsync(T target);
    }
}
