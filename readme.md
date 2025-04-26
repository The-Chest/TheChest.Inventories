# TheChest.Inventories

TheChest.Inventories is a library designed to manage inventories and slots for generic item collections. It provides a flexible and extensible framework for handling inventory systems, including support for stacked items, slot-based management.

## How does it work
//TODO

### Features

- **Generic Inventory Management**: Supports generic item types for flexibility.
- **Slot-Based System**: Items are stored and managed in slots, with interfaces and implementations for inventory slots.
- **Stackable Items**: Includes support for stackable items with `StackInventory` and `InventoryStackSlot`.
- **Extensible Interfaces**: Interfaces like `IInventory<T>` and `IInventorySlot<T>` allow for custom implementations.
- **Core Operations**: Add, remove, move, and retrieve items from inventories and slots.
- **Error Handling**: Includes exception handling for invalid operations, such as adding items to full slots or accessing invalid indices.

## Project Structure

### Containers
- **`Inventory<T>`**: A generic inventory implementation that manages items in slots.
- **`StackInventory<T>`**: Extends `Inventory<T>` to support stackable items.
- **Interfaces**: Defines contracts for inventory behavior, such as `IInventory<T>` and `IStackInventory<T>`.

### Slots
- **`InventorySlot<T>`**: Represents a single slot in the inventory.
- **`InventoryStackSlot<T>`**: Extends `InventorySlot<T>` to support stacked items.
- **Interfaces**: Defines contracts for slots behaviors, such as `IInventorySlot<T>` and `IInventoryStackSlot<T>`.

The class diagrams can be found in the [docs/class_diagram](/docs/class_diagram.md) file.

## How to use it

### Installation

#### Prerequisites
* .NET 6.0 or later

#### NuGet
To install the library via NuGet, you need to add a NuGet package origin reference to the following URL:
```
https://nuget.pkg.github.com/The-Chest/index.json
```
After adding the reference, use the following command to install the package:
```bash
nuget install TheChest.Core
```

#### DLL
Alternatively, you can download the DLL file and reference it directly in your project.

### Using the bult-in classes
The library provides ready-to-use implementations such as `Inventory<T>` and `InventorySlot<T>`. These can be used directly. For example:
#### Inventory
```csharp
var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

var inventory = new Inventory<string>(slots);
inventory.Add("Item1");
```
#### StackInventory
```csharp
var stackSlots = new IInventoryStackSlot<string>[10];
for (int i = 0; i < stackSlots.Length; i++)
{
    stackSlots[i] = new InventoryStackSlot<string>(new string[0], 5);
}

var stackInventory = new StackInventory<string>(stackSlots);
stackInventory.Add("StackableItem", "StackableItem");
``` 

### Implementing a custom Inventory