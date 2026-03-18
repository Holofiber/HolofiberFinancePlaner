# Holofiber Finance Planner

A production-oriented ASP.NET Core Web API built with Clean Architecture + CQRS, containerized with Docker and deployed via automated CI/CD.

This project demonstrates a modern backend setup including domain-driven design principles, containerization, and automated deployment to a VPS.

## Architecture

The solution follows Clean Architecture principles:

- `FinancialPlanner.Domain`  
  Domain entities and business invariants.
- `FinancialPlanner.Application`  
  CQRS handlers (MediatR), validation (FluentValidation), and abstractions.
- `FinancialPlanner.Infrastructure`  
  EF Core (PostgreSQL), JWT authentication, BCrypt hashing, repository implementations.
- `FinancialPlanner.Api`  
  Controllers, middleware, rate limiting, ProblemDetails, and logging.
- `FinancialPlanner.Contracts`  
  Request/response contracts for external consumers.

## Features

- JWT authentication with access tokens
- Secure password hashing (BCrypt)
- Expense creation and retrieval
- Centralized error handling (ProblemDetails)
- Structured logging
- Rate limiting middleware
- PostgreSQL persistence
- Dockerized environment
- CI/CD pipeline (GitHub Actions -> GHCR -> VPS auto-deploy)

## Local Development (Docker)

```bash
docker compose up --build
```

API will be available at:

- `http://localhost:8080/swagger`

## Production Deployment

The application is deployed on a VPS using:

- Docker & Docker Compose
- GitHub Container Registry (GHCR)
- GitHub Actions CI/CD
- SSH-based automated deployment

Deployment flow:

```text
git push
   |
   v
GitHub Actions build
   |
   v
Push Docker image to GHCR
   |
   v
SSH into VPS
   |
   v
docker compose pull
   |
   v
docker compose up -d
```

Minimal downtime (container restart-based deployment).

## Configuration

Configuration is managed via environment variables.

Required:

- `ConnectionStrings__DefaultConnection`
- `Jwt__Secret`
- `Jwt__Issuer`
- `Jwt__Audience`

In production, secrets must be provided via environment variables (never hardcoded).

## Database

- PostgreSQL 17 (Docker container)
- Persistent Docker volume
- Health checks enabled
- Internal-only database exposure (not publicly accessible)

## Security Considerations

- JWT-based authentication
- Password hashing with BCrypt
- Rate limiting enabled
- Database not exposed publicly
- Production environment separation

## Roadmap

- Refresh token rotation & revocation
- Role-based authorization
- API integration tests
- Observability (metrics, tracing)
- HTTPS via reverse proxy

## Purpose

This project demonstrates:

- Clean Architecture in a real-world setup
- Proper separation of concerns
- Containerized backend deployment
- Automated CI/CD pipeline
- Production-oriented infrastructure decisions
