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

### 1. Clone the Repository

git clone https://github.com/YOUR-USERNAME/Device-Management-System.git
cd Device-Management-System

### 2. Backend Setup (.NET 8)

Navigate to the backend folder:
cd backend/DeviceManager.Api

Update appsettings.json
Open backend/DeviceManager.Api/appsettings.json and replace the placeholders:

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

Create and Seed the Database
Open SQL Server Management Studio (SSMS) or Azure Data Studio and run the scripts in this exact order:

1. DBScripts/CreateDatabase.sql → Creates the DeviceDb database
2. DBScripts/SeedData.sql → Inserts 15 sample devices

Run the Backend:
dotnet run

The API will start at: http://localhost:5246

### 3. Frontend Setup (Angular)

Navigate to the frontend folder:
cd ../../frontend

Install dependencies:
npm install

Start the Angular app:
ng serve

Open your browser and go to: http://localhost:4200

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
│ └── DeviceManager.Api/ # .NET 8 Web API + Identity
├── frontend/ # Angular 21 Frontend
├── DBScripts/
│ ├── CreateDatabase.sql
│ └── SeedData.sql # 15 sample devices
├── DeviceManager.Tests/ # Integration tests
└── README.md

---

## Technologies Used

- Backend: .NET 8, Entity Framework Core, ASP.NET Identity, JWT
- Frontend: Angular 21, Standalone Components, Reactive Forms
- Database: SQL Server
- AI: Google Gemini

---

## Important Notes

- Make sure SQL Server is running on localhost,1433 with the sa user.
- Update the JWT secret key and database password before running in production.
- The search feature is case-insensitive and ranks results by relevance (Name has the highest priority).
- The backend uses secure HttpOnly cookies for authentication.

Project is ready to run! If you face any issues during setup, feel free to open an issue on GitHub.

Last updated: April 2026
