using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class GetGlobal : ICommandContainer
    {
        [JsonProperty("get-global")]
        public ICommand Command { get; }

        public GetGlobal(string identifier)
        {
            Command = new CommandGetGlobal { Identifier = identifier };
        }
    }
}
