using System.ComponentModel;
using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class ExecutionResponse<T>
    {
        [Description("Kie server response type")]
        public string Type { get; set; }

        [Description("Kie server response message")]
        public string Msg { get; set; }

        [JsonIgnore]
        [Description("Raw response body content in JSON format")]
        public string ResponseBody { get; set; }

        [JsonIgnore]
        [Description("Raw request body content in JSON format")]
        public string RequestBody { get; set; }
        public ResponseExecutionResult Result { get; set; }

        [JsonIgnore]
        [Description("If you set T type and your response has a single result, this property automaticly parsed this property")]
        public T SmartSingleResponse { get; set; }

        [JsonIgnore]
        [Description("Kie server execution time as miliseconds")]
        public int ElapsedTime { get; set; }
    }
}
