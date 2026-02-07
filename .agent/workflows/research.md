---
description: Research complex problems, explore libraries, and design solutions
triggers: [manual]
autonomy: supervised
outputs: [documentation, prototypes]
---

# Research Workflow

## Purpose

Delegate complex problem-solving and exploration tasks.
- Investigate best libraries for a specific need
- Solve difficult algorithmic or logic problems
- Prototype potential solutions before implementation
- Compare multiple approaches (Trade-off Analysis)

## Prerequisites

- Clear problem statement or research question
- Access to internet (via search_web tool) and codebase

## Steps

### 1. Problem Analysis & Decomposition
// turbo
Break down the research goal:

1. **Clarify Objectives**: What exactly needs to be solved?
2. **Context**: How does this fit into the current Boolk architecture?
3. **Constraints**: Performance, cost (Firebase), complexity, time.

### 2. Information Gathering
// turbo
Execute searches and documentation reviews:

- Search for relevant libraries or patterns (NuGet, GitHub, Docs)
- Review existing codebase for similar implementations
- Read official documentation (Microsoft, Firebase)

### 3. Alternative Expansion
// turbo
Brainstorm at least 3 distinct approaches:

| Approach | Description | Pros | Cons |
|----------|-------------|------|------|
| **A** | Standard/Safe | Reliable, well-documented | May be slow/heavy |
| **B** | Performance-Optimized | Fast, efficient | Complex, harder to maintain |
| **C** | Innovative/New | Modern, sleek | Experimental, risky |

### 4. Prototyping (Optional)
// turbo
Create small, isolated prototypes to validate assumptions:
- Create a temporary test file or console app snippet
- Verify API behavior
- Benchmark critical paths

### 5. Recommendation & Plan
// turbo
Synthesize findings into a clear recommendation:

1. **Recommended Approach**: Which option is best and why?
2. **Integration Plan**: How to add this to Boolk without breaking existing code.
3. **Risk Mitigation**: How to handle identified downsides.

### 6. Generate Research Artifact
// turbo
Create a research document at `.agent/research/{topic}.md`:

```markdown
# Research: {Topic}

## Executive Summary
{Brief recommendation}

## Options Analysis
{Trade-off table}

## Detailed Recommendation
{Code snippets, diagrams, rationale}

## prototype.cs (if applicable)
{Validated code snippet}
```

## Outputs

- `.agent/research/{topic}.md` - Detailed research findings

## Success Criteria

- Clear recommendation provided
- Trade-offs explicitly analyzed
- Solution verified against constraints
