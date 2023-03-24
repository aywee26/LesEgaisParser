# LesEgaisParser
Console application that parses LesEgais page for wood deals and saves them to database. Built as a solution of A2 entry task.

# Getting started
1. Set up database in SQL Server
     - Create user ```LesEgais```;
     - Create database ```LesEgais``` and assign created user as an owner.

Here's how it's done with ```sqlcmd```:
```
create login LesEgais with password = "LesEgais", check_policy = off
go

create database LesEgais
go

exec sp_changedbowner 'LesEgais'
go
```

2. Create table ```WoodDeals```. Create stored procedure ```sp_GetDeals```. Use queries in ```sql``` folder.

3. Edit ```LesEgaisParser\LesEgaisParser\App.config``` if necessary. You can change following parameters:

- ```configuration – connectionStrings – defaultConnection``` - SQL Server connection string.

- ```configuration – appSettings – NumberOfDealsPerRequest``` - number of deals that program gets per request, ```2500``` by default.

- ```configuration – appSettings – DelayBetweenRequestsSeconds``` - delay between requests in seconds, ```1``` by default. The delay will increase exponentially, if necessary.

- ```configuration – appSettings – DelayBetweenParsingMinutes``` - delay between every parsing attempt in minutes, ```10``` by default.

4. Build and run the project.

# Powered by
- [.NET Framework 4.8](https://learn.microsoft.com/en-us/dotnet/framework/get-started/overview) - Microsoft
- [ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/) - Microsoft
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/) - Microsoft and .NET Foundation
