<p align="center">
  <img src="./docs/Logo.png"/>
</p>

<p align="center">
  <i>A Modern Car Rental Application Built with Clean Architecture</i>
</p>

<p align="center">
  <img src="https://img.shields.io/github/license/nd2204/morent?style=for-the-badge" />
  <img src="https://img.shields.io/github/actions/workflow/status/nd2204/morent/workflow.yml?style=for-the-badge" />
  <img src="https://img.shields.io/github/commit-activity/w/nd2204/morent?style=for-the-badge" />
</p>

## 🏗 Architecture Overview

This project follows Clean Architecture principles with a clear separation of concerns:

```sh
morent-server/
│
├── Morent.Core/                       # Domain layer - core business entities, rules, and value objects
│   ├── MorentCarAggregate/            # Car domain models and business logic
│   ├── MorentUserAggregate/           # User domain models and business logic
│   ├── MorentReviewAggregate/         # Review domain models and business logic
│   ├── MorentRentalAggregate/         # Rental domain models and business logic
│   ├── MorentPaymentAggregate/        # Payment domain models and business logic
│   ├── MediaAggregate/                # Media handling domain models
│   ├── ValueObjects/                  # Reusable value objects (Money, Location, etc.)
│   ├── Interfaces/                    # Core interfaces and abstractions
│   ├── Exceptions/                    # Domain-specific exceptions
│   ├── GlobalUsing.cs                 # Global using statements
│   ├── CoreServiceExtensions.cs       # DI extensions for core services
│   └── AssemblyReference.cs           # Assembly reference marker
│
├── Morent.Application/                # Application layer - use cases, commands, queries
│   ├── Features/                      # Application features organized by domain
│   │   ├── Car/                       # Car-related features
│   │   │   ├── Commands/              # Create, Update, Delete car commands
│   │   │   ├── Queries/               # Get cars queries
│   │   │   └── DTOs/                  # Car data transfer objects
│   │   ├── User/                      # User-related features
│   │   ├── Rental/                    # Rental-related features
│   │   ├── Review/                    # Review-related features
│   │   └── Payment/                   # Payment-related features
│   ├── Repositories/                  # Repository interfaces
│   ├── Interfaces/                    # Application interfaces
│   ├── Exceptions/                    # Application-specific exceptions
│   ├── Extensions/                    # Extension methods for application layer
│   ├── GlobalUsing.cs                 # Global using statements
│   └── AssemblyReference.cs           # Assembly reference marker
│
├── Morent.Infrastructure/             # Infrastructure layer - external concerns, data access
│   ├── Data/                          # Data access implementation
│   │   ├── Repositories/              # Repository implementations
│   │   ├── Configs/                   # Entity configurations for EF Core
│   │   └── MorentDbContext.cs         # Database context
│   ├── Services/                      # External service implementations
│   ├── Migrations/                    # EF Core database migrations
│   ├── Email/                         # Email service implementation
│   ├── Settings/                      # Application settings and configuration classes
│   ├── GlobalUsing.cs                 # Global using statements
│   └── InfrastructureServiceExtensions.cs  # DI extensions for infrastructure services
│
├── Morent.WebApi/                     # Presentation layer - API controllers, Swagger, etc.
│   ├── Controllers/                   # API controllers
│   ├── Configurations/                # API configurations
│   ├── Middlewares/                   # Custom middleware components
│   ├── SeedData/                      # Seed data for development/testing
│   ├── Properties/                    # Project properties
│   ├── wwwroot/                       # Static web resources
│   ├── SeedData.cs                    # Seed data initialization
│   ├── Program.cs                     # Application entry point
│   ├── appsettings.json               # Application settings
│   ├── GlobalUsing.cs                 # Global using statements
```

## 🛠 Technology Stack

### Backend (.NET 9)
- **Framework**: ASP.NET Core Web API
- **Architecture**: Clean Architecture with CQRS
- **ORM**: Entity Framework Core
- **Database**: 
  - Development: SQLite
  - Production: SQL Server
- **Authentication**: JWT Bearer tokens

### Frontend (React 18)
- **Framework**: React with TypeScript
- **State Management**: React Query & Context API
- **Styling**: TailwindCSS
- **API Client**: Axios

### Development Tools
- **Version Control**: Git
- **CI/CD**: GitHub Actions
- **Containerization**: Docker
- **API Documentation**: Swagger/OpenAPI

## 🚀 Getting Started

1. **Clone the Repository**

```bash
git clone https://github.com/nd2204/morent.git
cd morent
```

2. **Set Up the Database**

```bash
# Apply migrations
./migrate.sh
```

3. **Start the Backend**

```bash
cd morent-server/Morent.WebApi
dotnet run
```

4. **Start the Frontend**

```bash
cd morent-client
npm install
npm run dev
```

## 📦 Development Database

The development environment uses SQLite for simplicity:

- **Database File**: `Morent.WebApi/Morent.db`
- **Connection String**: `"Data Source=Morent.db"`
- **Migration Commands**:

```bash
# Add new migration
dotnet ef migrations add "MigrationName" --project morent-server/Morent.Infrastructure --startup-project morent-server/Morent.WebApi

# Update database
dotnet ef database update --project morent-server/Morent.Infrastructure --startup-project morent
```

## Repo

![Morent Repo](https://repobeats.axiom.co/api/embed/0ab87f4aa83a4c4fc9ef278050025fd3c3e39339.svg "Repobeats analytics image")

## 👥 Contributing

Contributions are welcome! Follow these steps to contribute:

- Fork the repository
- Create a new branch: `git checkout -b feature-name`
- Commit your changes: `git commit -m 'Add new feature'`
- Push to the branch: `git push origin feature-name`
- Submit a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
