 
#### Contact microservice which includes; 
* Onion Architecture principle
* Implementing **DDD, CQRS, and Clean Architecture** with using Best Practices
* Developing **CQRS with using MediatR, FluentValidation packages**
* ASP.NET Core Web API application 
* REST API principles, CRUD operations
* **PostgreSQL database** connection and containerization
* Repository Pattern Implementation
* Swagger Open API implementation	
* Using **Entity Framework Core ORM** and auto migrate to Postgre server when application startup
* Use the **HealthChecks** feature in back-end 
  
#### Report microservice which includes;
* ASP.NET Web API application
* REST API principles, CRUD operations
* **PostgreSQL database** connection and containerization
* Consume Contact **REST Service** for contact informations and report
* Publish Report Queue with using **CAP and RabbitMQ** 
* Using **RabbitMQ Publish/Subscribe cap.default.topic** Exchange Model
* More **resilient used IHttpClientFactory**  to implement resilient HTTP requests 
* Implement **Retry and Circuit Breaker patterns** with exponential backoff with IHttpClientFactory and **Polly policies**
 
#### Docker Compose establishment with all microservices on docker;
* Containerization of microservices
* Containerization of databases
* Containerization of message queue
* Override Environment variables
 
### Installing
1. Clone the repository 
2. At the root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
or run docker-compose project on visual studio.

3. You can **launch microservices** as below urls:
* **Contact API -> http://localhost:5001/swagger/index.html**
* **Report API -> http://localhost:7001/swagger/index.html** 
* **Rabbit Management Dashboard -> http://localhost:15672**   -- guest/guest 
* **pgAdmin PostgreSQL -> http://localhost:5433**   -- contactsa/SplArmonsMAZONTINGEriCi