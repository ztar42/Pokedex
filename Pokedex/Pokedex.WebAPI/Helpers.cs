using Microsoft.Extensions.DependencyInjection;
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
    public static class Helpers
    {
        private static IEnumerable<TimeSpan> exponentialRetrydelay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(100), 3);
        private static readonly IAsyncPolicy<HttpResponseMessage> exponentialRetryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(exponentialRetrydelay);

        private static readonly IAsyncPolicy<HttpResponseMessage> circutBreakerPolicy = Policy<HttpResponseMessage>
            .HandleResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(60));

        public static IHttpClientBuilder AddExponentialRetryPolicy(this IHttpClientBuilder builder) =>
            builder.AddPolicyHandler(x => exponentialRetryPolicy);
        public static IHttpClientBuilder AddCircuitBreakerPolicy(this IHttpClientBuilder builder) =>
            builder.AddPolicyHandler(x => circutBreakerPolicy);
        public static IHttpClientBuilder SetBaseUrl(this IHttpClientBuilder builder, string baseUrl) =>
            builder.ConfigureHttpClient(x => x.BaseAddress = new Uri(baseUrl));
    }
}
