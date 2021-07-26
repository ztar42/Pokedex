using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.WebAPI.Configuration;
using Pokedex.WebAPI.Infrastructure;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.WebAPI
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder builder, HttpClientPolicyManager policyManager)
        {
            policyManager.ApplyCircuitBreakerPolicy(builder);
            return builder;
        }
        public static IHttpClientBuilder AddRetryAndCircuitBreakerPolicies(this IHttpClientBuilder builder, HttpClientPolicyManager policyManager)
        {
            policyManager.ApplyRetryPolicy(builder);
            policyManager.ApplyCircuitBreakerPolicy(builder);
            return builder;
        }
            
        public static IHttpClientBuilder AddConfiguredHttpClient<T, U>(this IServiceCollection serviceCollection, HttpClientConfig configuration)
            where T : class
            where U : class, T
        {
            var baseUrlExists = configuration.BaseUrls.TryGetValue(typeof(U).Name, out string baseUrl);
            return serviceCollection.AddHttpClient<T, U>(baseUrlExists ? x => x.BaseAddress = new Uri(baseUrl) : x => { });
        }

        public static IHttpClientBuilder AddConfiguredHttpClient<T>(this IServiceCollection serviceCollection, HttpClientConfig configuration)
            where T : class
        {
            var baseUrlExists = configuration.BaseUrls.TryGetValue(typeof(T).Name, out string baseUrl);
            return serviceCollection.AddHttpClient<T>(baseUrlExists ? x => x.BaseAddress = new Uri(baseUrl) : x => { });
        }
    }
}
