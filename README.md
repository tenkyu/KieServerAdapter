# .Net KieServerAdapter for ![Logo](Files/DroolsLogo210px.png)

KieServerAdapter is a restful client for Drools KieServer. You can easily call rules with your .Net project. These are the covered functions.

  - [SetGlobalCommand](https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_setglobalcommand)
  - [InsertObjectCommand](https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_insertobjectcommand)
  - [StartProcessCommand](https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_startprocesscommand)
  - [FireAllRulesCommand](https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_fireallrulescommand)
  - [GetGlobalCommand](https://docs.jboss.org/drools/release/latest/drools-docs/html_single/#_getglobalcommand)

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
