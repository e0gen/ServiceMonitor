# ServiceMonitor

Web service perform periodical check accessibility of all necessary http services and it collect and provide statistics about monitored services.

# Home page

The home page displays all the monitored services for the last 24 hours and provides error statistics for the last hour and day for each service, as well as the last status.

# API

Data on the availability of a certain service can be transferred to the service through API **POST** request

```
/api/status/send
```

```json
{
	"ServiceName": "SomeService",
	"Status": 200,
	"Date": "2020-03-19T18:25:43.511Z"
}
```

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