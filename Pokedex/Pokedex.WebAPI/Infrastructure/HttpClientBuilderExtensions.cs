using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.WebAPI.Configuration;
using Pokedex.WebAPI.Infrastructure;
using System;
using System.Linq;

namespace Pokedex.WebAPI
{
    public static class HttpClientBuilderExtensions
    {
        private static T GetInstance<T>(this IServiceCollection serviceCollection) where T : class => 
            serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(T))?.ImplementationInstance as T;
        private static void ApplyPolicies(IServiceCollection serviceCollection, IHttpClientBuilder builder, Policy[] policies)
        {
            var policyManager = serviceCollection.GetInstance<HttpClientPolicyManager>();
            if (policyManager != null && policies != null)
                policyManager.ApplyPolicies(builder);
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(HttpClientConfig)).Get<HttpClientConfig>();
            serviceCollection.AddSingleton<HttpClientConfig>(config);
            serviceCollection.AddSingleton<HttpClientPolicyManager>(new HttpClientPolicyManager(config));
            return serviceCollection;
        }
            
        public static IHttpClientBuilder AddConfiguredHttpClient<T, U>(this IServiceCollection serviceCollection)
            where T : class
            where U : class, T
        {
            var configuration = serviceCollection.GetInstance<HttpClientConfig>();
            var configExists = configuration.ClientConfigs.TryGetValue(typeof(U).Name, out ClientItemConfig config);
            if (!configExists)
                return serviceCollection.AddHttpClient<T, U>();

            var builder = serviceCollection.AddHttpClient<T, U>(x => x.BaseAddress = new Uri(config.BaseUrl));
            ApplyPolicies(serviceCollection, builder, config.Policies);
            return builder;
        }

        public static IHttpClientBuilder AddConfiguredHttpClient<T>(this IServiceCollection serviceCollection)
            where T : class
        {
            var configuration = serviceCollection.GetInstance<HttpClientConfig>();
            var configExists = configuration.ClientConfigs.TryGetValue(typeof(T).Name, out ClientItemConfig config);
            if (!configExists)
                return serviceCollection.AddHttpClient<T>();

            var builder = serviceCollection.AddHttpClient<T>(x => x.BaseAddress = new Uri(config.BaseUrl));
            ApplyPolicies(serviceCollection, builder, config.Policies);
            return builder;
        }
    }
}
