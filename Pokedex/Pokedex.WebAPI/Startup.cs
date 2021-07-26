using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokedex.WebAPI.Configuration;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.HealthChecks;
using Pokedex.WebAPI.Infrastructure;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Services;
using Pokedex.WebAPI.Translators;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Pokedex.WebAPI
{
    public class Startup
    {      
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var httpClientConfig = Configuration.GetSection(nameof(HttpClientConfig)).Get<HttpClientConfig>();
            var httpClientPolicyManager = new HttpClientPolicyManager(httpClientConfig);

            services.AddHealthChecks()
                .AddCheck("StartupCheck", () => HealthCheckResult.Healthy(), tags: new [] {"startup"})
                .AddCheck<PokemonStoreHealthCheck>(nameof(PokemonStoreHealthCheck), tags: new[] { "liveness" });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokedex.WebAPI", Version = "v1" });
            });

            services.AddSingleton<ITranslatorFactory<Pokemon, string>, DefaultTranslatorFactory>();
            services.AddScoped<ITranslator<Pokemon>, PokemonDescriptionTranslator>();
            services.AddConfiguredHttpClient<YodaStringTranslator>(httpClientConfig).AddRetryPolicy(httpClientPolicyManager);
            services.AddConfiguredHttpClient<ShakespeareStringTranslator>(httpClientConfig).AddRetryPolicy(httpClientPolicyManager);
            services.AddConfiguredHttpClient<IStore<Pokemon>, PokeAPIPokemonStore>(httpClientConfig).AddRetryAndCircuitBreakerPolicies(httpClientPolicyManager);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex.WebAPI v1"));
            }

            app.UseHealthChecks("/hc/startup", new HealthCheckOptions() { Predicate = x => x.Tags.Contains("startup") });
            app.UseHealthChecks("/hc/live", new HealthCheckOptions() { Predicate = x => x.Tags.Contains("liveness") });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
