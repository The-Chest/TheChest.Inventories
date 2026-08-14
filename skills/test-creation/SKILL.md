---
name: test-creation
description: Repository-specific guidance for generating and organizing unit tests using BaseTests<T>, factories, fixture types, naming conventions, and partial-class file structure.
---

# Test Creation

Use this skill whenever creating or updating tests in this repository.

## Detroit-school conventions

* **No mocks by default.** Use factory classes to instantiate the class under test and its dependencies.
* One assertion per test, or one logical outcome.
* Tests should be deterministic and independent of each other.
* Use **Arrange-Act-Assert** structure in every test.
* Do not add comments to tests. The test name should be descriptive enough to explain the test case.

## Tests

* Tests should ALWAYS:

  * Inherit `BaseTest<T>` and inject its factories.
  * Use randomized data when possible, while ensuring that the test remains deterministic.
  * Follow the order of execution of the code.

    * Example method:

    ```csharp
    public virtual bool AddAt(T item, int index)
    {
      if (item.IsNull())
        throw new ArgumentNullException(nameof(item));
      if (index > this.Size || index < 0)
        throw new ArgumentOutOfRangeException(nameof(index));

      this.slots[index].Add(item);
      this.OnAdd?.Invoke(this, (new[] { item }, index));

      return added;
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
  * Use `[TestFixture(typeof([T]))]` for the following types:
    * `TestItem`
    * `TestEnumItem`
    * `TestStructItem`
* If the test throws `InvalidOperationException`, it must test that the item was not added to the slot and that the `OnAdd` event was not called. Other exceptions only need to test that the exception was thrown.

## Class organization
* Test classes are organized by methods in partial classes.
  * Example:
    * `Container<T>.Add` -> `ContainerTests.Add.cs`
    * `Container<T>.Get` -> `ContainerTests.Get.cs`
* If the method has an overload, separate each overload into a different partial class file.
  * Example:
    * `Container<T>.Get(int index)` -> `ContainerTests.GetByIndex.cs`
    * `Container<T>.Get(T item)` -> `ContainerTests.GetItem.cs`

## Common namespace and extensions

* Common classes must always be inside namespace `TheChest.Tests.Common`.
* Extension methods used only by tests should be `internal` and in namespace `TheChest.Tests.Common.Extensions`.
