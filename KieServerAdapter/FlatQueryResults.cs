using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;


namespace KieServerAdapter
{
    #region Internal classes used to deserialize the query results JSON structures
    internal class DroolsMapElement
    {
        public string Key;
        public CommandObject Value;
    }
    internal class DroolsMap
    {
        [JsonProperty("element")]
        internal List<DroolsMapElement> Objects;
    }
    internal class DroolsListMap
    {
        [JsonProperty("element")]
        internal List<DroolsMap> Maps;
    }

    internal class DroolsSet
    {
        [JsonProperty("element")]
        internal List<string> Items;
    }
    #endregion

    [DroolsType("org.drools.core.runtime.rule.impl.FlatQueryResults")]
    public class FlatQueryResults
    {
        [JsonProperty("idFactHandleMaps")]
        internal DroolsListMap IdFactHandleMaps;

        [JsonProperty("idResultMaps")]
        internal DroolsListMap IdResultMaps;
        
        [JsonProperty("identifiers")]
        internal DroolsSet Identifiers;

        /// <summary>
        /// retrieve the objects collection from the query result maps
        /// </summary>
        public List<KeyValuePair<string, object>> Objects
        {
            get
            {
                var list = new List<KeyValuePair<string, object>>();

                foreach (var map in IdResultMaps.Maps)
                {
                    foreach (var obj in map.Objects)
                    {
                        list.Add(new KeyValuePair<string, object>(obj.Key, obj.Value));
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Return the collection of all objects of a type contained in the query results response,
        /// with the option to override the JsonConverter, drools(java) classname of the objects,
        /// and to specify a particular object identifier in the response to select
        /// </summary>
        /// <typeparam name="T">c-sharp class type to retrieve from the get-objects response;
        /// the class should have a DroolsTypeAttribute or you must specify the className parameter</typeparam>
        /// <param name="jsonConvert">JsonConverter to use while deserializing the objects</param>
        /// <param name="className">drools (java) class name of the object, not required if your
        ///   class has a DroolsTypeAttribute on it to specify the class name</param>
        /// <param name="identifier">the identifier key in the results map if you only want one
        /// specific instance</param>
        /// <returns>List of objects of the specified type</returns>
        public List<T> ObjectsOfType<T>(JsonConverter jsonConvert, string className = null, string identifier = null)
        {
            // if no class name given, see if there is a DroolsType attribute to pull it from
            if (String.IsNullOrEmpty(className))
            {
                className = typeof(T).GetAttributeValue<DroolsTypeAttribute, string>(a => a.TypeName);
            }

            if (String.IsNullOrEmpty(className))
                throw new InvalidOperationException("Unable to determine Drools type name for object type");

            List<KeyValuePair<string, object>> list = Objects;
            if (!String.IsNullOrEmpty(identifier))
            {
                list = list.FindAll(a => a.Key.Equals(identifier));
            }

            var items = list.FindAll(a => (a.Value as CommandObject).ObjectNameSpace.Equals(className));

            List<T> results = new List<T>();
            foreach (var j in items)
            {
                var obj = JsonConvert.DeserializeObject<T>((j.Value as CommandObject).CommandItem.ToString(), jsonConvert);
                if (obj != null)
                    results.Add(obj);
            }

            return results;
        }

        /// <summary>
        /// Return the collection of all objects of a type contained in the query results response,
        /// with the option to override the drools(java) classname of the objects,
        /// and to specify a particular object identifier in the response to select
        /// </summary>
        /// <typeparam name="T">c-sharp class type to retrieve from the get-objects response;
        /// the class should have a DroolsTypeAttribute or you must specify the className parameter</typeparam>
        /// <param name="className">drools (java) class name of the object, not required if your
        ///   class has a DroolsTypeAttribute on it to specify the class name</param>
        /// <param name="identifier">the identifier key in the results map if you only want one
        /// specific instance</param>
        /// <returns>List of objects of the specified type</returns>
        public List<T> ObjectsOfType<T>(string className = null, string identifier = null)
        {
            return ObjectsOfType<T>(new UnixTimestampConverter(), className, identifier);
        }

        /// <summary>
        /// Factory method that given a JToken of the node containing the query results, 
        /// deserializes it into a FlatQueryResults instance.
        /// </summary>
        public static FlatQueryResults FromJson(JToken json)
        {
            var jsonConverter = new UnixTimestampConverter();

            var serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = {  jsonConverter,
                                new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" } 
                },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var jsonProp = json as JObject;
            var result = jsonProp.First.First.ToObject<FlatQueryResults>(serializer);

            return result;
        }
    }
}
