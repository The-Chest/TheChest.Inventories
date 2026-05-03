# Test Creation Skill

Use this skill whenever creating or updating tests in this repository.

## Test rules

### Tests
- Tests should ALWAYS:
  - Use factory classes.
  - Inherit `BaseTests<T>` and inject its factories.
  - Follow the order of execution of the code.
    - Example method:
    ```csharp
    public virtual bool AddAt(T item, int index)
    {
      if (item.IsNull())
       throw new ArgumentNullException(nameof(item));
      if (index > this.Size || index < 0)
        throw new ArgumentOutOfRangeException(nameof(index));
      if (!this.slots[index].CanAdd(item))
        throw new InvalidOperationException(StackInventoryErrors.NotPossibleToAddItem);

      this.slots[index].Add(item);
      this.OnAdd?.Invoke(this, (new[] { item }, index));

      return added;
    }
    ```
    - Unit tests should be organized in the following order:
      1. `AddAt_NullItem_ThrowsArgumentNullException`
      2. `AddAt_InvalidIndex_ThrowsArgumentOutOfRangeException`
      3. `AddAt_SlotCannotAdd_ThrowsInvalidOperationException`
      4. `AddAt_SlotCanAdd_AddsToSlotAtIndex`
      5. `AddAt_SlotCanAdd_CallsOnAddEvent`
      6. `AddAt_SlotCanAdd_ReturnsNotAddedItems`
  - Use `[TestFixture(typeof([T]))]` for the following types:
    - `TestItem`
    - `TestEnumItem`
    - `TestStructItem`

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
