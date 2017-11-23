using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class CommandSetGlobal : ICommand, ICommandOutIdentifier, ICommandObject
    {
        public const string OutIdentifierDefault = "outSetGlobal";

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = OutIdentifierDefault;

        [JsonProperty("object")]
        public CommandObject CommandObject { get; set; }

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.SetGlobal;
    }
}
