# ChatStocksNet
## C# .NET chat application with stocks bot funcionality

### This project was made with .NET Core 3.1 (C#) and the following libraries:

- MediatR
- EntityFrameworkCore
- Microsoft Identity
- Microsoft Extensions
- Npgsql
- RabbitMQ
- Swashbuckle

### Installation:

- For the bot funcionality, we need to ensure that a RabbitMQ server is running. **(supposed to run on port 5672)** 
- But, in my case, I run a docker container with the following command:

```

docker run -d -p 15672:15672 -p 5672:5672 rabbitmq:3-management

```

- Inside the cloned folder, copy the following steps in a terminal **(.NET 3.1 SDK required)**

```

dotnet build

dotnet run --project .\src\WebUI\

```

- Later, open https://localhost:5588/ in a browser.
- You must be register before enter the chat rooms with a strong password **(a uppercase character, a lowercase character, a digit, a non-alphanumeric character, and at least six characters long)**.
