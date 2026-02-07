---
description: Architecture maintenance, refactoring proposals, and documentation updates
triggers: [manual]
autonomy: supervised
outputs: [documentation, reports]
---

# Architecture Maintenance Workflow

## Purpose

Maintain project architecture health through:
- Codebase structure analysis
- Refactoring proposals
- Bad architecture decision detection
- Documentation updates (README.md, diagrams)

## Prerequisites

- Access to all source files
- Current README.md and any existing architecture docs

## Steps

### 1. Codebase Structure Analysis
// turbo
Map the current project structure:

```
Analyzed Structure:
├── Models/           # Domain entities
├── Repositories/     # Data access layer
│   ├── Interfaces/   # Repository contracts
│   └── Firebase/     # Firebase implementations
├── Services/         # Business logic layer
├── Factory/          # Object creation
├── RankingEngine/    # Strategy & Observer patterns
│   ├── Strategies/
│   ├── Observers/
│   └── Interfaces/
├── Facade/           # Simplified API surface
├── Firebase/         # Firebase configuration
├── Pages/            # Blazor pages (UI layer)
└── Shared/           # Shared Blazor components
```

Verify layer dependencies follow:
```
Pages/Shared → Facade/Services → Repositories → Firebase
              ↓
         RankingEngine
              ↓
         Factory → Models
```

### 2. Layer Violation Detection
// turbo
Scan for architecture violations:

| Violation Type | Description | Severity |
|---------------|-------------|----------|
| Skip-layer access | Pages directly accessing Repositories | High |
| Circular dependency | Service A → Service B → Service A | High |
| Leaky abstraction | Firebase types in Service interfaces | Medium |
| Missing interface | Concrete class injected instead of interface | Medium |
| God class | Class with too many responsibilities | Medium |

Generate findings with file:line references.

### 3. Design Pattern Compliance Review
// turbo
Verify existing patterns are correctly implemented:

**Factory Pattern**
- All concrete restaurants inherit from RestaurantBase
- RestaurantFactory is the only creator of restaurant instances
- Factory method returns base type, not concrete

**Strategy Pattern**
- All ranking strategies implement common interface
- Context (RankingService) switches strategies at runtime
- Strategies are stateless

**Observer Pattern**
- RankingObserver subscribes to changes properly
- Observers are properly unsubscribed on disposal
- No memory leaks from dangling subscriptions

**Repository Pattern**
- Interfaces define all data operations
- No business logic in repositories
- Repositories are injected, not instantiated directly

**Facade Pattern**
- RestaurantSystemFacade simplifies complex operations
- Facade doesn't expose internal types
- Single entry point for external consumers

**Singleton Pattern**
- RankingService.GetInstance() is thread-safe
- Singleton state is properly managed
- Lazy initialization if applicable

### 4. Technical Debt Assessment
// turbo
Identify areas needing refactoring:

```markdown
## Technical Debt Inventory

### High Priority
- {Issue with impact explanation}

### Medium Priority  
- {Issue with impact explanation}

### Low Priority
- {Issue with impact explanation}

### Suggested Refactorings
1. {Refactoring with before/after}
2. {Refactoring with before/after}
```

### 5. Documentation Audit
// turbo
Review current documentation:

**README.md**
- Is architecture section up to date?
- Are all patterns documented?
- Is project structure accurate?
- Are usage instructions correct?

**Code Comments**
- Are complex algorithms explained?
- Are public APIs documented with XML docs?
- Are design decisions captured?

**Diagrams**
- Is UML diagram (uml_boolk2.png) current?
- Should component diagram be added?

### 6. Generate/Update Documentation
// turbo
Update README.md with:
- Current architecture diagram (Mermaid)
- Accurate project structure
- Pattern usage examples
- Updated feature list

Create/update `ARCHITECTURE.md` with:
- Detailed layer descriptions
- Dependency rules
- Extension guidelines
- Code conventions

### 7. Generate Architecture Report
// turbo
Create report at `.agent/reports/architecture-{timestamp}.md`:

```markdown
# Architecture Report - {date}

## Structure Health
- Layer Violations: {count}
- Pattern Issues: {count}
- Technical Debt Items: {count}

## Findings

### 🔴 Critical Issues
{blocking architectural problems}

### 🟡 Warnings
{non-blocking but important issues}

### 🟢 Strengths
{well-implemented patterns and practices}

## Refactoring Recommendations

### Priority 1
{detailed refactoring proposal}

### Priority 2
{detailed refactoring proposal}

## Documentation Updates Made
- {list of doc changes}

## Suggested Next Steps
1. {actionable recommendation}
2. {actionable recommendation}
```

## Outputs

- `.agent/reports/architecture-{timestamp}.md` - Detailed architecture report
- Updated `README.md` (if changes needed)
- New/updated `ARCHITECTURE.md`
- Updated diagrams if structure changed

## Success Criteria

- All layers analyzed
- All design patterns reviewed
- Technical debt catalogued with priorities
- Documentation is current and accurate
- Clear refactoring recommendations provided
