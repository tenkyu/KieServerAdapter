# .Net KieServerAdapter for ![Logo](Files/DroolsLogo210px.png)

KieServerAdapter is a restful client for Drools KieServer. You can easily call rules with your .Net project. 
These are the covered functions, documented here (https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#runtime-commands-samples-ref_kie-apis).

  - `SetGlobalCommand`
  - `InsertObjectCommand`
  - `StartProcessCommand`
  - `FireAllRulesCommand`
  - `GetGlobalCommand`
  - `GetObjectsCommand`
  - `QueryCommand`

Drools also open source but it is in Java Stack and Kie Server is talented execution server and has restful feautures please see the full documantation in  https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_kie.ksrestapi

### Installation

Install just call this line from your package manager console or see the instractions from https://www.nuget.org/packages/KieServerAdapter/

```javascript
PM> Install-Package KieServerAdapter
```

### Docker
You can find the Docker images and how to use them for last final version at

- [Drools Workbench Showcase](https://registry.hub.docker.com/u/jboss/drools-workbench-showcase/)
- [KIE Execution Server Showcase](https://registry.hub.docker.com/u/jboss/kie-server-showcase/)

For more info about the Drools Docker images see this [blog post](https://downloads.jboss.org/drools/release/snapshot/master/index.html).

### Quick Start

```csharp
//Your insert object with .Net class
var insertObject = new Human { Sex = "E", Age = 63, FullName = "Recep Tayyip Erdoğan", Country = "TR"};

//Initialize your excecuter
var executer = new KieExecuter { HostUrl = "http://localhost:8082", AuthUserName = "kieserver", AuthPassword = "kieserver1!" };

//Insert object to KieServer session
executer.Insert(insertObject, "com.project.Human");

//executer.StartProcess("project.Flow_Human");

//Fire
executer.FireAllRules();

//This your response
var response = await executer.ExecuteAsync("container");

// If your response has an single result, see the magic in SmartSingleResponse property.
//var response = await executer.ExecuteAsync<Human>("container");
```
Please see the KieServerAdapter.Test project for more detailed examples.

### Getting fact data to and from KIE server
There a several different options retrieving results from Drools via the KIE server:
* Insert fact models, have Drools modify the inserted facts, and retrieve the facts by their out-identifier after the rules fire.  That's how the example above works.
* Insert fact models, have Drools create new facts from the rules, then use the GetObjects command to pull down _all_ of the final facts in Drools memory.
* Insert fact models, have Drools create new facts from the rules, then use the Query command with DRL queries to retrieve specific objects from Drools memory.  This option makes sense if your rules are creating a lot of memory objects.

You could be using several of these options at once, even in the same batch command list.  But notice only the last two
options let you retrieve new facts created by the rules flow (either use GetObjects to get them all or Query commands to get
one or more specific new facts.)

A `DroolsTypeAttribute` attribute can be used to identify the Java class name Drools
will return for a given C# object model.  Use the `InsertType` methods instead of the `Insert` 
methods to insert objects tagged with this attribute, which also allow you to set an explicit out-identifier to make later retrieval by out-identifier from the results easier.

Particularly if you use GetObjects or Query command, you should definitely add the `DroolsTypeAttribute` to your data model classes and use the `InsertType` methods to make using
methods like `ObjectsOfType<T>` easier to use:

```csharp
[DroolsType("com.mycompany.mymodule.AsOfDate")]
public class AsOfDate
{
    [JsonConverter(typeof(JavaLocalDateConverter))] 
    public DateTime Date { get; set; }
}

// ...later in the code, firing the rules and retrieving all objects
executer.FireAllRules();
executer.GetObjects();
var response = await executer.ExecuteAsync("MyContainer");
var asOfDates = response.Result.ExecutionResults.ObjectsOfType<AsOfDate>();

// or alternately query for a specific object type result using a Query
executer.FireAllRules();
executer.Query("GetAsOfDateInstance", "resultAsOf");
var response = await executer.ExecuteAsync("MyContainer");
var qryResult = response.Result.ExecutionResults.QueryResult("resultAsOf");
var asOfDates = qryResult.ObjectsOfType<AsOfDate>();
```

Simple types can also be inserted and retrieved from results, and arrays created 
by Drools (usally by creating an ArrayList instance) can be retrieved as well:
```csharp
executer.Insert("my favorite string", "java.lang.String", true);
executer.Insert(4593, "java.lang.Integer", true);

//... and later looking at the response
var strs = response.Result.ExecutionResults.ObjectsOfType<String>("String");           
var ints = response.Result.ExecutionResults.ObjectsOfType<int>("Integer");
var lists = response.Result.ExecutionResults.ObjectsOfType<List<string>>("Array");
```
