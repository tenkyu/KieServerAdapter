using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class CommandFireAllRules : ICommand
    {
        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = "fired";

        [JsonProperty("max")]
        public int Max { get; set; } = -1;

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.FireAllRules;
    }
}
