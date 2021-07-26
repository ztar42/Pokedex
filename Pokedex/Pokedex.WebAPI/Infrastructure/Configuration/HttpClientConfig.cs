using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Configuration
{
    public class HttpClientConfig
    {
        public RetryPolicyConfiguration RetryPolicy { get; set; }
        public CircutBreakerPolicyConfiguration CircutBreakerPolicy { get; set; }
        public Dictionary<string,string> BaseUrls { get; set; }
    }

    public class RetryPolicyConfiguration
    {
        public int FirstRetryDelayMs { get; set; }
        public int NumberOfRetries { get; set; }
    }

    public class CircutBreakerPolicyConfiguration
    {
        public int HandledEventsBeforeBreaking { get; set; }
        public int DurationOfBreakMs { get; set; }
    }
}
