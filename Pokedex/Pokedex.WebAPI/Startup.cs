using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Services;
using Pokedex.WebAPI.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokedex.WebAPI", Version = "v1" });
            });

            services.AddSingleton<ITranslatorFactory<Pokemon, string>, DefaultTranslatorFactory>();
            services.AddScoped<ITranslator<Pokemon>, PokemonDescriptionTranslator>();
            services.AddHttpClient<YodaStringTranslator>(client => client.BaseAddress = new Uri(Configuration[$"ExternalUrls:{nameof(YodaStringTranslator)}"]));
            services.AddHttpClient<ShakespeareStringTranslator>(client => client.BaseAddress = new Uri(Configuration[$"ExternalUrls:{nameof(ShakespeareStringTranslator)}"]));         
            services.AddHttpClient<IStore<Pokemon>, PokeAPIPokemonStore>(client => client.BaseAddress = new Uri(Configuration[$"ExternalUrls:{nameof(PokeAPIPokemonStore)}"]));          
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
