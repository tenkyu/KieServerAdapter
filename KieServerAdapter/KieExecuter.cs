using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KieServerAdapter
{
    public class KieExecuter
    {
        [JsonIgnore]
        public string HostUrl { get; set; }

        [JsonIgnore]
        public string InstancesPath { get; set; } = "kie-server/services/rest/server/containers/instances/";

        [JsonIgnore]
        public string AuthUserName { get; set; }

        [JsonIgnore]
        public string AuthPassword { get; set; }

        [JsonProperty("lookup")]
        public string LookUp { get; set; } = "defaultKieSession";

        [JsonProperty("commands")]
        public List<ICommandContainer> Commands { get; private set; } = new List<ICommandContainer>();

        public void AddStartProcess(string processId)
        {
            Commands.Add(new StartProcess(processId));
        }

        public void AddInsert(object commandObject, string objectNameSpace)
        {
            Commands.Add(new Insert(commandObject, objectNameSpace));
        }

        public async Task<ExecutionResponse<object>> ExecuteAsync(string containerName)
        {
            return await ExecuteAsync<object>(containerName);
        }

        public async Task<ExecutionResponse<T>> ExecuteAsync<T>(string containerName)
        {
            var startDate = DateTime.Now;

            Commands = Commands.OrderByDescending(c => c.Command.CommandType).ToList();

            var json = JsonConvert.SerializeObject(this);
            var result = new ExecutionResponse<T>();

            using (var client = new HttpClient { BaseAddress = new Uri(HostUrl) })
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(AuthUserName))
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{AuthUserName}:{AuthPassword}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }

                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Concat(InstancesPath, containerName)))
                {
                    request.Content = new StringContent(json);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsAsync<ExecutionResponse<T>>();
                            result.ResponseBody = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }

            result.RequestBody = json;

            if (typeof(T) != typeof(object))
            {
                var command = Commands.FirstOrDefault(c => c.Command.CommandType == KieCommandTypeEnum.Insert);
                var entity = command?.Command as CommandInsert;

                if (result.Result?.ExecutionResults.Results.Count == 1)
                {
                    if (result.Result.ExecutionResults.Results[0].Key == entity?.OutIdentifier)
                    {
                        var item = (JObject)result.Result.ExecutionResults.Results[0].Value;
                        var first = item.First;

                        if (first is JProperty)
                        {
                            var prop = first as JProperty;

                            if (prop.Name.Equals(entity?.CommandObject.ObjectNameSpace))
                            {
                                result.SmartSingleResponse = JsonConvert.DeserializeObject<T>(prop.Value.ToString(), new UnixTimestampConverter());
                            }
                        }
                    }
                }
            }

            var span = DateTime.Now - startDate;
            result.ElapsedTime = (int)span.TotalMilliseconds;

            return result;
        }
    }
}
