using Newtonsoft.Json;

namespace KieServerAdapter
{
    /// <summary>
    /// Get all objects in drools memory
    /// </summary>
    /// <remarks>
    /// Note Kie server doesn't appear to implement the object-filter over REST,
    /// so it has been removed from this class.  Use CommandQuery instead and define
    /// a query in Drools to return the objects you want.
    /// </remarks>
    public class CommandGetObjects : ICommand, ICommandOutIdentifier
    {
        public const string OutIdentifierDefault = "getObjectsResults";

        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = OutIdentifierDefault;

        [JsonIgnore]
        public KieCommandTypeEnum CommandType { get; } = KieCommandTypeEnum.GetObjects;
    }
}
