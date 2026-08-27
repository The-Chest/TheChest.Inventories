# Test Naming Examples

Use these examples together with the naming convention in `../SKILL.md`.

## MethodName

| Method signature | No overloads | Has overload |
| --- | --- | --- |
| `Add(T item)` | `Add_` | `AddItem_` |
| `Add(int index)` | `Add_` | `AddAtIndex_` |
| `Get(int index)` | `Get_` | `GetByIndex_` |
| `Get(T item)` | `Get_` | `GetItem_` |
| `Move(int origin, int target)` | `Move_` | `MoveByIndex_` |

## Class Context/State

| Test name | Class context | Meaning |
| --- | --- | --- |
| `Add_EmptySlot_AddsItem` | `EmptySlot` | The slot has no items. |
| `Add_FullSlot_ThrowsInvalidOperationException` | `FullSlot` | The slot has reached its maximum capacity. |
| `Get_EmptyContainer_ReturnsEmptyArray` | `EmptyContainer` | The container has no items to get. |
| `Move_FullContainer_SwapsItemsFromSlots` | `FullContainer` | The container is fully occupied and all slots contain items. |

## ParamContext

| Test name | Param context | Meaning |
| --- | --- | --- |
| `Add_NullItem_ThrowsArgumentNullException` | `NullItem` | The item parameter is null, representing missing input. |
| `Add_DefaultValueParam_AddsToContent` | `DefaultValueParam` | The parameter uses a default or uninitialized value. |
| `Move_NegativeOrigin_ThrowsArgumentOutOfRangeException` | `NegativeOrigin` | The origin index is below zero, which is invalid. |
| `Move_InvalidTarget_ThrowsArgumentOutOfRangeException` | `InvalidTarget` | The target index is outside the valid range. |
| `Get_ExistingItem_ReturnsItem` | `ExistingItem` | The requested item exists in the collection. |
| `Get_NotFoundItem_ReturnsDefault` | `NotFoundItem` | The requested item does not exist in the collection. |

## Exception Results

| Test name | Expected result |
| --- | --- |
| `Add_NullItem_ThrowsArgumentNullException` | `ThrowsArgumentNullException` |
| `Get_InvalidIndex_ThrowsArgumentOutOfRangeException` | `ThrowsArgumentOutOfRangeException` |
| `Replace_EmptySlot_ThrowsInvalidOperationException` | `ThrowsInvalidOperationException` |
| `Move_SameOriginAndTarget_ThrowsArgumentException` | `ThrowsArgumentException` |

## State Change Results

| Test name | Expected result | Preferred over |
| --- | --- | --- |
| `Get_ExistingItem_RemovesItem` | `RemovesItem` | `RemovesFromContentField` |
| `Clear_FullContainer_ClearsAllSlots` | `ClearsAllSlots` | `ClearsContentField` |
| `Add_EmptySlot_AddsItem` | `AddsItem` | `SetsContentField` |
| `Move_ValidOriginAndTarget_SwapsItemsFromSlots` | `SwapsItemsFromSlots` | `SwapsContentFields` |
| `Add_EmptySlot_IncreasesSize` | `IncreasesSize` | `IncrementsSizeField` |
| `Remove_ExistingItem_DecreasesSize` | `DecreasesSize` | `DecrementsSizeField` |

## Return Value Results

| Test name | Expected result |
| --- | --- |
| `Contains_ExistingItem_ReturnsTrue` | `ReturnsTrue` |
| `Contains_NotFoundItem_ReturnsFalse` | `ReturnsFalse` |
| `CanAdd_FullSlot_ReturnsFalse` | `ReturnsFalse` |
| `CanMove_EmptyTarget_ReturnsTrue` | `ReturnsTrue` |

## Event and Callback Results

| Test name | Expected result |
| --- | --- |
| `Add_EmptySlot_CallsOnAddEvent` | `CallsOnAddEvent` |
| `Add_FullSlot_CallsOnAdd` | `CallsOnAdd` |
| `Get_FoundItem_CallsOnGetEvent` | `CallsOnGetEvent` |
| `Move_ValidOriginAndTarget_CallsOnMoveEvent` | `CallsOnMoveEvent` |

## Negative Results

| Test name | Expected result |
| --- | --- |
| `Add_SlotCannotAdd_DoesNotAddToSlot` | `DoesNotAddToSlot` |
| `Add_SlotCannotAdd_DoesNotCallOnAdd` | `DoesNotCallOnAdd` |
| `Get_FoundItem_DoesNotCallOnGetEvent` | `DoesNotCallOnGetEvent` |
| `Move_InvalidOrigin_DoesNotChangeSlots` | `DoesNotChangeSlots` |

## Combining Context and State

| Test name | Class context | Param context |
| --- | --- | --- |
| `Add_EmptySlot_DefaultValueParam_AddsToContent` | `EmptySlot` (The slot is empty and ready to receive new data) | `DefaultValueParam` (The input value is a default or uninitialized value) |
| `Add_SlotWithSameItem_ExistingItemParam_StacksItem` | `SlotWithSameItem` (The slot already contains the same item type) | `ExistingItemParam` (The provided item matches the existing item type) |
| `Add_SlotWithNotEnoughSpace_LargeAmount_ThrowsInvalidOperationException` | `SlotWithNotEnoughSpace` (The slot has limited remaining capacity) | `LargeAmount` (The requested amount exceeds available space) |
| `Move_SlotWithItems_NegativeOrigin_ThrowsArgumentOutOfRangeException` | `SlotWithItems` (The slot contains movable items) | `NegativeOrigin` (The origin index is invalid because it is negative) |
| `Get_FullContainer_ExistingItem_ReturnsItem` | `FullContainer` (The container is fully populated with items) | `ExistingItem` (The item being removed is present in the container) |