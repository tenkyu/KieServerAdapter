using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class CommandInsert : ICommand
    {
        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = "response";

        [JsonProperty("object")]
        public CommandObject CommandObject { get; set; }

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.Insert;
    }
}
