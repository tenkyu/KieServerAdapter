using System;
using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class UnixTimestampConverter : JsonConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime?) || objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return Epoch.AddMilliseconds((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - Epoch).TotalMilliseconds + "000");
        }
    }
}
