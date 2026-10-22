# Factura API Security Project

A .NET 10 ASP.NET Core Web API for managing user records stored in SQL Server. The API uses Microsoft Entra ID (Azure AD) OpenID Connect authentication, Entity Framework Core, and a repository/service structure.

## Features

- Create, retrieve, update, and deactivate user records.
- List active users and look up a user by database ID or national ID.
- Authenticate protected API routes through Microsoft Entra ID.
- Generate an OpenAPI document in the Development environment.

## Technology

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10 with SQL Server
- Microsoft.Identity.Web (OpenID Connect)

## Requirements

- .NET 10 SDK
- A SQL Server database with the `sec.User` table available
- A Microsoft Entra ID app registration configured for OpenID Connect

The project does not include database migrations or a database creation script. Prepare the database and table before running the API. The table should match the `User` entity described below.

## Configuration

Configure the following values outside source control. The checked-in settings contain empty placeholders; do not put production credentials in those files.

### Microsoft Entra ID

Set these configuration values under the `AzureAd` section:

| Setting | Description |
| --- | --- |
| `Instance` | Entra ID authority, normally `https://login.microsoftonline.com` |
| `TenantId` | Directory (tenant) ID |
| `ClientId` | Application (client) ID |
| `ClientSecret` | Client secret for the app registration |
| `Domain` | Tenant domain, if required by the app registration |
| `CallbackPath` | OpenID Connect callback path; configured as `/signin-oidc` |

For environment variables, use the ASP.NET Core double-underscore convention, such as `AzureAd__TenantId`, `AzureAd__ClientId`, and `AzureAd__ClientSecret`. Register the callback URL for the environment with the Entra ID app registration.

### SQL Server

The connection string key is `AZURE_SQL_CONNECTIONSTRING`.

- **Development:** The application reads `ConnectionStrings:AZURE_SQL_CONNECTIONSTRING` from configuration. The Development environment loads `appsettings.Development.json` in addition to the normal configuration sources. You can also set `ConnectionStrings__AZURE_SQL_CONNECTIONSTRING` as an environment variable.
- **Other environments:** Set the `AZURE_SQL_CONNECTIONSTRING` environment variable directly.

Example connection string format:

`Server=<server>;Database=<database>;User Id=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;`

Use a trusted secret store or local user secrets for credentials. Do not commit secrets or connection strings.

## Run locally

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --launch-profile https
```

The HTTPS launch profile listens on `https://localhost:5001` and `http://localhost:5052`. Configure the database connection and Entra ID settings before testing protected endpoints. The `http` launch profile uses `http://localhost:5052`.

In Development, the OpenAPI document is available at:

`https://localhost:5001/openapi/v1.json`

OpenAPI is mapped only in Development. The project does not configure Swagger UI.

## API

All `/User` endpoints require authentication. An unauthenticated request is challenged using the configured OpenID Connect scheme, which normally redirects to the Entra ID sign-in flow. The API currently configures OpenID Connect authentication; although the JWT bearer package is referenced, a JWT bearer authentication handler is not configured in `Program.cs`.

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/User` | List active users. |
| `GET` | `/User/{id}` | Retrieve a user by database ID. |
| `GET` | `/User/nationalId/{nationalId}` | Retrieve a user by national ID. |
| `POST` | `/User` | Create a user. Returns `409 Conflict` when the national ID already exists. |
| `PATCH` | `/User` | Update the supported user fields for the record identified by `id`. |
| `DELETE` | `/User/{id}` | Soft-delete a user by setting `IsActive` to `false`. |

The controllers use `200 OK` for successful create, update, and delete operations. Lookups and updates/deletes for records that do not exist return `404 Not Found`.

### User JSON fields

The API binds request JSON to the `User` entity. The JSON property names are typically camel-cased by ASP.NET Core:

| Field | Type | Notes |
| --- | --- | --- |
| `id` | integer | Database key. Used to identify records for updates. |
| `name` | string | User's first name. |
| `lastName` | string | User's last name. |
| `nationalId` | string | Unique identifier checked when creating a record. |
| `countryId` | integer | Country identifier. |
| `email` | string | Email address. |
| `phoneNumber` | integer | Phone number as an integer in the current model. |
| `address` | string | Address. |
| `isActive` | boolean | Controls whether a record appears in the list endpoint. |
| `datetime` | date/time | Date/time value stored with the record. |

Example create request:

```http
POST /User
Content-Type: application/json

{
  "name": "Alex",
  "lastName": "Morgan",
  "nationalId": "A1234567",
  "countryId": 1,
  "email": "alex@example.com",
  "phoneNumber": 5551234,
  "address": "100 Main Street",
  "isActive": true,
  "datetime": "2026-01-15T12:00:00Z"
}
```

The repository checks national ID uniqueness across stored records, including inactive records. `GET /User` returns only active records, while lookups by ID and national ID can return inactive records. A newly created user is only included in the list when `isActive` is `true`.

`PATCH /User` updates `name`, `lastName`, `countryId`, `email`, `phoneNumber`, `address`, and `isActive` for the supplied `id`. It does not update `nationalId` or `datetime`; submit the full entity shape for fields that the update operation uses.

### Public test route

`GET /test/{nombre}` returns a simple Spanish greeting, for example `/test/Alex` returns `hola, Alex!`. This route is mapped separately and is not covered by the `[Authorize]` attribute on `UserController`.

## Project structure

```text
Controllers/       HTTP endpoints and authorization
Data/              EF Core DbContext and entity model
Repositories/      SQL-backed user data access
Services/          User service layer
Program.cs         Authentication, dependency injection, database, and middleware setup
```
