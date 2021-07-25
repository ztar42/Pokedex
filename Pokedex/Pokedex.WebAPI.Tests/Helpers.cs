using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokedex.WebAPI.Entities;
using System;
using System.Linq;
using System.Reflection;

namespace Pokedex.WebAPI.Tests
{
    public static class Helpers
    {
        public static IHost CreateHostForControllers(params Type[] controllers)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHost(host => host.UseStartup<Startup>().UseTestServer())
                .Build();
        }
        public static Pokemon GetRandomPokemon()
        {
            Faker faker = new Faker();
            return new Pokemon()
            {
                Name = faker.Random.String(10),
                Description = faker.Lorem.Paragraph(),
                Habitat = faker.Random.String(10),
                IsLegendary = faker.Random.Bool()
            };
        }
    }
}
