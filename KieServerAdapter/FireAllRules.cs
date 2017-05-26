using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class FireAllRules : ICommandContainer
    {
        [JsonProperty("fire-all-rules")]
        public ICommand Command { get; }

        public FireAllRules(int max)
        {
            Command = new CommandFireAllRules { Max = max };
        }
    }
}
