# Class Diagrams

This document contains class diagrams for the components of TheChest.Inventory project.

## Inventories


### Inventory Diagram
The `Inventory` class is a container that holds and manages items in slots.

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
        class Container~T~ {
        }
	}
	namespace TheChest.Inventories {
        class IInventory~T~ {
	        + ~~event~~ OnGet: InventoryGetEventHandler 
	        + ~~event~~ OnAdd: InventoryAddEventHandler 
            + T? Get(int index)
	        + T[] GetAll(T item)
	        + T[] Clear()
	        + bool Add(T item)
	        + T[] Add(params T[] items)
	        + T? AddAt(T item, int index, bool replace = true)
	        + void Move(int origin, int target)
	        + int GetCount(T item)
        }
        class IInventorySlot~T~ {
	        + bool Add(T item)
	        + T? Get()
	        + bool Contains(T item)
	        + T Replace(T item)
        }
        class Inventory~T~ {
	        - IInventorySlot~T~[] slots
	        + IInventorySlot~T~ this[int index]
	        + IInventorySlot~T~[] Slots
	        + ~~event~~ OnGet: InventoryGetEventHandler 
	        + ~~event~~ OnAdd: InventoryAddEventHandler 
            + ~~event~~ OnMove: InventoryMoveEventHandler 
	        + Inventory(IInventorySlot~T~[] slots)
            + T[] Add(params T[] items)
	        + bool Add(T item)
	        + T? AddAt(T item, int index, bool replace = true)
	        + T[] Clear()
	        + T[] GetAll(T item)
	        + T? Get(int index)
	        + T? Get(T item)
	        + T[] Get(T item, int amount)
	        + int GetCount(T item)
	        + void Move(int origin, int target)
        }
        class IInteractiveContainer~T~{
	        + ~~event~~ OnMove: InventoryMoveEventHandler 
            + void Move(int origin, int target)
            + T[] Clear()
        } 
	}

	<<abstract>> Container
	<<interface>> IInventory
	<<interface>> IInventorySlot
	<<interface>> IInteractiveContainer

    Inventory --|> Container
    Inventory ..|> IInventory
    Inventory ..|> IInteractiveContainer
    IInventorySlot ..* Inventory
```

### Stack Inventory Diagram
The `StackInventory` class is a generic container that holds and manages items in slots that can hold more than one amount of the same type.

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
    namespace TheChest.Core{
        class StackContainer~T~ {
        }
    }
    
    namespace TheChest.Inventories{
        class StackInventory~T~ {
            - IInventoryStackSlot~T~[] slots
            + IInventoryStackSlot~T~ this[int index]
            + IInventoryStackSlot~T~[] Slots
            + StackInventory(IInventoryStackSlot~T~[] slots)
            + bool Add(T item)
            + T[] AddAt(T item, int index, bool replace = true)
            + T[] Add(params T[] items)
            + T[] AddAt(T[] items, int index, bool replace = true)
            + T[] Clear()
            + T[] GetAll(int index)
            + T[] GetAll(T item)
            + T? Get(int index)
            + T? Get(T item)
            + T[] Get(T item, int amount)
            + T[] Get(int index, int amount)
            + int GetCount(T item)
            + void Move(int origin, int target)
        }    
        class IInventoryStackSlot~T~ {
            + bool CanAdd(T item)
            + bool CanAdd(T[] items)
            + void Add(ref T item)
            + void Add(ref T[] items)
            + bool CanReplace(T item)
            + bool CanReplace(T[] items)
            + T[] Replace(ref T[] items)
            + T[] Replace(ref T item)
            + T? Get()
            + T[] Get(int amount)
            + T[] GetAll()
            + bool Contains(T item)
        }
        class IStackInventory~T~ {
            + bool Add(T item)
            + T[] AddAt(T item, int index, bool replace = true)
            + T[] Add(params T[] items)
            + T[] AddAt(T[] items, int index, bool replace = true)
            + T[] Clear()
            + T[] GetAll(int index)
            + T[] GetAll(T item)
            + T? Get(int index)
            + T? Get(T item)
            + T[] Get(T item, int amount)
            + T[] Get(int index, int amount)
            + int GetCount(T item)
            + void Move(int origin, int target)
        }
    }
	<<abstract>> StackContainer
	<<interface>> IStackInventory
	<<interface>> IInventoryStackSlot

    StackInventory ..|> IStackInventory
    StackInventory --> StackContainer
    IInventoryStackSlot --* StackInventory
```

### Lazy Stack Inventory Diagram
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
        + LazyStackInventory(IInventoryLazyStackSlot~T~[] slots)
        + IInventoryLazyStackSlot~T~ this[int index]
        + IInventoryLazyStackSlot~T~[] Slots
        + bool Add(T item)
        + int Add(T item, int amount)
        + T[] AddAt(T item, int index, int amount, bool replace)
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
    }
}

