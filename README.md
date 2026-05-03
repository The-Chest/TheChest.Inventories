# TheChest.Inventories

[![NuGet Version](https://img.shields.io/nuget/v/TheChest.Inventories)](https://www.nuget.org/packages/TheChest.Inventories)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=The-Chest_TheChest.Inventories&metric=coverage)](https://sonarcloud.io/summary/new_code?id=The-Chest_TheChest.Inventories)

TheChest.Inventories is a library for managing inventories and slots in generic item collections. It provides a flexible and extensible framework for inventory systems, with support for stackable items, customizable slots, and operations such as adding, removing, moving, and querying items.

## Key features

- **Generic inventory support**: works with generic item types for maximum flexibility.
- **Slot-based system**: stores items in single-item or stackable slots.
- **Extensible interfaces**: enables custom inventory implementations.
- **Core operations**: add, remove, move, and retrieve items.
- **Stacking support**: handles both simple inventories and stack-based inventories.

## Project structure

### Main components

- `Inventory<T>`
  - Generic inventory implementation using single-item slots.
  - Uses `InventorySlot<T>` to represent each slot.

- `StackInventory<T>`
  - Generic inventory for stackable items.
  - Uses `InventoryStackSlot<T>` to represent slots holding multiple units of the same item.

- `LazyStackInventory<T>`
  - Stackable inventory with lazy item loading.
  - Uses `InventoryLazyStackSlot<T>` to represent slots that can return items on demand.

## Installation

### Via NuGet
Add the NuGet package source:

```bash
nuget source add -n TheChest https://nuget.pkg.github.com/The-Chest/index.json
```

Install the package:

```bash
nuget install TheChest.Inventories
```

### Via DLL
You can also download the DLL directly and reference it in your project.

## Usage examples

### Simple inventory

```csharp
var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

var inventory = new Inventory<string>(slots);
inventory.Add("Item1");
```

### StackInventory

```csharp
var stackSlots = new IInventoryStackSlot<string>[10];
for (int i = 0; i < stackSlots.Length; i++)
{
    stackSlots[i] = new InventoryStackSlot<string>(Array.Empty<string>(), 5);
}

var stackInventory = new StackInventory<string>(stackSlots);
stackInventory.Add("StackableItem", "StackableItem");
```

### LazyStackInventory

```csharp
var lazyStackSlots = new IInventoryLazyStackSlot<string>[10];
for (int i = 0; i < lazyStackSlots.Length; i++)
{
    lazyStackSlots[i] = new InventoryLazyStackSlot<string>($"item_{i}_", 5, 2);
}

var lazyStackInventory = new LazyStackInventory<string>(lazyStackSlots);
lazyStackInventory.Add("StackableItem", "StackableItem");
```

## Additional documentation

More usage and extension details are available in the `docs/` folder.

## Future plans

Future version plans are available on the [GitHub Project Board](https://github.com/orgs/The-Chest/projects/19/views/2).
