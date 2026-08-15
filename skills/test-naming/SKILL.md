---
name: test-naming
description: Repository-specific naming conventions for unit tests, including method names, class state, parameter context, expected results, overload naming, and context ordering.
---

# Test Naming

Use this skill whenever creating or updating unit test names in this repository.

For examples, see `references/examples.md`.

## Naming Convention

Tests must follow this naming pattern:

```text
[MethodName]_[Class_Context/State]_[ParamContext]*_[ExpectedResult]
```

The naming convention is composed of four parts:

1. `MethodName`
2. `Class_Context/State`
3. `ParamContext`
4. `ExpectedResult`

---
### MethodName

* Required.
* Must identify the method being tested.
* Use the method name exactly as it appears in the class under test when there are no overloads.
* When the method has overloads, include the relevant parameter distinction in the test method name.
* The overload distinction should describe the parameter type or semantic role, not the exact C# type syntax.

---
### Class Context/State

* Optional, but must be present when `ParamContext` is omitted.
* Describes the relevant state or condition of the class under test.
* Use this when the test's behavior is primarily determined by the current state of the class.
* Prefer concise descriptions that identify the meaningful state rather than implementation details.

---
### ParamContext

* Optional, but must be present when `Class_Context/State` is omitted.
* Describes the relevant context or values of the method parameters.
* Use this when the parameter values or conditions are what distinguish the test case.
* Describe the meaning of the parameter value rather than necessarily reproducing the literal value.

---
### ExpectedResult

* Required.
* Describes the expected behavior or outcome of the test.
* Prefer behavior-oriented names over implementation-oriented names.
* The expected result should describe what the test verifies, not how the implementation achieves it.
* For exception tests, use `Throws[ExceptionType]`.
* For state changes, describe the resulting state or behavior.
* For return values, describe the semantic meaning of the returned value when possible.
* For events or callbacks, describe the expected invocation.

#### Exception Results

Use `Throws[ExceptionType]` when the primary expected outcome is an exception.
Do not use generic results such as `Fails`, `ThrowsException`, or `ThrowsError` when the specific exception type is part of the contract.

#### State Change Results

Describe the resulting state or behavior when the method changes the state of the object.
Prefer the resulting behavior over implementation details.

#### Return Value Results

Describe the semantic meaning of the returned value.

Use a descriptive result such as `ReturnsItem` or `ReturnsDefault` when the actual value has meaningful semantics.
Use `ReturnsTrue` or `ReturnsFalse` when the boolean result itself is the relevant outcome.

#### Event and Callback Results

Describe whether the expected event or callback was invoked.
Use `Calls[EventOrCallback]` and `DoesntCall[EventOrCallback]` consistently.

#### Negative Results

When the important outcome is that something does **not** happen, use `Doesnt[Behavior]`.
The negative result should identify the behavior that is expected not to occur. Avoid vague names such as `DoesntFail` or `NoChange` unless the absence of any state change is itself the specific contract being tested.

---
## Combining Context and State

`Class_Context/State` and `ParamContext` can be used independently or together.

At least one of them must be present. They **must not both be omitted**.

When both contexts are relevant, `Class_Context/State` comes before `ParamContext`.

### Valid forms

* `[MethodName]_[Class_Context/State]_[ExpectedResult]`
* `[MethodName]_[ParamContext]_[ExpectedResult]`
* `[MethodName]_[Class_Context/State]_[ParamContext]_[ExpectedResult]`

Do not add context segments merely to make the test name longer. Each segment should communicate information necessary to distinguish or understand the test case.
