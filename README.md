# BlackDragonAPI

BlackDragonAPI is a .NET-based Web API designed to provide secure and performant web services utilizing modern best practices, including authentication, authorization, and database integration.

## Table of Contents
- [Overview](#overview)
- [Setup](#setup)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Technologies Used](#technologies-used)

---

## Overview

BlackDragonAPI is built using ASP.NET Core and provides the following features:
- **Authentication and Authorization**: Uses JWT-based authentication to secure the API.
- **Entity Framework Core**: Integrated with SQLite to manage the application's data models.
- **Swagger/OpenAPI**: Enables API documentation and testing.
- **Development-Specific Configuration**: Separate configuration files for development and production environments.

---

## Setup

To run the BlackDragonAPI locally, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/terryreidhead/BlackDragonAPI.git
   ```

2. Navigate to the project directory:
   ```bash
   cd BlackDragonAPI
   ```

3. Update the `appsettings.json` file:
   - Replace `CHANGE_THIS_TO_A_LONG_RANDOM_SECRET_32CHARS_MIN` in the `Jwt:Key` field with a strong, random secret key.
   - Ensure the `ConnectionStrings:DefaultConnection` field points to your desired database path (e.g., `blackdragon.db`).

4. Run the application:
   ```bash
   dotnet run
   ```

By default, the API will be available at `http://localhost:5209`.

---

## Configuration

BlackDragonAPI uses JSON configuration files for environment-specific app settings.

### `appsettings.json`
This file contains the general configuration for the application, including:
- **Logging settings**
- **Database connection string**
- **JWT authentication settings**

Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=blackdragon.db"
  },
  "Jwt": {
    "Issuer": "BlackDragonAPI",
    "Audience": "BlackDragonApp",
    "Key": "SET_YOUR_SECRET_KEY"
  }
}
```

---

## API Endpoints

### Example Endpoint
The API includes different endpoints for your services. Here’s an example HTTP request defined as a test:

- **GET** `/weatherforecast`
  - Test request using the `BlackDragonAPI.http` file:
    ```http
    @BlackDragonAPI_HostAddress = http://localhost:5209

    GET {{BlackDragonAPI_HostAddress}}/weatherforecast/
    Accept: application/json
    ```

Add more endpoints and describe their behavior as you develop the API.

---

## Technologies Used

The project leverages the following technologies and frameworks:

### Framework:
- [.NET 10.0](https://dotnet.microsoft.com/) *(Target Framework)*

### NuGet Packages:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (JWT-based authentication)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (Identity services)
- `Microsoft.AspNetCore.OpenApi` (OpenAPI/Swagger integration)
- `Microsoft.EntityFrameworkCore` (ORM for SQLite)
- `Microsoft.EntityFrameworkCore.Sqlite` (SQLite provider)
- `Swashbuckle.AspNetCore` (API documentation and testing)

---

## Contribution Guideline

Feel free to submit issues and pull requests for any improvements or new features you would like to see.

---

## License

This project is licensed under the **[MIT License](LICENSE.md)**. Feel free to use, modify, and distribute this software.

---

## Contact

For any inquiries or feature requests, you can contact the repository owner at **[GitHub Profile](https://github.com/terryreidhead)**.