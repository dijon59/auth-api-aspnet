#!/bin/bash

echo "Building and starting API..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "Error: Docker is not running. Please start Docker and try again."
    exit 1
fi

echo "Docker is running"
echo ""

# Clean up old containers and volumes
echo "Cleaning up old containers and volumes..."
docker-compose down -v
echo ""

# Build and start services
echo "Building Docker images..."
docker-compose build --no-cache
echo ""

echo "Starting services..."
docker-compose up -d --build
echo ""


echo "Containers started!"

echo "API running at: http://localhost:5054/"