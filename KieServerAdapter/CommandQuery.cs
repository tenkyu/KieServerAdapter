using Newtonsoft.Json;
using System.Collections.Generic;

namespace KieServerAdapter
{
    public class CommandQuery : ICommand, ICommandOutIdentifier
    {
        public const string OutIdentifierDefault = "queryResults";

        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = OutIdentifierDefault;

        [JsonProperty("arguments")]
        public IEnumerable<object> Arguments { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.Query;
    }
}
