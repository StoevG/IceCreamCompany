# IceCreamCompany API

**IceCreamCompany API** is a comprehensive solution designed to manage and synchronize workflows within the IceCreamCompany ecosystem. Built using Clean Architecture principles, it provides a well-structured, maintainable, and extensible system for handling workflows, integrating external data sources, and ensuring efficient data management.

---

## Table of Contents

- [Technical Overview](#technical-overview)
- [Product Structure](#product-structure)
---

![Workflows screenshot](https://raw.githubusercontent.com/StoevG/IceCreamCompany/refs/heads/main/docs/images/FE.png)
![Swagger UI screenshot](https://raw.githubusercontent.com/StoevG/IceCreamCompany/refs/heads/main/docs/images/BE.png)

## Technical Overview

IceCreamCompany API is built using **Clean Architecture** with a clear separation of concerns:

- **API Layer:**  
  Contains controllers that handle HTTP requests and responses, with custom Swagger setup for interactive HTML documentation.

- **Application Layer:**  
  Contains services, view models, and mappers that orchestrate business logic and integrate with external APIs.

- **Domain Layer:**  
  Defines core entities, repository interfaces, and shared constants (such as error messages).

- **Infrastructure Layer:**  
  Implements data access using the Repository Pattern with EF Core DbContext and integrates external services (such as Redis for caching).

This design ensures that the core business logic is decoupled from external dependencies and is built for maintainability and extensibility.

---

## Product Structure

- **IceCreamCompany.Api**  
  Contains all API controllers (e.g., `WorkflowController`) and global error handling middleware.

- **IceCreamCompany.Application.Core**  
  Implements core services (e.g., `WorkflowService`, `UniversalLoaderService`), view models, mappers, and external API integrations.

- **IceCreamCompany.Domain**  
  Defines core domain entities (`Workflow`), repository interfaces, and shared constants.

- **IceCreamCompany.Infrastructure**  
  Implements data access using the Repository Pattern and provides the EF Core DbContext. It also manages external API integrations and caching (Redis).

- **IceCreamCompany.Tests**  
  Contains unit and integration tests using **xUnit**, **Moq**, and related frameworks. A `TestDbContextFactory` is provided for in-memory testing.
