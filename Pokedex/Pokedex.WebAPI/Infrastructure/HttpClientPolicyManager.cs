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
    public sealed class HttpClientPolicyManager
    {
        private readonly IAsyncPolicy<HttpResponseMessage> exponentialRetryPolicy;
        private readonly IAsyncPolicy<HttpResponseMessage> circutBreakerPolicy;
        private readonly Dictionary<Configuration.Policy, IAsyncPolicy<HttpResponseMessage>> policyMap;
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

            //configure circuitbreaker policy
            var eventsBeforeBreaking = configuration.CircuitBreakerPolicy.HandledEventsBeforeBreaking;
            var delayOfBreak = TimeSpan.FromMilliseconds(configuration.CircuitBreakerPolicy.HandledEventsBeforeBreaking);
            this.circutBreakerPolicy = Policy<HttpResponseMessage>
                .HandleResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .CircuitBreakerAsync(eventsBeforeBreaking, delayOfBreak);

            policyMap = new Dictionary<Configuration.Policy, IAsyncPolicy<HttpResponseMessage>>()
            {
                [Configuration.Policy.Retry] = exponentialRetryPolicy,
                [Configuration.Policy.CircuitBreaker] = circutBreakerPolicy
            };
        }

        public void ApplyPolicies(IHttpClientBuilder builder, params Configuration.Policy[] policies)
        {
            if (policies == null)
                return;
            foreach (var policy in policies)
                builder.AddPolicyHandler(x => policyMap[policy]);
        }
    }
}
