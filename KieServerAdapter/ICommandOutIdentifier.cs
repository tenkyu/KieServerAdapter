using Newtonsoft.Json;

namespace KieServerAdapter
{
    public interface ICommandOutIdentifier
    {
        [JsonProperty("out-identifier")]
        string OutIdentifier { get; set; }
    }

}
