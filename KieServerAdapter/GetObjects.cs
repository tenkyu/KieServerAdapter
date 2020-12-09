using System;
using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class GetObjects : ICommandContainer
    {
        [JsonProperty("get-objects")]
        public ICommand Command { get; }

        public GetObjects(string outIdentifier = null)
        {
            Command = new CommandGetObjects()
            {
                OutIdentifier = String.IsNullOrEmpty(outIdentifier) ?
                    CommandGetObjects.OutIdentifierDefault :
                    outIdentifier
            };
        }
    }
}
