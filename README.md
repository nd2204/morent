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

```
Morent/
├── morent-server/                # Backend solution
│   ├── Morent.Core/             # Enterprise/Domain layer
│   │   └── Entities/            # Domain entities
│   ├── Morent.Application/      # Application layer
│   │   ├── Common/             # Shared components
│   │   ├── Features/           # Use cases
│   │   └── Interfaces/         # Port definitions
│   ├── Morent.Infrastructure/   # Infrastructure layer
│   │   ├── Data/              # Database implementation
│   │   │   ├── Configs/       # Entity configurations
│   │   │   └── Migrations/    # Database migrations
│   │   └── Services/          # External service implementations
│   └── Morent.WebApi/          # Web API layer
│       └── Controllers/        # API endpoints
└── morent-client/              # React frontend
    ├── src/
    │   ├── components/        # React components
    │   ├── features/         # Feature modules
    │   └── types/           # TypeScript definitions
    └── public/              # Static assets
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
