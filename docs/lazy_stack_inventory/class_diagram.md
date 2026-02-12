# Lazy Stack Inventory Diagram
The `LazyStackInventory` class is a generic container that holds and manages items in slots that can hold more than one amount of the same type, but stores only one entity.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction TB

namespace TheChest.Core {
    class StackContainer~T~ {
    }
}
<<abstract>> StackContainer

namespace TheChest.Inventories.Containers {
    class LazyStackInventory~T~ {
        - IInventoryLazyStackSlot~T~[] slots
        + IInventoryLazyStackSlot~T~ this[int index]

        + LazyStackInventory(IInventoryLazyStackSlot~T~[] slots)

	    + ~~event~~ OnGet: LazyStackInventoryGetEventHandler~T~ 
	    + ~~event~~ OnAdd: LazyStackInventoryAddEventHandler~T~
	    + ~~event~~ OnMove: LazyStackInventoryMoveEventHandler~T~ 
	    + ~~event~~ OnReplace: LazyStackInventoryReplaceEventHandler~T~ 

        + bool CanAdd(T item, int amount = 1)
        + bool CanAddAt(T item, int index, int amount = 1)

        + bool Add(T item)
        + int Add(T item, int amount)
        + int AddAt(T item, int index, int amount)

        + T[] Clear()

        + T? Get(int index)
        + T? Get(T item)
        + T[] Get(T item, int amount)
        + T[] Get(int index, int amount)
        + T[] GetAll(T item)
        + T[] GetAll(int index)

        + int GetCount(T item)

        + void Move(int origin, int target)

        + bool CanReplace(T item, int index, int amount)
        + T[] Replace(T item, int index, int amount)
    }
}

namespace TheChest.Inventories.Slots.Interfaces {
    class IInventoryLazyStackSlot~T~ {
        + bool Contains(T item)
        
        + bool CanAdd(T item, int amount = 1)
        + int Add(T item, int amount = 1)

        + bool CanReplace(T item, int amount = 1)
        + T[] Replace(T item, int amount = 1)
        
        + T[] Get(int amount = 1)
        + T[] GetAll()
    }
}
<<interface>> IInventoryLazyStackSlot

namespace TheChest.Inventories.Containers.Interfaces {
    class ILazyStackInventory~T~ {
	    + ~~event~~ OnGet: LazyStackInventoryGetEventHandler~T~ 
	    + ~~event~~ OnAdd: LazyStackInventoryAddEventHandler~T~ 
	    + ~~event~~ OnMove: LazyStackInventoryMoveEventHandler~T~ 
	    + ~~event~~ OnReplace: LazyStackInventoryReplaceEventHandler~T~ 
        
        + T? Get(int index)
        + T? Get(T item)
        + T[] Get(T item, int amount)
        + T[] GetAll(T item)
        + int GetCount(T item)
        
        + T[] Get(int index, int amount)
        + T[] GetAll(int index)
        
        + bool CanAdd(T item)
        + bool CanAdd(T item, int amount)
        + bool CanAddAt(T item, int index, int amount)

        + bool Add(T item)
        + int Add(T item, int amount)
        + int AddAt(T item, int index, int amount)
        
        + void Move(int origin, int target)
        + T[] Clear()
        
        + bool CanReplace(T item, int index, int amount)
        + T[] Replace(T item, int index, int amount)
    }
}
<<interface>> ILazyStackInventory

LazyStackInventory~T~ --|> StackContainer~T~
LazyStackInventory~T~ ..|> ILazyStackInventory~T~
LazyStackInventory~T~ o-- IInventoryLazyStackSlot~T~
```

## Inventory Lazy Stack Slot Diagram
The `InventoryLazyStackSlot` class can hold and manage a single item inside it, but can also hold more than one amount of the same type.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction TB

namespace TheChest.Core {
    class LazyStackSlot~T~ {
    }
}
<<abstract>> LazyStackSlot

namespace TheChest.Inventories {
    class IInventoryLazyStackSlot~T~ {
        + bool CanAdd(T item, int amount = 1)
        + int Add(T item, int amount = 1)
        + bool CanReplace(T item, int amount = 1)
        + T[] Replace(T item, int amount = 1)
        + T[] Get(int amount = 1)
        + T[] GetAll()
        + bool Contains(T item)
    }
    class InventoryLazyStackSlot~T~ {
        + InventoryLazyStackSlot(T? currentItem = default)
        + InventoryLazyStackSlot(T? currentItem = default, int maxStackAmount = 1)
        
        + bool Contains(T item)
        
        + bool CanAdd(T item, int amount = 1)
        + int Add(T item, int amount = 1)
        
        + bool CanReplace(T item, int amount = 1)
        + T[] Replace(T item, int amount = 1)
        
        + T[] Get(int amount = 1)
        + T[] GetAll()
    }
}
<<interface>> IInventoryLazyStackSlot
InventoryLazyStackSlot~T~ --|> LazyStackSlot~T~
InventoryLazyStackSlot~T~ ..|> IInventoryLazyStackSlot~T~
```