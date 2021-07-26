using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.HealthChecks
{
    public class PokemonStoreHealthCheck : IHealthCheck
    {
        private readonly IStore<Pokemon> store;
        public PokemonStoreHealthCheck(IStore<Pokemon> store)
        {
            this.store = store;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Pokemon result;
            try
            {
                result = await store.GetByNameAsync("mewtwo");
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
            return result == null ? HealthCheckResult.Unhealthy() : HealthCheckResult.Healthy();
        }
    }
}
