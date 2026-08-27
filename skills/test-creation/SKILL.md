---
name: test-creation
description: Repository-specific guidance for generating and organizing unit tests. Use this whenever writing, adding, or modifying test methods or test files in this repository, including adding coverage for a new or changed method, filling in an empty test partial class, or reorganizing existing tests, even if the user doesn't explicitly say "test-creation".
---

# Test Creation

Use this skill whenever creating or updating tests in this repository. For how to name the tests you write, also follow `skills/test-naming/SKILL.md`.

## Detroit-school conventions

* **No mocks by default.** Use factory classes to instantiate the class under test and its dependencies. Mocks couple tests to implementation details; factories keep tests focused on observable behavior, which is what makes them survive refactors.
* One assertion per test, or one logical outcome. This keeps failures easy to diagnose: a red test tells you exactly what broke instead of forcing you to dig through an assertion chain.
* Tests should be deterministic and independent of each other, so a failure always points to a real regression rather than execution order or leftover state from another test.
* Use **Arrange-Act-Assert** structure in every test.
* Do not add comments to tests. The test name (see `skills/test-naming/SKILL.md`) should already say what the test verifies; a comment restating that just goes stale as the code evolves.

## Tests

Every test in this repository should:

* Inherit `BaseTest<T>` and inject its factories, so item creation and randomization stay consistent across the suite.
* Use randomized data when possible, while ensuring that the test remains deterministic. Randomized values catch bugs that a single hardcoded example would miss, while determinism keeps the test reliable across runs.
* Follow the order of execution of the code under test, so the test file reads as a walkthrough of the method rather than an arbitrary list.

  * Example method:

    ```csharp
    public virtual bool AddAt(T item, int index)
    {
      if (item.IsNull())
        throw new ArgumentNullException(nameof(item));
      if (index < 0 || index >= this.Size)
        throw new ArgumentOutOfRangeException(nameof(index));

      this.slots[index].Add(item);
      this.OnAdd?.Invoke(this, (new[] { item }, index));

      return true;
    }
    ```
  * Unit tests should be organized according to the execution order of the method:
    1. Tests for null or otherwise invalid arguments.
    2. Tests for invalid argument ranges or values.
    3. Tests for failures caused by the current state of dependencies.
    4. Tests verifying that failed operations do not modify state.
    5. Tests verifying that failed operations do not trigger events or callbacks.
    6. Tests for successful operations and resulting state changes.
    7. Tests for events or callbacks triggered by successful operations.
    8. Tests for the returned value of successful operations.
* Use `[TestFixture(typeof([T]))]` for the following types, so behavior is verified across reference types, enums, and structs alike:
  * `TestItem`
  * `TestEnumItem`
  * `TestStructItem`
* If the test throws `InvalidOperationException`, it must test that the item was not added to the slot and that the `OnAdd` event was not called, since this exception represents a state-based failure where nothing should have changed. 
  * Other exceptions only need to test that the exception was thrown.
* Use `[IgnoreIfReferenceType]` for tests that only apply to reference types, and `[IgnoreIfValueType]` for tests that only apply to value types. 
  * This ensures that the test suite runs cleanly across all three types of `T`.

## Class organization
* Test classes are organized by methods in partial classes.
  * Example:
    * `Container<T>.Add` -> `ContainerTests.Add.cs`
    * `Container<T>.Get` -> `ContainerTests.Get.cs`
* If the method has an overload, separate each overload into a different partial class file.
  * Example:
    * `Container<T>.Get(int index)` -> `ContainerTests.GetByIndex.cs`
    * `Container<T>.Get(T item)` -> `ContainerTests.GetItem.cs`
* If an interface-level test class (e.g. `IInventoryStackSlotTests<T>`) already covers a method's behavior in full, the corresponding concrete partial file (e.g. `InventoryStackSlotTests.xxx.cs`) can stay empty. Only add tests to the concrete file when it verifies something the interface tests don't, such as behavior specific to that implementation.

## Common namespace and extensions

* Common classes must always be inside namespace `TheChest.Tests.Common`.
* Extension methods used only by tests should be `internal` and in namespace `TheChest.Tests.Common.Extensions`.
