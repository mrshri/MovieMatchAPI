# MovieMatchAPI

MovieMatchAPI is a production-ready movie recommendation Web API built using ASP.NET Core.  
The project demonstrates clean architecture, secure authentication, and scalable backend design suitable for real-world enterprise applications.

## Why This Project?

This project was built to showcase:
- Strong backend development skills in ASP.NET Core
- Clean architecture and separation of concerns
- Secure API design with JWT authentication
- Scalable, maintainable, and testable code structure

## Key Highlights

- Designed and developed RESTful APIs for movie suggestions and related operations  
- Implemented clean layered architecture (API, Application, Domain, Infrastructure)  
- Secured endpoints using JWT-based authentication and authorization  
- Used Entity Framework Core with repository pattern for data access  
- Applied DTOs and dependency injection to improve maintainability and scalability  

## Architecture Overview

The solution follows Clean Architecture principles:

MovieMatchAPI/
- MovieSuggestion.API – API controllers and request handling  
- MovieSuggestion.Application – Business logic and services  
- MovieSuggestion.Domain – Core domain models and interfaces  
- MovieSuggestion.Infrastructure – EF Core, repositories, database context  
- MovieSuggestion.AuthApi – Authentication and authorization  

This structure ensures loose coupling and easy extensibility.

## Technologies Used

- ASP.NET Core Web API  
- C#  
- Entity Framework Core  
- SQL Server  
- JWT Authentication  
- Dependency Injection  

## Sample API Endpoints

| Endpoint | Method | Description |
|--------|--------|-------------|
| /api/movies | GET | Fetch movie suggestions |
| /api/movies/{id} | GET | Get movie details |
| /api/auth/login | POST | User authentication |
| /api/auth/register | POST | User registration |

## How to Run Locally

1. Clone the repository
git clone https://github.com/mrshri/MovieMatchAPI.git

cd MovieMatchAPI
2. Update the database connection string in `appsettings.json`

3. Apply migrations
dotnet ef database update --project MovieSuggestion.Infrastructure


4. Run the API


dotnet run --project MovieSuggestion.API


## Learning Outcomes

- Hands-on experience with clean architecture in ASP.NET Core  
- Practical implementation of JWT authentication  
- Designing scalable REST APIs  
- Applying enterprise-level coding standards  

## Future Enhancements

- Advanced recommendation algorithms  
- Pagination, sorting, and filtering  
- Swagger/OpenAPI documentation  
- Unit and integration testing  
- Integration with external movie data APIs  
