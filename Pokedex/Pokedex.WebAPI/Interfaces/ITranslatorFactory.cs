using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Interfaces
{
    public interface ITranslatorFactory<T, U>
    {
        public ITranslator<U> GetTranslator(T target);
    }
}
