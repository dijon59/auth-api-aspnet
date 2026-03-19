# Authentication Web Application

This is a full-stack authentication application built with React, C# (.NET 8), PostgreSQL, and Docker.


## Prerequisites
The following are the tools required to start the project
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)
- [Node.js 20+](https://nodejs.org/)

## Quick Start


1. **For Backend : Run the build script**:
   ```bash
   ./build.sh
   ```
   You can also run manual docker compose by using the following command
   ```
    docker-compose up --build
   ```

2. **Access the application**:
   - Frontend: http://localhost:3000 -> Make sure you run the frontend, NB Frontend does not run in the same container. It is separate. 
   - Backend API: http://localhost:5054


## API Endpoints

### Authentication Endpoints

#### 1. Register User
POST /api/auth/register
```
curl -X POST http://localhost:5054/api/auth/register \
-H "Content-Type: application/json" \
-d '{
  "firstName": "John",
  "lastName": "doe",  
  "email": "john@test.com",
  "password": "123456"
}'
```

**Response**:
```json
{
    "firstName":"John",
    "lastName":"doe",
    "email":"john@test.com"
}
```

#### 2. Login User
POST /api/auth/login
```
curl -X POST http://localhost:5054/api/auth/login \   
-H "Content-Type: application/json" \
-d '{
  "email": "john@test.com",
  "password": "123456"
}'
```

**Response**:
```json
{
    "token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e...",
    "username":"John",
}
```

#### 3. Get User Details (Protected)
```http
GET /api/auth/user
Authorization: Bearer <token>
```

**Response**:
```json
{
    "firstName": "John",
    "lastName": "doe",
    "email": "john@test.com"
}
```