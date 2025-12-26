MovieMatchAPI

MovieMatchAPI is a movie recommendation and suggestion web API built with ASP.NET Core. It provides RESTful endpoints for retrieving movie suggestions and related data, organized using a layered architecture for maintainability and testability.

Features

Movie suggestion/recommendation REST API

Modular layered solution structure (API, Application, Domain, Infrastructure)

Authentication and authorization support via a dedicated Auth API

Clean architecture with separation of concerns

DTOs and dependency injection for scalability

Entity Framework Core for data access

Project Structure
MovieMatchAPI/
│
├── MovieSuggestion.API           # Web API project
├── MovieSuggestion.Application   # Business logic layer
├── MovieSuggestion.Domain        # Domain models and interfaces
├── MovieSuggestion.Infrastructure# Data access, EF Core, repositories
├── MovieSuggestion.AuthApi       # Authentication API
├── MovieSuggestionAPI.sln        # Solution file
├── .gitignore
└── README.md

Getting Started
Prerequisites

Make sure you have the following installed:

.NET SDK (recommended latest 7.x/8.x)

SQL Server or any supported relational database

(Optional) Postman or similar tool for testing APIs

Setup

Clone the repository

git clone https://github.com/mrshri/MovieMatchAPI.git
cd MovieMatchAPI


Configure Database Connection

Open appsettings.json in MovieSuggestion.API

Add your database connection string under ConnectionStrings

Run Database Migrations

dotnet ef database update --project MovieSuggestion.Infrastructure


Run the Application

dotnet run --project MovieSuggestion.API


The API will be available at https://localhost:5001 (or similar).

API Endpoints
Endpoint	Method	Description
/api/movies	GET	Retrieve movie suggestions/list
/api/movies/{id}	GET	Get movie details by ID
/api/auth/login	POST	Authenticate a user and get a token
/api/auth/register	POST	Register a new user

Note: Update this section with actual endpoints once routes and controllers are finalized in code.

Authentication

The API uses JWT tokens for securing endpoints. Call the Auth API to obtain a token, then include it in request headers:

Authorization: Bearer <your_token_here>

Technologies Used

ASP.NET Core Web API

C#

Entity Framework Core

SQL Server (or any supported RDBMS)

JWT Authentication

Contributing

Contributions are welcome! To contribute:

Fork the repository

Create a feature branch

Commit your changes

Open a pull request
