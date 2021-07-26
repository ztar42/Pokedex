using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Services;
using Pokedex.WebAPI.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IStore<Pokemon> pokemonStore;
        private readonly ILogger<PokemonController> logger;

        public PokemonController(ILogger<PokemonController> logger, 
            IStore<Pokemon> pokemonStore)
        {
            this.logger = logger;
            this.pokemonStore = pokemonStore;
        }

        /// <summary>
        /// Provides pokemon description by the name
        /// </summary>
        /// <param name="name">Pokemon name</param>
        /// <returns>Pokemon description</returns>
        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pokemon))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string name)
        {
            var pokemon = await pokemonStore.GetByNameAsync(name);
            if (pokemon == null)
            {
                logger.LogInformation("Pokemon {@name} has not been found", name);
                return NotFound();
            }
            logger.LogInformation("Pokemon {@name} has been found: {@pokemon}", name, pokemon);
            return Ok(pokemon);
        }      
    }
}
