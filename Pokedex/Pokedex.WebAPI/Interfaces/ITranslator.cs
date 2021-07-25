using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Interfaces
{
    public interface ITranslator<T>
    {
        public Task<T> TranslateAsync(T target);
    }
}
