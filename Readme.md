# Developer.IO - Nerd Store Enterprise
The objective is to develop an e-commerce platform focused on the corporate sector, following sound principles and approaches that prioritize scalability, security, business/software modeling, and related aspects.

## Technologies Used
- GRPC
- .NetCore5
- SQL Server
- Messaging (RabbitMQ)

## Overview of the Adopted Architecture

The architecture proposed for this application is not a model or template for any other. Each application should have a specific architecture according to the scenario and nuances of the business problem. Therefore, the architecture of each application should be modeled according to the scenario and its specificity.

The architectural solution adopted for this application is **distributed**, or commonly known as **distributed monolith**. One of its main characteristics is the division of the application into bounded contexts, consequently distributing the application into responsible services, where each one handles a subdomain of the business. The image below shows an overview of the architecture, with 6 bounded contexts (subdomains).
The project is in portuguese, but the bounded contexts are:

- Payments -> Pagamentos
- Customers -> Clientes
- Catalogs -> Catalogos
- Orders  (core of the application) -> Pedidos (core da aplicação)
- Cart -> Carrinho 

![Application Architecture](./docs/arquitetura-aplicacao.png)

This application **does not have a microservices architecture, but is based on microservices**. This is because it dispenses with "rules" that are necessary in a microservices application. In addition, applications in this architectural style are complex and often unnecessary for the context of the business problem you want to solve.

Therefore, using a microservices architecture depending on the complexity of the business can be like using a "cannon to kill an ant."

## How to Run?

You can run the application from [Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/)

### Running in Visual Studio
To run in Visual Studio, follow these steps:

- Install the following tools: .Net 5, SQL Server (preferably with SSMS), and Docker.

- Create a RabbitMQ instance from a Docker container using the following command:

    ```
    docker run -d --hostname rabbit-host --name rabbit-nerdstore -p 15672:15672 -p 5672:5672 rabbitmq:3-management-alpine
    ```

- To run the *migrations* in the source code in each of the projects using the Package [*Manager Console (PMC)*](https://www.learnentityframeworkcore.com/migrations/commands/pmc-commands) in Visual Studio. To Run the following command in each project in PMC:

    ```
    Update-Database -Context {context_name} -StartupProject {startup_project_name}
     ```
- Set all projects to run in the SelfHosting model instead of IIS, as all URL/endpoint configurations are using the SelfHosting settings in launchSettings.json.

- Configure the solution of the application in Visual Studio to start multiple projects, exactly with the same projects shown in the figure below:

    ![Selecting projects to run in Visual Studio](./docs/executar-aplicacao-visual-studio-1.png)

- Now, start the application.

    ![Starting the application with N projects](./docs/executar-aplicacao-visual-studio-2.png)

<hr>

## References

Below are the links to the main sources for the development of this project, with a focus on the courses provided by [Desenvolvedor.IO](https://desenvolvedor.io/).

- [ASP.NET Core Enterprise Applications](https://desenvolvedor.io/curso-online-asp-net-core-enterprise-applications);
- [Dev-Store Github Repo](https://github.com/desenvolvedor-io/dev-store);
- [eShopContainers Github Repo](https://github.com/dotnet-architecture/eShopOnContainers);
- [.NET Microservices: Architecture for Containerized .NET Applications](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/);
- [Design a microservice-oriented application](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/microservice-application-design);
- [Creating a simple data-driven CRUD microservice](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/data-driven-crud-microservice);
- [Design a DDD-oriented microservice](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice).