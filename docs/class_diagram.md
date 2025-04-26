# Class Diagrams

This document contains class diagrams for the components of TheChest.Inventory project.

## Inventories
Inventories are the facade classes that will be interacted and 


### Inventory Diagram
The `Inventory` diagram represents a container that holds and manages items in slots.

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
<<abstract>> Container

namespace TheChest.Inventories {
    class IInventory~T~ {
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
        + Inventory(IInventorySlot~T~[] slots)
        + IInventorySlot~T~ this[int index]
        + IInventorySlot~T~[] Slots
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
}

<<interface>> IInventorySlot
<<interface>> IInventory

Inventory~T~ --|> Container~T~ 
Inventory~T~ ..|> IInventory~T~
Inventory~T~ ..|> IInventorySlot~T~
```

### IStackInventory Diagram
The `IStackInventory` diagram represents a generic container that holds and manages items in slots that can hold more than one amount of it.

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
## Slots
