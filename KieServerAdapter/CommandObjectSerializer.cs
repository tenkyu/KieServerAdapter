using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace KieServerAdapter
{
    //http://blog.maskalik.com/asp-net/json-net-implement-custom-serialization/
    public class CommandObjectSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as CommandObject;
            writer.WriteStartObject();
            if (obj != null)
            {
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializer.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });
                writer.WritePropertyName(obj.ObjectNameSpace);
                serializer.Serialize(writer, obj.CommandItem);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                var prop = token.First as JProperty;
                return new CommandObject((prop as JProperty).Value, (prop as JProperty).Name);
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(CommandObject).IsAssignableFrom(objectType);
        }
    }
}
