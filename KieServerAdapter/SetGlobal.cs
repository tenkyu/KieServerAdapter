using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class SetGlobal : ICommandContainer
    {
        [JsonProperty("set-global")]
        public ICommand Command { get; }

        public SetGlobal(string identifier, object commandObject, string objectNameSpace)
        {
            Command = new CommandSetGlobal { Identifier = identifier, CommandObject = new CommandObject(commandObject, objectNameSpace) };
        }
    }
}
