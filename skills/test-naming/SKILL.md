---
name: test-naming
description: Repository-specific naming conventions for unit tests, including method names, class state, parameter context, expected results, overload naming, and context ordering.
---

# Test Naming

Use this skill whenever creating or updating unit test names in this repository.

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

#### Examples

* `Add(T item)`
	* No overloads -> `Add_`
	* Has overload -> `AddItem_`
* `Add(int index)`
	* No overloads -> `Add_`
	* Has overload -> `AddAtIndex_`
* `Get(int index)`
	* No overloads -> `Get_`
	* Has overload -> `GetByIndex_`
* `Get(T item)`
	* No overloads -> `Get_`
	* Has overload -> `GetItem_`
* `Move(int origin, int target)`
	* No overloads -> `Move_`
	* Has overload -> `MoveByIndex_`

---
### Class Context/State

* Optional, but must be present when `ParamContext` is omitted.
* Describes the relevant state or condition of the class under test.
* Use this when the test's behavior is primarily determined by the current state of the class.
* Prefer concise descriptions that identify the meaningful state rather than implementation details.

#### Examples

* `Add_EmptySlot_AddsItem`
  * Class Context: `EmptySlot` _(The slot has no items)_
* `Add_FullSlot_ThrowsInvalidOperationException`
  * Class Context: `FullSlot` _(The slot has reached its maximum capacity)_
* `Get_EmptyContainer_ReturnsEmptyArray`
  * Class Context: `EmptyContainer` _(The container has no items to get)_
* `Move_FullContainer_SwapsItemsFromSlots`
  * Class Context: `FullContainer` _(The container is fully occupied and all slots contains items)_
  
---
### ParamContext

* Optional, but must be present when `Class_Context/State` is omitted.
* Describes the relevant context or values of the method parameters.
* Use this when the parameter values or conditions are what distinguish the test case.
* Describe the meaning of the parameter value rather than necessarily reproducing the literal value.

#### Examples

* `Add_NullItem_ThrowsArgumentNullException`
    Param Context: `NullItem` (The item parameter is null, representing missing input)
* `Add_DefaultValueParam_AddsToContent`
    Param Context: `DefaultValueParam` (The parameter uses a default or uninitialized value)
* `Move_NegativeOrigin_ThrowsArgumentOutOfRangeException`
    Param Context: `NegativeOrigin` (The origin index is below zero, which is invalid)
* `Move_InvalidTarget_ThrowsArgumentOutOfRangeException`
    Param Context: `InvalidTarget` (The target index is outside the valid range)
* `Get_ExistingItem_ReturnsItem`
    Param Context: `ExistingItem` (The requested item exists in the collection)
* `Get_NotFoundItem_ReturnsDefault`
    Param Context: `NotFoundItem` (The requested item does not exist in the collection)
    
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

##### Examples

* `Add_NullItem_ThrowsArgumentNullException`
    * Expected Result: `ThrowsArgumentNullException`
* `Get_InvalidIndex_ThrowsArgumentOutOfRangeException`
    * Expected Result: `ThrowsArgumentOutOfRangeException`
* `Replace_EmptySlot_ThrowsInvalidOperationException`
    * Expected Result: `ThrowsInvalidOperationException`
* `Move_SameOriginAndTarget_ThrowsArgumentException`
    * Expected Result: `ThrowsArgumentException` 

#### State Change Results

Describe the resulting state or behavior when the method changes the state of the object.
Prefer the resulting behavior over implementation details.

##### Examples

* `Get_ExistingItem_RemovesItem`
    * Expected Result: `RemovesItem`
    * Preferred over `RemovesFromContentField`
* `Clear_FullContainer_ClearsAllSlots`
    * Expected Result: `ClearsAllSlots` 
    * Preferred over `ClearsContentField`
* `Add_EmptySlot_AddsItem`
    * Expected Result: `AddsItem`
    * Preferred over `SetsContentField`
* `Move_ValidOriginAndTarget_SwapsItemsFromSlots`
    * Expected Result: `SwapsItemsFromSlots`
    * Preferred over `SwapsContentFields`
* `Add_EmptySlot_IncreasesSize`
    * Expected Result: `IncreasesSize`
    * Preferred over `IncrementsSizeField`
* `Remove_ExistingItem_DecreasesSize`
    * Expected Result: `DecreasesSize`
    * Preferred over `DecrementsSizeField`

#### Return Value Results

Describe the semantic meaning of the returned value.

Use a descriptive result such as `ReturnsItem` or `ReturnsDefault` when the actual value has meaningful semantics. 
Use `ReturnsTrue` or `ReturnsFalse` when the boolean result itself is the relevant outcome.

##### Examples

* `Contains_ExistingItem_ReturnsTrue`
* `Contains_NotFoundItem_ReturnsFalse`
* `CanAdd_FullSlot_ReturnsFalse`
* `CanMove_EmptyTarget_ReturnsTrue`

#### Event and Callback Results

Describe whether the expected event or callback was invoked.
Use `Calls[EventOrCallback]` and `DoesntCall[EventOrCallback]` consistently.

##### Examples

* `Add_EmptySlot_CallsOnAddEvent`
* `Add_FullSlot_CallsOnAdd`
* `Get_FoundItem_CallsOnGetEvent`
* `Move_ValidOriginAndTarget_CallsOnMoveEvent`

#### Negative Results

When the important outcome is that something does **not** happen, use `Doesnt[Behavior]`.
The negative result should identify the behavior that is expected not to occur. Avoid vague names such as `DoesntFail` or `NoChange` unless the absence of any state change is itself the specific contract being tested.

##### Examples

* `Add_SlotCannotAdd_DoesntAddToSlot`
* `Add_SlotCannotAdd_DoesntCallOnAdd`
* `Get_FoundItem_DoesntCallOnGetEvent`
* `Move_InvalidOrigin_DoesntChangeSlots`

---
## Combining Context and State

`Class_Context/State` and `ParamContext` can be used independently or together.

At least one of them must be present. They **must not both be omitted**.

When both contexts are relevant, `Class_Context/State` comes before `ParamContext`.

### Valid forms

* `[MethodName]_[Class_Context/State]_[ExpectedResult]` 
* `[MethodName]_[ParamContext]_[ExpectedResult]`
* `[MethodName]_[Class_Context/State]_[ParamContext]_[ExpectedResult]`

### Examples

* `Add_EmptySlot_DefaultValueParam_AddsToContent`
    * Class Context: `EmptySlot` (The slot is empty and ready to receive new data)
    * Param Context: `DefaultValueParam` (The input value is a default or uninitialized value)
* `Add_SlotWithSameItem_ExistingItemParam_StacksItem`
    * Class Context: `SlotWithSameItem` (The slot already contains the same item type)
    * Param Context: `ExistingItemParam` (The provided item matches the existing item type)
* `Add_SlotWithNotEnoughSpace_LargeAmount_ThrowsInvalidOperationException`
    * Class Context: `SlotWithNotEnoughSpace` (The slot has limited remaining capacity)
    * Param Context: `LargeAmount` (The requested amount exceeds available space)
* `Move_SlotWithItems_NegativeOrigin_ThrowsArgumentOutOfRangeException`
    * Class Context: `SlotWithItems` (The slot contains movable items)
    * Param Context: `NegativeOrigin` (The origin index is invalid because it is negative)
* `Get_FullContainer_ExistingItem_ReturnsItem`
    * Class Context: `FullContainer` (The container is fully populated with items)
    * Param Context: `ExistingItem` (The item being removed is present in the container)

Do not add context segments merely to make the test name longer. Each segment should communicate information necessary to distinguish or understand the test case.
