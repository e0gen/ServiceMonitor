# ServiceMonitor

Microservice perform periodical check accessibility of all necessary http services and it collect and provide statistics about monitored services.

# Home page

The home page displays all the monitored services for the last 24 hours and provides error statistics for the last hour and day for each service, as well as the last status.

![Home](https://user-images.githubusercontent.com/5530344/77160261-0d6d1200-6ab8-11ea-8766-2619ee77fcd5.png)

# API

Additionally data of the availability of a certain service can be send directly to the service through API

**POST**   `/api/status/send`

```json
{
	"ServiceName": "SomeService",
	"Status": 200,
	"Date": "2020-03-19T18:25:43.511Z"
}
```

To check microservice is up without consuming any resources

**GET**   `/api/status`

Returns `OK(200)` if is service running

## Features 

- Service Checker worker can be deployed independently of web application.

## Built on

* C# 7
* .NET Core 3.1
* ASP.NET Core
* Entity Framework Core
* MS SQL Server

## Extra
* [Skeleton](http://getskeleton.com/) - CSS style

## Authors

* **Evgeny Nevzorov** - [e0gen](https://github.com/e0gen)
