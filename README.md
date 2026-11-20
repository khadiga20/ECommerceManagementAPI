# E-Commerce Management API

## Project Overview
E-Commerce Management API is an ASP.NET Core Web API project developed to manage products, categories, customers, and orders.

The project demonstrates data validation, global exception handling, filtering, searching, sorting, pagination, service layer architecture, dependency injection, and Entity Framework Core relationships.

## Technologies Used
- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (LocalDB)
- LINQ
- Data Annotations
- Postman

## Project Structure

```text
ECommerceManagementAPI
│
├── Controllers
├── Data
├── DTOs
├── Helper
├── Migrations
├── Models
├── Services
│   ├── Interfaces
│   └── Implementation
│
├── Screenshots
├── appsettings.json
├── Program.cs
└── README.md
```

## Database Configuration

The project uses SQL Server LocalDB.

Update the `DefaultConnection` connection string in `appsettings.json` if needed.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

## How to Run the Project

1. Open the project in Visual Studio.
2. Restore NuGet packages.
3. Configure the database connection string.
4. Open Package Manager Console.
5. Run:

```powershell
Update-Database
```

6. Build and run the project.
7. Test the API using Postman.

## API Endpoints

### Products

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get products with filtering, sorting, and pagination |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

### Categories

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/categories` | Get all categories |
| POST | `/api/categories` | Create a category |

### Orders

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/orders` | Create an order |
| GET | `/api/orders/{id}` | Get order details |

## Product Query Parameters

The products endpoint supports:

| Parameter | Description |
|---|---|
| `search` | Search by product name |
| `categoryId` | Filter by category |
| `minPrice` | Minimum product price |
| `maxPrice` | Maximum product price |
| `isActive` | Filter active or inactive products |
| `sortBy` | Sort by name or price |
| `sortOrder` | Ascending (`asc`) or descending (`desc`) |
| `page` | Page number |
| `pageSize` | Number of products per page |

Example:

```text
GET /api/products?search=Wireless&minPrice=500&maxPrice=2000&isActive=true&sortBy=price&sortOrder=asc&page=1&pageSize=10
```

## Validation and Error Handling

- Data Annotation validation is applied to request DTOs.
- Custom validation rules are implemented.
- Global exception handling returns consistent Problem Details responses.
- HTTP 404 is used for resources that do not exist.
- HTTP 409 is used for business conflicts.
- Unexpected errors are logged without exposing internal details to clients.

## Service Layer and Dependency Injection

The application separates business logic from controllers using:

- `IProductService` and `ProductService`
- `ICategoryService` and `CategoryService`
- `IOrderService` and `OrderService`

Services that depend on Entity Framework Core DbContext are registered using the Scoped lifetime.

## Order Business Rules

Before creating an order, the application checks that:

- The customer exists.
- Requested products exist and are active.
- Sufficient stock is available.
- Quantities are greater than zero.
- Prices are retrieved from the database.
- Product stock is updated when the order is successfully processed.

## EF Core Relationships

- Category → Products (One-to-Many)
- Customer → Orders (One-to-Many)
- Order → OrderItems (One-to-Many)
- Product → OrderItems (One-to-Many)

DTOs and projections are used to return the required data without exposing entity graphs directly.

## Screenshots

The `Screenshots` folder contains Postman evidence for:

# 1. Successful product creation.
<img width="977" height="691" alt="Screenshot 2026-11-20 222452" src="https://github.com/user-attachments/assets/0811dc0e-7b62-4d2c-9acb-943177fcb037" />

# 2. Invalid input validation (400).
<img width="967" height="742" alt="Screenshot 2026-11-20 222532" src="https://github.com/user-attachments/assets/287ef87a-3065-4af9-bca0-88d095ce1738" />

# 3. Not Found response (404).
<img width="991" height="741" alt="Screenshot 2026-11-20 222558" src="https://github.com/user-attachments/assets/2d9d2b87-6697-4791-b351-2395db16ca1c" />

# 4. Conflict response (409).
<img width="966" height="722" alt="Screenshot 2026-11-20 222630" src="https://github.com/user-attachments/assets/eb67808a-a5c8-490c-b535-c23c71fb0008" />

# 5. Filtering and sorting.
<img width="1165" height="796" alt="Screenshot 2026-11-20 222707" src="https://github.com/user-attachments/assets/a93349b2-2d6d-4350-bf5f-1f138c0697cb" />

# 6. Pagination.
<img width="1062" height="773" alt="Screenshot 2026-11-20 222900" src="https://github.com/user-attachments/assets/6bbb1f30-f9e2-4940-88fc-b792ffd28dc8" />

## Author

Khadiga Yahia

ASP.NET Core Web API Practical Assignment — Lectures 5 & 6
