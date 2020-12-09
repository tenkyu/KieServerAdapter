using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KieServerAdapter
{
    public class Query : ICommandContainer
    {
        [JsonProperty("query")]
        public ICommand Command { get; }

        public Query(string identifier, string outIdentifier = null, IEnumerable<object> arguments = null)
        {
            Command = new CommandQuery
            {
                Name = identifier,
                Arguments = arguments,
                OutIdentifier = String.IsNullOrEmpty(outIdentifier) ?
                CommandQuery.OutIdentifierDefault :
                outIdentifier
            };
        }
    }
}
