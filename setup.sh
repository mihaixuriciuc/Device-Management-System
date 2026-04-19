#!/bin/bash
echo ""
echo "🚀 Device Management System - Setup (macOS / Linux)"
echo "==================================================="
echo ""

# Backend - .NET
echo "→ Restoring .NET backend packages..."
cd backend/DeviceManager.Api || { echo "❌ Backend folder not found!"; exit 1; }
dotnet restore
cd ../..

# Frontend - Angular
echo "→ Installing Angular frontend dependencies..."
cd frontend || { echo "❌ Frontend folder not found!"; exit 1; }
npm install
cd ..

echo ""
echo "✅ All dependencies installed successfully!"
echo ""
echo "Next steps:"
echo "  1. Update appsettings.json (JWT key, DB password, Gemini API key)"
echo "  2. Run the SQL scripts (CreateDatabase.sql then SeedData.sql)"
echo "  3. Start backend → cd backend/DeviceManager.Api && dotnet run"
echo "  4. Start frontend → cd frontend && ng serve"
echo ""
read -p "Press Enter to exit..."