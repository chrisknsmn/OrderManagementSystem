#!/bin/bash

echo "🚀 Building Repair Order Management System..."

# Navigate to frontend directory
cd frontend

# Install dependencies if node_modules doesn't exist
if [ ! -d "node_modules" ]; then
    echo "📦 Installing frontend dependencies..."
    npm install
fi

# Build the frontend
echo "🔨 Building Vue.js frontend..."
npm run build

# Navigate back to root
cd ..

# Build the .NET API
echo "🔨 Building .NET API..."
dotnet build --configuration Release

echo "✅ Build completed successfully!"
echo "📁 Frontend files are in: wwwroot/"
echo "🚀 Ready for deployment!"