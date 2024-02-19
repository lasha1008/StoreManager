This project is example of online store.
There are 8 different layers (dlls): 
1.DTO (data transfer object) _ for data objects.
2.Repositories _ for data communication, in this case I'm using Entity Framework and mssql. Repository pattern and UnitOfWork pattern is also included here.
3.Services _ buisiness logic is writen in this layer. CQRS pattern is included.
4.Facade _ every necessary interfaces, extensions and custom exceptions are located here.
5.Tests _ it includes every xunit tests for Repositories and Services. Database runs inMemory for testing.
6.Models _ models are similar to DTOs but they contain only necessary fields.
7.API _ this module includes configurations such as: dependency injection, swagger, JWT authorization, serilog, global exception handler, autoMapper. Every api controller is located here.
8.Web _ this is independent project which uses MVC pattern to create page views. This module uses API (Module 7).
