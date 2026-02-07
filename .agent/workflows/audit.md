---
description: Proactive code quality audit with Firebase optimization focus
triggers: [proactive, manual]
autonomy: supervised
outputs: [reports]
---

# Code Quality & Audit Workflow

## Purpose

Continuously monitor code health with focus on:
- C# and Blazor best practices
- Firebase query optimization and packet usage reduction
- Design pattern conformity (Factory, Strategy, Observer, Repository, Facade, Singleton)
- Architecture layer violations

## Prerequisites

- Boolk solution must compile without errors
- Access to all source files in the project

## Steps

### 1. Pattern Conformity Check
// turbo
Analyze codebase for design pattern violations:

```
Patterns to verify:
- Factory: All restaurant types created via RestaurantFactory
- Strategy: Ranking strategies implement IRankingStrategy
- Observer: RankingObserver properly subscribed to RankingService
- Repository: No direct Firebase access outside Repositories/Firebase/
- Facade: External API access goes through RestaurantSystemFacade
- Singleton: RankingService accessed via GetInstance()
```

Report any violations as **warnings**.

### 2. Firebase Optimization Analysis
// turbo
Scan for Firebase inefficiencies:

```
Check for:
- Unbounded queries (missing .Limit())
- N+1 query patterns
- Missing composite indexes
- Redundant reads (same document fetched multiple times)
- Large document reads when only subset needed
- Real-time listeners without proper disposal
```

Calculate estimated packet impact and suggest optimizations.

### 3. C# Best Practices Scan
// turbo
Verify coding standards:

```
Check for:
- Async/await: No .Result or .Wait() blocking calls
- Null handling: Proper use of nullable reference types
- IDisposable: Proper disposal of resources
- Naming: PascalCase for public, _camelCase for private fields
- SOLID violations: Large classes, missing interfaces
```

### 4. Blazor-Specific Checks
// turbo
Analyze Blazor components in Pages/ and Shared/:

```
Check for:
- Component lifecycle issues (improper OnInitializedAsync usage)
- Missing StateHasChanged() calls
- Render optimization (ShouldRender, @key usage)
- Event callback memory leaks
- Large component files (suggest extraction)
```

### 5. Generate Audit Report
// turbo
Create report at `.agent/reports/audit-{timestamp}.md`:

```markdown
# Audit Report - {date}

## Summary
- Pattern Violations: {count}
- Firebase Optimizations: {count}
- Code Quality Issues: {count}
- Blazor Issues: {count}

## Detailed Findings
### ⚠️ Warnings
{list of warnings with file:line references}

### 💡 Optimization Suggestions
{list of Firebase and performance suggestions}

## Metrics
- Files Analyzed: {count}
- Estimated Firebase Packet Reduction: {estimate}
```

## Outputs

- `.agent/reports/audit-{timestamp}.md` - Detailed audit report

## Success Criteria

- All source files analyzed
- Report generated with actionable warnings
- No blocking errors (only warnings)
- Firebase optimization opportunities identified
