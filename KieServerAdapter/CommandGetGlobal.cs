using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class CommandGetGlobal: ICommand
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; }

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.GetGlobal;
    }
}
