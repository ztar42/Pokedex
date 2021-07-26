using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.WebAPI.Configuration;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Infrastructure
{
    public class HttpClientPolicyManager
    {
        private readonly IAsyncPolicy<HttpResponseMessage> exponentialRetryPolicy;
        private readonly IAsyncPolicy<HttpResponseMessage> circutBreakerPolicy;
        public HttpClientPolicyManager(HttpClientConfig configuration)
        {
            //configure retry policy
            var firstRetryDelay = TimeSpan.FromMilliseconds(configuration.RetryPolicy.FirstRetryDelayMs);
            var retryCount = configuration.RetryPolicy.NumberOfRetries;
            var jitteredDelayGenerator = Backoff.DecorrelatedJitterBackoffV2(firstRetryDelay, retryCount);
            this.exponentialRetryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrTransientHttpStatusCode()
                .WaitAndRetryAsync(jitteredDelayGenerator);

            //configure circutbreaker policy
            var eventsBeforeBreaking = configuration.CircutBreakerPolicy.HandledEventsBeforeBreaking;
            var delayOfBreak = TimeSpan.FromMilliseconds(configuration.CircutBreakerPolicy.HandledEventsBeforeBreaking);
            this.circutBreakerPolicy = Policy<HttpResponseMessage>
                .HandleResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .CircuitBreakerAsync(eventsBeforeBreaking, delayOfBreak);
        }

        public void ApplyRetryPolicy(IHttpClientBuilder builder) => builder.AddPolicyHandler(x => exponentialRetryPolicy);
        public void ApplyCircuitBreakerPolicy(IHttpClientBuilder builder) => builder.AddPolicyHandler(x => circutBreakerPolicy);
    }
}
