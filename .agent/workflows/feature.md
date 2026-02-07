---
description: Human-in-the-loop feature planning and implementation
triggers: [manual]
autonomy: hitl
outputs: [documentation, code]
gates: [design_approval, implementation_approval]
---

# Feature Planning Workflow

## Purpose

Collaborative feature development with user control. This workflow ensures:
- User approves design before any implementation
- User approves implementation plan before coding
- No autonomous feature additions

## Prerequisites

- Clear feature request from user
- Understanding of current architecture

## Gate Model

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Discovery  │────▶│   Design    │────▶│   Review    │
│   & Impact  │     │  Document   │     │  GATE #1    │
└─────────────┘     └─────────────┘     └──────┬──────┘
                                               │
                                    User Approves Design
                                               │
                                               ▼
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Review    │◀────│Implementation│◀────│   Plan      │
│  GATE #2    │     │    Plan      │     │ Creation    │
└──────┬──────┘     └─────────────┘     └─────────────┘
       │
User Approves Plan
       │
       ▼
┌─────────────┐     ┌─────────────┐
│   Execute   │────▶│   Verify    │
│   & Test    │     │  & Report   │
└─────────────┘     └─────────────┘
```

## Steps

### Phase 1: Discovery & Impact Analysis

1. **Understand the Feature Request**
   - Clarify requirements with user
   - Identify acceptance criteria
   - Document constraints

2. **Analyze Codebase Impact**
   - Which layers are affected (Models, Repositories, Services, Pages)?
   - Which design patterns need extension?
   - What new files are needed?
   - What existing files need modification?

3. **Identify Dependencies**
   - New NuGet packages required?
   - Firebase schema changes?
   - External API integrations?

### Phase 2: Design Document

4. **Create Feature Specification**
   
   Create document at `.agent/features/{feature-name}.md`:

   ```markdown
   # Feature: {Feature Name}
   
   ## Overview
   Brief description of what this feature does.
   
   ## User Stories
   - As a {user}, I want {action} so that {benefit}
   
   ## Acceptance Criteria
   - [ ] Criterion 1
   - [ ] Criterion 2
   
   ## Technical Design
   
   ### Affected Components
   | Component | Change Type | Description |
   |-----------|-------------|-------------|
   | ... | New/Modify | ... |
   
   ### Design Pattern Usage
   Explain how existing patterns (Factory, Strategy, etc.) will be extended.
   
   ### Data Model Changes
   Any new models or Firebase schema changes.
   
   ### UI Components
   New or modified Blazor components.
   
   ## Implementation Approach
   High-level steps for implementation.
   
   ## Risks & Considerations
   - Risk 1 and mitigation
   ```

### 🚦 GATE #1: Design Approval

5. **Request User Review**
   
   Present the feature specification to user:
   - Summarize impact
   - Highlight any breaking changes
   - Wait for explicit approval before proceeding
   
   **DO NOT PROCEED WITHOUT USER APPROVAL**

### Phase 3: Implementation Planning

6. **Create Detailed Implementation Plan**
   
   Break down into specific, ordered tasks:

   ```markdown
   ## Implementation Checklist
   
   ### 1. Data Layer
   - [ ] Create/modify model classes
   - [ ] Add repository interface methods
   - [ ] Implement Firebase repository
   
   ### 2. Business Layer  
   - [ ] Add service methods
   - [ ] Implement business logic
   - [ ] Add validation
   
   ### 3. Presentation Layer
   - [ ] Create Blazor components
   - [ ] Add routing
   - [ ] Implement UI logic
   
   ### 4. Testing
   - [ ] Unit tests for new logic
   - [ ] Integration tests for repository
   - [ ] Component tests for UI
   ```

### 🚦 GATE #2: Implementation Approval

7. **Request Implementation Approval**
   
   Present the implementation plan to user:
   - Show exact files to be created/modified
   - Estimate complexity
   - Wait for explicit approval
   
   **DO NOT CODE WITHOUT "APPROVED" FROM USER**

### Phase 4: Execution

8. **Interactive Implementation**
   
   Follow the approved implementation plan with **STRICT MANUL APPROVAL** for each file:
   
   **For every file modification:**
   1. Prepare the exact code changes (diff or full content).
   2. **STOP** and present the changes to the user.
   3. Ask: "Do you accept these changes for [Filename]?"
   4. **WAIT** for explicit user approval (e.g., "Yes", "Accept", "Next").
   5. Only apply the changes (`replace_file_content` / `write_to_file`) **AFTER** approval.
   
   *Note: Using `notify_user` or waiting for chat response is required for each file or logical batch.*

   - Commit with tagged messages: `[FEATURE:{name}] {description}`
   - Follow existing code patterns
   - Add appropriate error handling
   - Include logging where relevant

9. **Generate Tests**
   
   Invoke `/test` workflow for new code:
   - Unit tests for all new public methods
   - Integration tests for new repository methods
   - Component tests for new UI elements

### Phase 5: Verification

10. **Validate Implementation**
    - Ensure all acceptance criteria are met
    - Run full test suite
    - Verify no regressions
    - Update documentation

11. **Final Report**
    
    Update feature document with:
    - Implementation status
    - Files created/modified
    - Test coverage for new code
    - Any deviations from plan

## Outputs

- `.agent/features/{feature-name}.md` - Feature specification and status
- Modified/new source files as per implementation plan
- New test files for the feature

## Success Criteria

- Both gates passed with explicit user approval
- All acceptance criteria marked as complete
- Tests pass for new functionality
- Documentation updated
- No regressions in existing functionality
