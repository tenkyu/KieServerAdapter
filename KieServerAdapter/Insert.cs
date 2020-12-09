using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class Insert : ICommandContainer
    {
        [JsonProperty("insert")]
        public ICommand Command { get; }

        public Insert(object commandObject, string objectNameSpace, bool returnObject = true)
        {
            Command = new CommandInsert { 
                CommandObject = new CommandObject(commandObject, objectNameSpace),
                ReturnObject = returnObject };
        }

        public Insert(object commandObject, string objectNameSpace, string outIdentifier, bool returnObject = true)
        {
            Command = new CommandInsert
            {
                CommandObject = new CommandObject(commandObject, objectNameSpace),
                ReturnObject = returnObject
            };
            if (!string.IsNullOrEmpty(outIdentifier))
                ((CommandInsert)Command).OutIdentifier = outIdentifier;
        }

    }
}
