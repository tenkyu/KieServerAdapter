using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class StartProcess : ICommandContainer
    {
        [JsonProperty("start-process")]
        public ICommand Command { get; }

        public StartProcess(string processId)
        {
            Command = new CommandStartProcess {ProcessId = processId };
        }
    }
}
