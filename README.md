# Boolk - Restaurant Ranking System

A Blazor Server application for ranking restaurants based on user reviews using various strategies.

## Architecture

This project implements several design patterns:
- **Factory Pattern**: `RestaurantFactory` for creating different restaurant types
- **Strategy Pattern**: Multiple ranking strategies (BestValue, Cheapest, MostFilling)
- **Observer Pattern**: `RankingObserver` for tracking ranking updates
- **Repository Pattern**: Firebase-based repositories for data access
- **Facade Pattern**: `RestaurantSystemFacade` for simplified API access
- **Singleton Pattern**: `RankingService` as a singleton instance

## Project Structure

```
Boolk/
├── Models/              # Domain models (RestaurantBase, User, Review)
├── Repositories/        # Repository interfaces and Firebase implementations
├── Factory/            # RestaurantFactory
├── RankingEngine/       # Strategy and Observer patterns
├── Services/           # Business logic layer
├── Facade/             # Facade pattern
├── Firebase/           # Firebase configuration
├── Pages/              # Blazor pages
└── Shared/             # Shared Blazor components
```

## Features

- **Restaurant Management**: Add restaurants of different types (FastFood, StudentBar, Premium)
- **Review System**: Add reviews with price, satiety level, and comments
- **Ranking Strategies**:
  - **Best Value**: Ranks by satiety per price ratio
  - **Cheapest**: Ranks by lowest average price
  - **Most Filling**: Ranks by highest average satiety level
- **Real-time Updates**: Observer pattern notifies when rankings are updated

## Usage

1. **Add Restaurants**: Navigate to `/restaurants` and add new restaurants
2. **Add Reviews**: Navigate to `/reviews` and add reviews for restaurants
3. **View Rankings**: Navigate to `/` (home) and select a ranking strategy to see top restaurants