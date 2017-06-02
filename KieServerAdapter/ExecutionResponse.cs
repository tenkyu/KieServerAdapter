using System.ComponentModel;
using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class ExecutionResponse<T> : FriendlyExecutionResponse
    {
        public ResponseExecutionResult Result { get; set; }

        [JsonIgnore]
        [Description("If you set T type and your response has a single result, this property automaticly parsed this property")]
        public T SmartSingleResponse { get; set; }

        [JsonIgnore]
        [Description("Friendly execution response")]
        public FriendlyExecutionResponse FriendlyResponse => this.ForceType<FriendlyExecutionResponse>();
    }
}
