
# Unishere


🚀 **Unishere** is a template designed to kickstart your .NET micro services application with best practices


## 🛠 Microservices

#### Microservices Communication
* Sync inter-service **gRPC Communication**
* Async Microservices Communication with **RabbitMQ Message-Broker Service**
* Using **RabbitMQ Publish/Subscribe Topic** Exchange Model
* Using **MassTransit** for abstraction over RabbitMQ Message-Broker system
* Publishing BasketCheckout event queue from Basket microservices and Subscribing this event from Ordering microservices
* Create **RabbitMQ EventBus.Messages library** and add references Microservices

#### Explorer Microservice
* Developing **CQRS with using MediatR, FluentValidation**
* Consuming **RabbitMQ** BasketCheckout event queue with using **MassTransit-RabbitMQ** Configuration

#### Yarp API Gateway Microservice
* Develop API Gateways with **Yarp Reverse Proxy** applying Gateway Routing Pattern
* Yarp Reverse Proxy Configuration; Route, Cluster, Path, Transform, Destinations
* **Rate Limiting** with FixedWindowLimiter on Yarp Reverse Proxy Configuration
* **SqlServer database** connection and containerization
* Using **Entity Framework Core ORM** and auto migrate to SqlServer when application startup

## 🔧 Run The Project
### docker

```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

dotnet tool install --global dotnet-ef

dotnet ef migrations add openIddict --context ApplicationDbContext

dotnet ef migrations add explorer --project ../Unisphere.Explorer.Infrastructure/

dotnet ef migrations add identity --project ../Unisphere.Identity.Infrastructure/
```
