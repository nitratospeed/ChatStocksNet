# ChatStocksNet
## C# .NET chat application with stocks bot funcionality

### This project was made with .NET Core 3.1 (C#) and the following libraries:

- MediatR
- Microsoft Entity Framework Core
- Microsoft Identity
- Microsoft Extensions
- InMemory EFCore (DB only persists when app is running, stored in memory)
- RabbitMQ
- SignalR
- MassTransit
- CsvHelper
- Swashbuckle

### Installation:

- For the stocks funcionality, we need to ensure that a RabbitMQ server is running. **(supposed to run on port 5672)** 
- But, in my case, I run a docker container with the following command:

```

docker run -d -p 15672:15672 -p 5672:5672 rabbitmq:3-management

```

- Chat App: Inside the cloned folder, copy the following steps in a terminal **(.NET 3.1 SDK required)**

```

dotnet build

dotnet run --project .\src\Chat\WebUI\

```

- Later, open https://localhost:5099/ in a browser.
- You must be register before enter the chat rooms with a strong password **(a uppercase character, a lowercase character, a digit, a non-alphanumeric character, and at least six characters long)**.

- Stocks App: Inside the cloned folder, copy the following steps in a terminal **(.NET 3.1 SDK required)**

```

dotnet build

dotnet run --project .\src\Stocks\

```

- Later, open https://localhost:5097/ in a browser.
