using Newtonsoft.Json;

namespace KieServerAdapter
{
    [JsonConverter(typeof(CommandObjectSerializer))]
    public class CommandObject
    {
        public CommandObject(object commandItem, string objectNameSpace)
        {
            CommandItem = commandItem;
            ObjectNameSpace = objectNameSpace;
        }

        public object CommandItem { get; set; }

        internal string ObjectNameSpace { get; set; }
    }
}
