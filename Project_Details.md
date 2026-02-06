# Hotel API - Technical Documentation

## Project Overview

**Project Name:** Hotel API  
**Framework:** ASP.NET Core (NET 10)  
**Architecture:** Three-tier Layered Architecture (Controller ? Business Logic ? Data Access)  
**Primary Purpose:** RESTful API for managing hotel-related data with focus on country information  
**Current Branch:** country_api  

The Hotel API is a modern, scalable backend service designed to provide hotel booking and management functionality. The initial implementation focuses on country data retrieval with built-in caching and comprehensive error handling.

---

## Architecture Overview

### Layered Architecture Components

### Dependency Injection & Configuration

- **IoC Container:** Microsoft.Extensions.DependencyInjection
- **Service Registration:** `DependencyConfiguration.RegisterServices()`
- **Scope:** Scoped for all services (per HTTP request)

---

## Core API Endpoints

### 1. Get Country List

**Endpoint:** `GET /api/country/list`

**Description:** Retrieves a complete list of all countries with caching support.

**Response Schema:**

### ResponseResult<T>
Generic response wrapper for all API responses.

````````

# Response
````````markdown					
````````

### SqlHelper

**Purpose:** Centralized SQL execution and database operations

**Features:**
- Connection management
- Query execution
- Result mapping to strongly-typed models
- Exception handling

---

## CORS Configuration

**Enabled Origins:** `http://localhost:3000`

**Allowed Methods:** All (GET, POST, PUT, DELETE, etc.)

**Allowed Headers:** All

**Purpose:** Enable frontend applications running on `localhost:3000` to communicate with this API.

---

## Features

### 1. **In-Memory Caching**
   - Reduces database load
   - Improves response time
   - Configurable TTL and sliding expiration

### 2. **Async/Await Pattern**
   - Non-blocking operations
   - Better scalability
   - Responsive API

### 3. **Dependency Injection**
   - Loose coupling
   - Easy testing with mocks
   - Centralized configuration

### 4. **Layered Architecture**
   - Separation of concerns
   - Maintainability
   - Testability

### 5. **Structured Exception Handling**
   - Consistent error responses
   - Detailed error information
   - User-friendly messages

### 6. **Response Standardization**
   - Uniform response format
   - Includes metadata (pagination, sorting)
   - Exception details for debugging

### 7. **Swagger/OpenAPI Integration**
   - Auto-generated API documentation
   - Interactive testing interface
   - API endpoint discovery

---

## Request/Response Flow	

#### Get Specific Country

**Response:**
````````


# Response
````````markdown
````````

---

## Performance Considerations

1. **Caching Impact:** Significantly reduces database load for repeated queries
2. **Async Operations:** Non-blocking I/O improves throughput
3. **Sliding Expiration:** Ensures frequently accessed data stays cached
4. **Null Caching:** Prevents thundering herd for non-existent resources

---

## Future Enhancement Opportunities

1. Implement Redis for distributed caching
2. Add pagination support for country list
3. Implement request logging and monitoring
4. Add API versioning
5. Implement rate limiting
6. Add authentication/authorization
7. Implement database query optimization with indexes
8. Add integration tests
9. Implement API versioning through headers or URL paths
10. Add comprehensive API documentation with examples

---

**Documentation Generated:** February 05, 2026  
**Project Repository:** https://192.168.1.218/gaurav/hotelapi  
**Branch:** country_api  
**Framework Target:** .NET 10