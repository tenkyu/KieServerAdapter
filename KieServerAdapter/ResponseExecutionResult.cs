using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class ResponseExecutionResult
    {
        [JsonProperty("execution-results")]
        public ExecutionResult ExecutionResults { get; set; }
    }
}
