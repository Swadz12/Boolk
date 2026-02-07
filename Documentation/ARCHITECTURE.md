# Boolk Architecture Documentation

## Overview

Boolk is a restaurant ranking system built with **Blazor Server** and **.NET 8**, following **Clean Architecture** principles. The system allows users to view restaurants rooted in a Firebase Firestore database, submit reviews, and view dynamic rankings based on different strategies.

## Architectural Patterns

The solution is divided into strictly defined layers to ensure separation of concerns and testability:

### 1. Domain Layer (`Boolk.Domain`)
*   **Role**: The core of the application. Contains enterprise logic and entities.
*   **Dependencies**: None.
*   **Key Components**:
    *   **Entities**: `RestaurantBase`, `Kebab`, `Pizza`, `Review`, `User`.
    *   **Factories**: `RestaurantFactory` for creating restaurant instances.

### 2. Application Layer (`Boolk.Application`)
*   **Role**: Defines use cases and application logic.
*   **Dependencies**: `Boolk.Domain`.
*   **Key Components**:
    *   **Interfaces**: `IRestaurantService`, `IReviewService`, `IRankingService`.
    *   **DTOs**: Data Transfer Objects for API communication.
    *   **Strategies**: `IRankingStrategy` and concrete implementations (`BestValue`, `Cheapest`, `MostFilling`).

### 3. Infrastructure Layer (`Boolk.Infrastructure`)
*   **Role**: Implements interfaces defined in Application layer and manages external concerns (Database, Auth).
*   **Dependencies**: `Boolk.Application`, `Boolk.Domain`.
*   **Key Components**:
    *   **Persistence**: `FirebaseUnitOfWork`, `RestaurantRepository`, `ReviewRepository`.
    *   **Services**: `RankingService`, `AuthService`.
    *   **External Libs**: `Google.Cloud.Firestore`.

### 4. API Layer (`Boolk.API`)
*   **Role**: Exposes Application logic via HTTP endpoints (REST API).
*   **Dependencies**: `Boolk.Application`, `Boolk.Infrastructure`.

### 5. Client Layer (`Boolk.Client`)
*   **Role**: The UI layer built with Blazor Server.
*   **Dependencies**: `Boolk.Application` (via HTTP Client proxies).

## Design Patterns Implemented

### 🏭 Factory Pattern
Used to centralize the creation of restaurant objects.
*   **Implementation**: `Boolk.Domain.Factories.RestaurantFactory`
*   **Purpose**: Decouples the creation logic from the consumer, allowing for flexible restaurant type instantiation (e.g., creating a `Kebab` or `Pizza` restaurant).

### ♟️ Strategy Pattern
Used to interchange ranking algorithms at runtime.
*   **Interface**: `Boolk.Application.Ranking.IRankingStrategy`
*   **Context**: `Boolk.Infrastructure.Services.RankingService`
*   **Strategies**:
    *   `BestValueStrategy`: Calculates score based on Satiety / Price.
    *   `CheapestStrategy`: Ranks by lowest average price.
    *   `MostFillingStrategy`: Ranks by highest average satiety.

### 📦 Repository Pattern
Used to abstract data access logic.
*   **Interfaces**: `IRestaurantRepository`, `IReviewRepository`.
*   **Implementation**: Firebase-based repositories in Infrastructure.
*   **Purpose**: Decouples business logic from specific data access technology (Firestore).

### ⚡ Singleton Pattern
Used for stateless service components that are expensive to create or need to be shared.
*   **Instances**: `RestaurantFactory`, `FirestoreDb` (Firebase Connection).

## Unit of Work
The `IUnitOfWork` interface manages valid transactions across multiple repositories, ensuring data consistency (though Firestore has its own atomic operations, this abstraction standardizes access).
