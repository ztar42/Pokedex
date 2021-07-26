using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Configuration
{
    public class HttpClientConfig
    {
        public RetryPolicyConfiguration RetryPolicy { get; set; }
        public CircuitBreakerPolicyConfiguration CircuitBreakerPolicy { get; set; }
        public Dictionary<string, ClientItemConfig> ClientConfigs { get; set; }
    }

    public class RetryPolicyConfiguration
    {
        public int FirstRetryDelayMs { get; set; }
        public int NumberOfRetries { get; set; }
    }

    public class CircuitBreakerPolicyConfiguration
    {
        public int HandledEventsBeforeBreaking { get; set; }
        public int DurationOfBreakMs { get; set; }
    }
    public class ClientItemConfig
    {
        public string BaseUrl { get; set; }
        public Policy[] Policies { get; set; }
    }
}
