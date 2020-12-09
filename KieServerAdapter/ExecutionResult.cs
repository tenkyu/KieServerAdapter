using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KieServerAdapter
{
    public class ExecutionResult
    {
        public List<KeyValuePair<string, object>> Results;
        public List<KeyValuePair<string, object>> Facts;

        /// <summary>
        /// indexer for the Results collection
        /// </summary>
        public object this[string resultKey]
        {
            get { return Results.SingleOrDefault(e => e.Key.Equals(resultKey)).Value; }
        }

        /// <summary>
        /// retrieve the object collection resulting from a GetObjects command
        /// </summary>
        /// <param name="outIdentifier">the out-identifier of the get-objects response collection,
        ///   defaults to the default value used by the GetObjects command instance</param>
        public List<KeyValuePair<string, object>> Objects(string outIdentifier = null)
        {
            var outObject = this[String.IsNullOrEmpty(outIdentifier) ? 
                CommandGetObjects.OutIdentifierDefault :
                outIdentifier
                ] as JArray;

            if (outObject == null)
                return null;

            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
            foreach (var j in outObject)
            {
                ExtractObject(j, list);
            }

            return list;
        }

        internal void ExtractObject(JToken j, List<KeyValuePair<string, object>> list)
        {
            if (!j.HasValues)   // simple type, not an object (i.e. string, bool or some kind of number)
            {
                // quote these so the json deserializer can parse and type convert
                list.Add(new KeyValuePair<string, object>(Enum.GetName(typeof(JTokenType), j.Type), "\"" + j.ToString() + "\""));
            }
            else if (j.Type == JTokenType.Array)
            {
                // we won't have a type name so use "List" for a list of things
                list.Add(new KeyValuePair<string, object>("Array", j.ToString()));
            }
            else
            {
                var first = j.First;
                var prop = first as JProperty;
                if (first != null)
                {
                    list.Add(new KeyValuePair<string, object>(prop.Name, prop.Value));
                }
            }
        }

        /// <summary>
        /// Return the collection of all objects of a type contained in the get-objects command response,
        /// with the option to override the JsonConverter, drools(java) classname of the objects,
        /// and the out-identifier of the get-objects response element
        /// </summary>
        /// <typeparam name="T">c-sharp class type to retrieve from the get-objects response;
        /// the class should have a DroolsTypeAttribute or you must specify the className parameter. See remarks.</typeparam>
        /// <param name="jsonConvert">JsonConverter to use while deserializing the objects</param>
        /// <param name="className">drools (java) class name of the object, not required if your
        ///   class has a DroolsTypeAttribute on it to specify the class name</param>
        /// <param name="outIdentifier">the out-identifier of the get-objects response collection,
        ///   defaults to the default value used by the GetObjects command instance</param>
        /// <returns>List of objects of the specified type</returns>
        /// <remarks>You can retrieve simple types by explicitly specifying the JTokenType name (ex.
        /// "String", "Integer", etc. with an appropriate T type String, int, etc.)</remarks>
        public List<T> ObjectsOfType<T>(JsonConverter jsonConvert, string className = null, string outIdentifier = null)
        {
            // if no class name given, see if there is a DroolsType attribute to pull it from
            if (className is null)
            {
                className = typeof(T).GetAttributeValue<DroolsTypeAttribute, string> ( a => a.TypeName );
            }

            if (className is null)
                throw new InvalidOperationException("Unable to determine Drools type name for object type");

            var list = Objects(outIdentifier).FindAll(a => a.Key.Equals(className));

            List<T> results = new List<T>();
            foreach (var j in list)
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(j.Value.ToString(), jsonConvert);
                    if (obj != null)
                        results.Add(obj);
                }
                // eat any exceptions since we really don't know if the list item is one of the objects or not
                catch(Exception e) { }
            }

            return results;
        }

        /// <summary>
        /// Return the collection of all objects of a type contained in the get-objects command response,
        /// with the option to override the drools(java) classname of the objects,
        /// and the out-identifier of the get-objects response element
        /// </summary>
        /// <typeparam name="T">c-sharp class type to retrieve from the get-objects response;
        /// the class should have a DroolsTypeAttribute or you must specify the className parameter</typeparam>
        /// <param name="className">drools (java) class name of the object, not required if your
        ///   class has a DroolsTypeAttribute on it to specify the class name</param>
        /// <param name="outIdentifier">the out-identifier of the get-objects response collection,
        ///   defaults to the default value used by the GetObjects command instance</param>
        /// <returns>List of objects of the specified type</returns>
        public List<T> ObjectsOfType<T>(string className = null, string outIdentifier = null)
        {
            return ObjectsOfType<T>(new UnixTimestampConverter(), className, outIdentifier);
        }

        /// <summary>
        /// return the result instance of a query command
        /// </summary>
        /// <param name="outIdentifier">out-identifier of the query results if a non-default value 
        /// was used in the command</param>
        public FlatQueryResults QueryResult(string outIdentifier = null)
        {
            var qryResult = this[String.IsNullOrEmpty(outIdentifier) ?
                CommandQuery.OutIdentifierDefault : outIdentifier] as JObject;

            if (qryResult == null)
                return null;

            return FlatQueryResults.FromJson(qryResult);
        }


    }
}
