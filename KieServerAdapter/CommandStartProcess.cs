using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class CommandStartProcess : ICommand
    {
        [JsonProperty("processId")]
        public string ProcessId { get; set; }

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.StartProcess;
    }
}
