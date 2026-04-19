# Device Management System

A full-stack device inventory management system built with Angular 21 (Frontend) and .NET 8 (Backend).

## Features

- User registration and login with JWT + HttpOnly cookies
- Role-based access (Admin & User)
- Device CRUD operations (Admin only)
- Assign / Unassign devices to users
- My Profile page with assigned devices
- Free-text search with relevance ranking (Name > Manufacturer > Processor > RAM)
- AI-powered device description generator (Google Gemini)

---

## How to Run Locally

### 1. Clone the Repository(make sure you have git installed)

    git clone https://github.com/mihaixuriciuc/Device-Management-System.git
    cd Device-Management-System

### 2. Install All Dependencies (One Command)

We provide cross-platform setup scripts so you can install everything in one go:

On Windows (double-click this file):
setup.bat

On macOS / Linux (run in terminal):
chmod +x setup.sh
./setup.sh

These scripts will automatically:

- Restore all .NET backend packages
- Install all Angular frontend dependencies (npm install)

Make sure the Angular and .NET are correctly installed

### 3. Configure the Application

Open backend/DeviceManager.Api/appsettings.json and replace the placeholders:

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Database=DeviceDb;User Id=sa;Password=YOUR_SQL_SA_PASSWORD;TrustServerCertificate=True;"
      },
      "Jwt": {
        "Key": "YOUR_LONG_JWT_SECRET_KEY_HERE_AT_LEAST_32_CHARACTERS",
        "Issuer": "DeviceManager.Api",
        "Audience": "DeviceManager.Angular"
      },
      "Gemini": {
        "ApiKey": "your_gemini_api_key_here"
      },
      "AdminSettings": {
        "Email": "admin@devicemanager.com",
        "Password": "SuperSecretAdmin123!"
      }
    }

Create and seed the database:
For this step to work, we need to do the following:

1. Download Docker from: https://docs.docker.com/get-started/get-docker/
2. Use SQL Server(mssql) extension in VSCode
3. Open Docker Desktop (the whale icon).
4. Wait until it says “Docker Desktop is running” (green light).
5. Open PowerShell (recommended) or Command Prompt as Administrator and copy-paste this command:

```
docker run -e "ACCEPT_EULA=Y" `
           -e "MSSQL_SA_PASSWORD=YOUR_SQL_SA_PASSWORD" `
           -p 1433:1433 `
           --name DeviceDb `
           -d mcr.microsoft.com/mssql/server:2022-latest

```

6. Use this command to remive migrations

```
dotnet ef database drop --force
```

7. Generate from models Migrations

```
dotnet ef migrations add InitialSetup
```

8. Aplly migrations

```
dotnet ef database update
```

. DBScripts/CreateDatabase.sql → Creates the DeviceDb database
. DBScripts/SeedData.sql → Inserts 15 sample devices

### 4. Start the Application

Backend (in one terminal):
cd backend/DeviceManager.Api
dotnet run

API will be available at: http://localhost:5246

Frontend (in another terminal):
cd frontend
ng serve

Open your browser: http://localhost:4200

---

## Default Login Credentials

Admin Account:

- Email: admin@devicemanager.com
- Password: SuperSecretAdmin123!

Regular Users: Register new accounts from the Register page.

---

## Project Structure

    Device-Management-System/
    ├── backend/
    │   └── DeviceManager.Api/          # .NET 8 Web API + Identity
    ├── frontend/                       # Angular 21 Frontend
    ├── DBScripts/
    │   ├── CreateDatabase.sql
    │   └── SeedData.sql                # 15 sample devices
    ├── DeviceManager.Tests/            # Integration tests
    ├── setup.bat                       # Windows setup script
    ├── setup.sh                        # macOS / Linux setup script
    └── README.md

---

## Technologies Used

- Backend: .NET 8, Entity Framework Core, ASP.NET Identity, JWT
- Frontend: Angular 21, Standalone Components, Reactive Forms
- Database: MSSQL Server
- AI: Google Gemini

---

## Important Notes

- Make sure SQL Server is running on localhost,1433 with the sa user.
- Update the JWT secret key and database password before running in production.
- The search feature is case-insensitive and ranks results by relevance (Name has the highest priority).
- The backend uses secure HttpOnly cookies for authentication.

Project is ready to run! If you face any issues during setup, feel free to open an issue on GitHub.

Last updated: April 2026
