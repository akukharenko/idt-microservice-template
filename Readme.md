# Boss Sample Service - Template

Service short description
TBD

## Description

Full description
TBD

## Environments

* Development (Scrum Master Environment): [link here]
* QA (Scrum RC-QA Environment): [link here]
* UAT (Waiting to implement step on pipeline): [link here]
* Prod (Waiting to implement step on pipeline): [link here]

## Jenkins

TBD

## Build Process for Local Development

* You have Docker installed
* You have .NET 5 installed (SDK and runtime)

## Infrastructure Diagram

TBD

## Wiki Info

The home page for the application details: [link here]

---

## Run the application

To run the service locally it can be used 2 simple solutions:
1. Project Tye
2. Docker Compose

To run application with Tye need to use command:
```
tye run --dashboard
```

After this is will create all needed container for dependencies like Seq, Mongo and etc, run application and open browser with dashboard - http://127.0.0.1:8000/. This page is very helpful to see all the services running, for .NET it provides some metrics. Also here for each service it show the logs from console (stdout aggregated and shown on the page).
Also information like Replicas can show the info about how many instances of service running. Restarts show errors - sometimes application can't start and this field can help to identify it.

To stop the process use *Ctrl+C*. It will automatically stop and remove containers created for the additional services. You don't need to take care of it because Tye is knows all the items started and do cleanup after stop.
Main application runs locally on the current machine without any containers.

More details on he Tye - https://github.com/dotnet/tye

**Note: Tye is dotnet global tool and need to be installed on the local machine.**

To install the tool you can use th next command:
```
dotnet tool install -g Microsoft.Tye --version "0.6.0-alpha.21070.5"
```

With Docker compose need to use command:
```
docker-compose up
```

This command start all services and application inside containers - main .NET application if image is not available will be build by using Dockerfile with instructions.

To stop the containers use *Ctrl+C* and command to remove containers:
```
docker-compose down
```

---
