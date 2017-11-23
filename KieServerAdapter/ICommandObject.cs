using Newtonsoft.Json;

namespace KieServerAdapter
{
    public interface ICommandObject
    {
        [JsonProperty("object")]
        CommandObject CommandObject { get; set; }
    }

}
