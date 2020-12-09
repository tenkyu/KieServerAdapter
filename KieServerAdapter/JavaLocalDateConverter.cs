using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KieServerAdapter
{
    /// <summary>
    /// Json converter for the Java LocalDate value returned by Drools
    /// (an array of 3 integers, year, month and day)
    /// </summary>
    public class JavaLocalDateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime?) || objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            if (arr.Count != 3)
                throw new InvalidOperationException("Invalid LocalDate array: year, month and day required.");

            return new DateTime((int) arr[0], (int) arr[1], (int) arr[2]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = (DateTime) value;
            writer.WriteRawValue("[" + dt.Year + ", " + dt.Month + ", " + dt.Day + "]");
        }
    }
}
