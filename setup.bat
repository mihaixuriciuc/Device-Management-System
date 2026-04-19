@echo off
echo.
echo 🚀 Device Management System - Setup (Windows)
echo =============================================
echo.

:: Backend - .NET
echo → Restoring .NET backend packages...
cd backend\DeviceManager.Api
dotnet restore
cd ..\..

:: Frontend - Angular
echo → Installing Angular frontend dependencies...
cd frontend
npm install
cd ..

echo.
echo ✅ All dependencies installed successfully!
echo.
echo Next steps:
echo   1. Update appsettings.json (JWT key, DB password, Gemini API key)
echo   2. Run the SQL scripts (CreateDatabase.sql then SeedData.sql)
echo   3. Start backend → cd backend\DeviceManager.Api && dotnet run
echo   4. Start frontend → cd frontend && ng serve
echo.
pause