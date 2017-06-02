using System.ComponentModel;
using Newtonsoft.Json;

namespace KieServerAdapter
{
    public class FriendlyExecutionResponse
    {
        [Description("Kie server response type")]
        public string Type { get; set; }

        [Description("Kie server response message")]
        public string Msg { get; set; }

        [JsonIgnore]
        [Description("Raw response body content in JSON format")]
        public string ResponseBody { get; set; }

        [JsonIgnore]
        [Description("Raw request body content in JSON format")]
        public string RequestBody { get; set; }

        [JsonIgnore]
        [Description("Kie server execution time as miliseconds")]
        public int ElapsedTime { get; set; }

        [JsonIgnore]
        [Description("Kie server response type with Enum")]
        public TypeEnum ResponseType
        {
            get
            {
                switch (Type)
                {
                    case "SUCCESS":
                        return TypeEnum.Success;
                    case "FAILURE":
                        return TypeEnum.Failure;
                }

                return TypeEnum.Unassigned;
            }
        }
    }

    public enum TypeEnum
    {
        Unassigned,
        Success,
        Failure
    }
}
