---
name: test-creation-skill
description: Repository-specific guidance for generating and organizing unit tests using BaseTests<T>, factories, fixture types, naming conventions, and partial-class file structure.
---

# Test Creation Skill

Use this skill whenever creating or updating tests in this repository.

## Test rules

### Detroit-school conventions
- **No mocks by default** Use factory classes to instantiate the class under test and its dependencies.
- One assertion per test (or one logical outcome).
- Tests should be deterministic and independent of each other.
- **Arrange-Act-Assert** structure in every test.
- No comments in tests, the test name should be descriptive enough to explain the test case.

### Tests
- Tests should ALWAYS:
  - Inherit `BaseTest<T>` and inject its factories.
  - Use randomized data when possible, but ensure that the test is deterministic.
  - Follow the order of execution of the code.
    - Example method:
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
    - Unit tests should be organized in the following order:
      1. `AddAt_NullItem_ThrowsArgumentNullException`
      2. `AddAt_InvalidIndex_ThrowsArgumentOutOfRangeException`
      3. `AddAt_SlotCannotAdd_ThrowsInvalidOperationException`
      4. `AddAt_SlotCannotAdd_DoesntAddToSlotAtIndex`
      5. `AddAt_SlotCannotAdd_DoesntCallOnAdd`
      6. `AddAt_SlotCanAdd_AddsToSlotAtIndex`
      7. `AddAt_SlotCanAdd_CallsOnAddEvent`
      8. `AddAt_SlotCanAdd_ReturnsNotAddedItems`
  - Use `[TestFixture(typeof([T]))]` for the following types:
    - `TestItem`
    - `TestEnumItem`
    - `TestStructItem`
- If the test throws `InvalidOperatioNException` it must test that the item was not added to the slot and that the `OnAdd` event was not called. Other exceptions only need to test that the exception was thrown.
### Class organization
- Test classes are organized by methods in partial classes.
  - Example:
    - `Container<T>.Add` -> `ContainerTests.Add.cs`
    - `Container<T>.Get` -> `ContainerTests.Get.cs`
- If the method has an overload, separate each overload in different partial class files.
  - Example:
    - `Container<T>.Get(int index)` -> `ContainerTests.GetByIndex.cs`
    - `Container<T>.Get(T item)` -> `ContainerTests.GetItem.cs`

### Naming
- Tests should follow: `[Method]_[Context]_[ExpectedResult]`.
  - Example for `Move(int origin, int target)`:
    - Invalid origin test: `Move_NegativeOrigin_ThrowsArgumentOutOfRangeException`
    - Valid test case: `Move_ValidOriginAndTarget_SwapsItemsFromSlots`

### Common namespace and extensions
- Common classes must always be inside namespace `TheChest.Tests.Common`.
- Extension methods used only by tests should be `internal` and in namespace `TheChest.Tests.Common.Extensions`.
