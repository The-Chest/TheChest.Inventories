---
name: changelog-write
description: Repository-specific guidance for analyzing and generating `CHANGELOG.md` entries that document API changes, new features, bug fixes, and known issues in a technical, structured, and developer-focused manner.
---

# Changelog Analysis Skill

## Purpose

Analyze an existing `CHANGELOG.md` and generate or update entries using the same writing structure, formatting conventions, tone, and semantic organization already present in the file.

This skill is intended for developer-focused libraries, frameworks, SDKs, engines, or API-centric projects.

---

## Core Principles

The changelog must prioritize:

* API clarity
* technical precision
* navigability
* behavioral descriptions
* semantic grouping

Avoid:

* marketing language
* release hype
* vague summaries
* implementation details
* conversational prose

---

## Structural Conventions

### Section Organization

Always organize changes into semantic sections:

```md
## What's added
## What's changed
## What's fixed
## What's removed
## Known issues
```

Only include sections that contain entries.

Do not create empty sections.

---

## Hierarchical Documentation Pattern

The changelog follows a deeply nested API-oriented hierarchy.

Use this structure:

```text
[Change Category]
 └─ [Affected Type]
     └─ [Feature Group]
         └─ [Method/API]
             └─ [Behavior Summary]
```

Example:

```md
* Validation methods to Inventories
  * `Inventory<T>`
    * Can Add
      * `CanAdd(T item)` - Checks if an item can be added
```

---

## Writing Rules

### 1. API-First Documentation

Document exact API surface changes.

Prefer:

```md
`CanMove(int origin, int target)`
```

Over:

```md
Movement validation improvements
```

Always mention:

* methods
* overloads
* interfaces
* generics
* properties
* events
* classes
* structs
* enums

when relevant.

---

### 2. Present-Tense Descriptions

Use concise present-tense behavioral summaries.

Preferred verbs:

* Checks
* Validates
* Returns
* Adds
* Removes
* Moves
* Replaces
* Creates
* Tries to
* Prevents
* Throws

Examples:

```md
Checks if all items can be added
Returns the available slot index
Prevents invalid stack merging
```

Avoid past tense:

```md
Added validation
Fixed issue where
```

inside item descriptions.

---

### 3. Behavior-Oriented Explanations

Describe external behavior and API contract.

Good:

```md
Checks if all items can be moved from one index to another
```

Bad:

```md
Uses a loop to validate movement
```

Never describe internal implementation unless the change directly affects consumers.

---

### 4. Explicit Type Qualification

Always mention affected concrete types.

Examples:

```md
`Inventory<T>`
`StackInventory<T>`
`LazyStackInventory<T>`
```

Avoid ambiguous references like:

```md
the inventory
the stack implementation
```

---

### 5. Method Signature Formatting

All APIs must use inline code formatting:

```md
`CanAdd(T item)`
```

Never omit parameter lists when documenting methods.

---

### 6. Verb-Based Semantic Categorization

Classify entries according to semantic intent:

| Section | Usage |
|---|---|
| What's added | New APIs or capabilities |
| What's changed | Modified behavior |
| What's fixed | Bug corrections |
| What's removed | Removed APIs or behaviors |
| Known issues | Technical limitations or unresolved problems |

Do not mix categories.

---

## Tone

The tone must remain:

* technical
* neutral
* implementation-focused
* low-abstraction
* non-promotional

Do not use:

* hype
* marketing phrasing
* subjective quality claims
* emotional language

Avoid:

```md
Amazing new system
Powerful feature
Huge improvement
```

Prefer:

```md
Adds support for stack merging
Checks slot compatibility before moving items
```

---

## Formatting Rules

### Bullet Style

Use nested Markdown bullets:

```md
* Feature
  * Type
    * Capability
      * API - Description
```

Maintain consistent indentation.

---

### Exhaustive API Listing

Prefer explicit overload listing instead of grouped summaries.

Preferred:

```md
* `CanAdd(T item)` - Checks if an item can be added
* `CanAdd(T item, int amount)` - Checks if an amount can be added
```

Avoid:

```md
Added multiple CanAdd overloads
```

---

### Feature Group Naming

Feature groups should be concise and semantic.

Examples:

* Can Add
* Can Remove
* Stack Operations
* Validation Methods
* Move Operations

Avoid vague groups:

* Misc
* Utilities
* Improvements

---

## Interpretation Rules

When analyzing git diffs, commits, PRs, or branch changes:

### Additions

Map to:

```md
## What's added
```

if:

* new APIs exist
* new overloads exist
* new interfaces/classes exist
* new behaviors are introduced

---

### Changes

Map to:

```md
## What's changed
```

if:

* behavior changes
* validation logic changes
* exceptions change
* return values change
* semantics evolve

---

### Fixes

Map to:

```md
## What's fixed
```

if:

* bugs are corrected
* invalid behavior is resolved
* edge cases are handled

---

### Removed

Map to:

```md
## What's removed
```

if:

* APIs are deleted
* overloads disappear
* behaviors are deprecated or removed

---

## Generation Constraints

Always:

* preserve existing hierarchy style
* preserve terminology consistency
* preserve naming conventions
* preserve formatting consistency
* preserve Markdown formatting
* preserve existing section order

Never:

* rewrite unrelated entries
* collapse overloads into summaries
* introduce marketing prose
* infer undocumented behavior
* document implementation internals as features

---

## Output Expectations

Generated changelog entries should be:

* highly scannable
* API-oriented
* behavior-focused
* structurally consistent
* developer-centric

The resulting document should resemble:

* API changelogs
* engineering release notes
* SDK patch documentation
* engine/framework release notes

rather than user-facing product updates.