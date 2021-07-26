using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pokedex.WebAPI.Entities;
using System;

namespace Pokedex.WebAPI.Tests
{
    public static class Helpers
    {
        public static IConfiguration CreateAppSettingsConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public static IHost CreateTestHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHost(host => host.UseStartup<Startup>().UseTestServer())
                .Build();
        }
        public static Uri GetBaseUrlForClient(this IConfiguration configuration, Type type) =>
            new Uri(configuration[$"HttpClientConfig:ClientConfigs:{type.Name}:BaseUrl"]);
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
