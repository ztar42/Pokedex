namespace Pokedex.WebAPI.Interfaces
{
    public interface ITranslatorFactory<T, U>
    {
        public ITranslator<U> GetTranslator(T target);
    }
}