namespace TheChest.Inventories.Slots.Interfaces {
    class IInventoryLazyStackSlot~T~ {
        + bool CanAdd(T item, int amount = 1)
        + int Add(T item, int amount = 1)
        + bool CanReplace(T item, int amount = 1)
        + T[] Replace(T item, int amount = 1)
        + T[] Get(int amount = 1)
        + T[] GetAll()
        + bool Contains(T item)
    }
}
<<interface>> IInventoryLazyStackSlot

namespace TheChest.Inventories.Containers.Interfaces {
    class ILazyStackInventory~T~ {
        + T? Get(int index)
        + T? Get(T item)
        + T[] Get(T item, int amount)
        + T[] GetAll(T item)
        + int GetCount(T item)
        + bool Add(T item)
        + T[] Get(int index, int amount)
        + T[] GetAll(int index)
        + int Add(T item, int amount)
        + int AddAt(T item, int index, int amount)
        + T[] AddAt(T item, int index, int amount, bool replace)
        + void Move(int origin, int target)
        + T[] Clear()
    }
}
<<interface>> ILazyStackInventory

LazyStackInventory~T~ --|> StackContainer~T~
LazyStackInventory~T~ ..|> ILazyStackInventory~T~
LazyStackInventory~T~ o-- IInventoryLazyStackSlot~T~
```

## Slots

### InventorySlot Diagram
The `InventorySlot` class can hold and manage a single item inside it.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction BT

namespace TheChest.Core {
    class Slot~T~ {
        + T? Content
        + bool IsEmpty
        + bool IsFull
        + Slot(T? currentItem = default)
    }
}
<<abstract>> Slot

namespace TheChest.Inventories {
    class IInventorySlot~T~ {
        + bool Add(T item)
        + T? Get()
        + bool Contains(T item)
        + T Replace(T item)
    }
    class InventorySlot~T~ {
        + InventorySlot(T? currentItem = default)
        + bool Add(T item)
        + bool Contains(T item)
        + T? Get()
        + T? Replace(T item)
    }
}

<<interface>> IInventorySlot

InventorySlot~T~ --|> Slot~T~ 
InventorySlot~T~ ..|> IInventorySlot~T~
```

### InventoryStackSlot Diagram
The `InventoryStackSlot` class can hold and manage a list of the same items inside it.

```mermaid
---
config:
  theme: mc
  look: classic
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction BT

namespace TheChest.Core {
    class StackSlot~T~ {
        + T[] Content
        + int MaxStackAmount
        + int StackAmount
        + bool IsEmpty
        + bool IsFull
        + StackSlot(T[] items)
        + StackSlot(T[] items, int maxStackAmount)
    }
}
<<abstract>> StackSlot

namespace TheChest.Inventories {
    class IInventoryStackSlot~T~ {
        + bool CanAdd(T item)
        + bool CanAdd(T[] items)
        + void Add(ref T item)
        + void Add(ref T[] items)
        + bool CanReplace(T item)
        + bool CanReplace(T[] items)
        + T[] Replace(ref T item)
        + T[] Replace(ref T[] items)
        + T? Get()
        + T[] Get(int amount)
        + T[] GetAll()
        + bool Contains(T item)
    }

    class InventoryStackSlot~T~ {
        + InventoryStackSlot(T[] items)
        + InventoryStackSlot(T[] items, int maxStackAmount)
        + void Add(ref T item)
        + void Add(ref T[] items)
        + bool CanAdd(T item)
        + bool CanAdd(T[] items)
        + T[] GetAll()
        + T[] Get(int amount)
        + T? Get()
        + bool CanReplace(T item)
        + bool CanReplace(T[] items)
        + T[] Replace(ref T item)
        + T[] Replace(ref T[] items)
        + bool Contains(T item)
    }
}

<<interface>> IInventoryStackSlot
InventoryStackSlot~T~ --|>  StackSlot~T~
InventoryStackSlot~T~ ..|> IInventoryStackSlot~T~ 
```

### Inventory Lazy Stack Slot Diagram
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
        + T? Content
        + int MaxStackAmount
        + int StackAmount
        + bool IsEmpty
        + bool IsFull
        + LazyStackSlot(T? currentItem = default)
        + LazyStackSlot(T? currentItem = default, int maxStackAmount = 1)
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
        + bool CanAdd(T item, int amount = 1)
        + int Add(T item, int amount = 1)
        + bool CanReplace(T item, int amount = 1)
        + T[] Replace(T item, int amount = 1)
        + T[] Get(int amount = 1)
        + T[] GetAll()
        + bool Contains(T item)
    }
}
<<interface>> IInventoryLazyStackSlot
InventoryLazyStackSlot~T~ --|> LazyStackSlot~T~
InventoryLazyStackSlot~T~ ..|> IInventoryLazyStackSlot~T~
```