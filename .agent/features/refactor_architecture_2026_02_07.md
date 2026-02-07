# Feature: Architecture Refactor & Scalability Improvements

## Overview
This feature addresses critical architecture violations and scalability issues identified in the "Architecture Report - 2026-02-07". Key improvements include removing skip-layer violations in the UI, implementing pagination for restaurant data, and refactoring the Factory pattern for OCP compliance.

## User Stories
- As a developer, I want the `Restaurants.razor` page to access data via the Facade layer so that business logic is strictly enforced and layers are decoupled.
- As a user, I want the restaurants list to support pagination so that the application remains fast as the number of restaurants grows.
- As a developer, I want the `RestaurantFactory` to be extensible without modification so that I can add new restaurant types easily (OCP).

## Acceptance Criteria
- [ ] `Restaurants.razor` no longer injects `IRestaurantRepository`.
- [ ] `Restaurants.razor` uses `RestaurantSystemFacade` for data access.
- [ ] `IRestaurantRepository` supports `GetAllAsync(int skip, int take)`.
- [ ] `FirebaseRestaurantRepository` implements pagination using Firestore's `Offset` and `Limit`.
- [ ] `RestaurantFactory` uses a specific mapping strategy (Dictionary) instead of a switch statement.
- [ ] Logic for displaying restaurant types (icons/names) is moved out of `Restaurants.razor` into a ViewModel.
- [ ] `RankingService` is registered as a standard Singleton in DI, removing manual `GetInstance` usage.

## Technical Design

### Affected Components
| Component | Change Type | Description |
|-----------|-------------|-------------|
| `Pages/Restaurants.razor` | Modify | Remove Repositories injection; Use Facade; Add Pagination UI; Use ViewModel for display logic. |
| `Facade/RestaurantSystemFacade.cs` | Modify | Add `GetRestaurantsAsync(int page, int pageSize)`. |
| `Services/RestaurantService.cs` | Modify | Helper methods for mapping to ViewModels (if needed). |
| `Repositories/Interfaces/IRestaurantRepository.cs` | Modify | Add `GetAllAsync(int skip, int take)`. |
| `Repositories/Firebase/FirebaseRestaurantRepository.cs` | Modify | Implement pagination. |
| `Factory/RestaurantFactory.cs` | Modify | Refactor to use Dictionary<string, Func<...>>. |
| `ViewModels/RestaurantViewModel.cs` | New | DTO/ViewModel for UI display logic (Icon, DisplayName). |
| `Program.cs` | Modify | Fix RankingService registration |

### Design Pattern Usage
- **Facade Pattern**: Reinforcing usage by forcing UI to go through `RestaurantSystemFacade`.
- **Factory Pattern**: Improving implementation to be Closed for Modification (OCP) using a registration dictionary.
- **ViewModel Pattern**: Decoupling UI logic from data models.

### Data Model Changes
- No schema changes.
- New `RestaurantViewModel` class (not persisted).

### UI Components
- `Restaurants.razor`: Add "Previous" and "Next" buttons for pagination.

## Implementation Approach
1.  **Repository**: Update Interface and Firebase implementation for pagination.
2.  **Factory**: Refactor `RestaurantFactory`.
3.  **Service/Facade**: Expose paginated data and mapping logic.
4.  **ViewModel**: Create `RestaurantViewModel`.
5.  **UI**: Update `Restaurants.razor` to use Facade and new ViewModel, remove Repository dependency.
6.  **Cleanup**: Fix Singleton registration in `Program.cs`.

## Risks & Considerations
- **Ranking**: The ranking calculation currently loads *all* data. This refactor implements pagination for the *list view*, but the ranking calculation (`RankingService`) might still be heavy as it requires full dataset for scoring. This is a known limitation to be addressed in a future "Long-term" refactor.
