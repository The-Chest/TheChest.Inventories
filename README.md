# TheChest.Inventories

[![NuGet Version](https://img.shields.io/nuget/v/TheChest.Inventories)](https://www.nuget.org/packages/TheChest.Inventories)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=The-Chest_TheChest.Inventories&metric=coverage)](https://sonarcloud.io/summary/new_code?id=The-Chest_TheChest.Inventories)

TheChest.Inventories is a library for managing inventories and slots in generic item collections. It provides a flexible and extensible framework for inventory systems, with support for stackable items, event-driven operations, and lazy-loaded inventory management.

## Table of Contents

- [What is Inventory Management?](#what-is-inventory-management)
- [Key Features](#key-features)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Usage Examples](#usage-examples)
  - [Simple Inventory](#simple-inventory)
  - [Stack Inventory](#stack-inventory)
  - [Lazy Stack Inventory](#lazy-stack-inventory)
  - [Working with Events](#working-with-events)
  - [Error Handling](#error-handling)
- [Architecture](#architecture)
- [Advanced Features](#advanced-features)
- [Extension and Customization](#extension-and-customization)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [Additional Resources](#additional-resources)

## What is Inventory Management?

Inventory management refers to the system of tracking and organizing items in collections. This library is designed for scenarios where you need to:

- Store items in slots with defined capacities
- Manage stackable items (multiple units in a single slot)
- Track inventory changes through events
- Implement custom inventory behaviors
- Handle lazy-loaded or on-demand item retrieval

Use cases include game inventory systems, warehouse management, resource pools, and item storage systems.

## Key Features

- **Generic inventory support**: Works with any generic item type for maximum flexibility
- **Slot-based system**: Stores items in single-item or stackable slots
- **Three inventory types**:
  - Standard `Inventory<T>` for single-item slots
  - `StackInventory<T>` for stackable items
  - `LazyStackInventory<T>` for lazy-loaded stackable items
- **Event system**: Comprehensive events for add, remove, move, and replace operations
- **Extensible interfaces**: Enable custom inventory implementations
- **Core operations**: Add, remove, move, and retrieve items with flexible APIs
- **Validation**: Built-in methods to check operations before execution

## Project Structure

### Main Components

- **`Inventory<T>`**
  - Generic inventory implementation using single-item slots
  - Uses `InventorySlot<T>` to represent each slot
  - Best for: Game inventories with single-item slots, fixed-size collections
  - [Learn more](docs/inventory/class_diagram.md)

- **`StackInventory<T>`**
  - Generic inventory for stackable items
  - Uses `InventoryStackSlot<T>` to represent slots holding multiple units of the same item
  - Best for: Resource management, currency systems, consumable items
  - [Learn more](docs/stack_inventory/class_diagram.md)

- **`LazyStackInventory<T>`**
  - Stackable inventory with lazy item loading
  - Uses `InventoryLazyStackSlot<T>` to represent slots that can return items on demand
  - Best for: Large collections, on-demand item generation, performance optimization
  - [Learn more](docs/lazy_stack_inventory/class_diagram.md)

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

## Quick Start

### Basic Setup

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

// Create slots
var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

// Create inventory
var inventory = new Inventory<string>(slots);

// Add an item
if (inventory.CanAdd("Item1"))
{
    inventory.Add("Item1");
}
```

## Usage Examples

### Simple Inventory

A basic inventory for items that occupy one slot each:

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

// Initialize slots
var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

// Create inventory
var inventory = new Inventory<string>(slots);

// Add items
inventory.Add("Sword");
inventory.Add("Shield");

// Retrieve items
var item = inventory.Get(0);

// Check inventory
int count = inventory.GetCount("Sword");
```

### Stack Inventory

Inventory for stackable items (multiple units per slot):

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

// Initialize stack slots with max stack size of 5
var stackSlots = new IInventoryStackSlot<string>[10];
for (int i = 0; i < stackSlots.Length; i++)
{
    stackSlots[i] = new InventoryStackSlot<string>(Array.Empty<string>(), 5);
}

// Create stack inventory
var stackInventory = new StackInventory<string>(stackSlots);

// Add stackable items
stackInventory.Add("Gold");
stackInventory.Add("Gold");
stackInventory.Add("Gold");

// Retrieve multiple items
var retrieved = stackInventory.Get("Gold", 2);
```

### Lazy Stack Inventory

Stackable inventory with lazy item loading:

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

// Initialize lazy stack slots
var lazyStackSlots = new IInventoryLazyStackSlot<string>[10];
for (int i = 0; i < lazyStackSlots.Length; i++)
{
    lazyStackSlots[i] = new InventoryLazyStackSlot<string>($"item_{i}_", 5, 2);
}

// Create lazy stack inventory
var lazyStackInventory = new LazyStackInventory<string>(lazyStackSlots);

// Add items
lazyStackInventory.Add("StackableItem", 3);

// Items are generated on demand
var items = lazyStackInventory.Get(0);
```

### Working with Events

Listen to inventory changes through events:

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

var inventory = new Inventory<string>(slots);

// Subscribe to add events
inventory.OnAdd += (sender, args) =>
{
    foreach (var action in args.Data)
    {
        Console.WriteLine($"Item {action.Item} added to slot {action.Index}");
    }
};

// Subscribe to get events
inventory.OnGet += (sender, args) =>
{
    foreach (var action in args.Data)
    {
        Console.WriteLine($"Item {action.Item} retrieved from slot {action.Index}");
    }
};

// Subscribe to move events
inventory.OnMove += (sender, args) =>
{
    Console.WriteLine($"Item moved from {args.Data.Origin} to {args.Data.Target}");
};

// Subscribe to replace events
inventory.OnReplace += (sender, args) =>
{
    Console.WriteLine($"Item replaced at slot {args.Data.Index}");
};

// Operations will now trigger events
inventory.Add("Sword");
var item = inventory.Get(0);
```

### Error Handling

Always validate operations before executing:

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new IInventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>();
}

var inventory = new Inventory<string>(slots);

// Check if we can add before adding
if (inventory.CanAdd("Item"))
{
    inventory.Add("Item");
}
else
{
    Console.WriteLine("Inventory is full!");
}

// Check if we can move before moving
if (inventory.CanMove(0, 5))
{
    inventory.Move(0, 5);
}
else
{
    Console.WriteLine("Cannot move item to that slot");
}

// Use Try methods for safer operations
if (inventory.TryReplace("NewItem", 0, out var oldItem))
{
    Console.WriteLine($"Replaced {oldItem} with NewItem");
}
else
{
    Console.WriteLine("Failed to replace item");
}
```

## Architecture

### Class Diagrams

- [Inventory Architecture](docs/inventory/class_diagram.md)
- [Stack Inventory Architecture](docs/stack_inventory/class_diagram.md)
- [Lazy Stack Inventory Architecture](docs/lazy_stack_inventory/class_diagram.md)

### Event System

Detailed documentation on the event system for each inventory type:

- [Inventory Events](docs/inventory/events.md)
- [Stack Inventory Events](docs/stack_inventory/events.md)
- [Lazy Stack Inventory Events](docs/lazy_stack_inventory/events.md)

## Advanced Features

### Event Patterns

All inventory types support a comprehensive event system:
- `OnGet` - Fires when items are retrieved
- `OnAdd` - Fires when items are added
- `OnMove` - Fires when items are moved between slots
- `OnReplace` - Fires when items are replaced

See [Inventory Events](docs/inventory/events.md) for detailed examples.

### Stacking and Capacity

Different inventory types handle stacking differently:
- **Inventory<T>**: One item per slot
- **StackInventory<T>**: Multiple items per slot with a defined max stack
- **LazyStackInventory<T>**: Multiple items per slot, loaded on demand

### Performance Considerations

- Use `LazyStackInventory<T>` when working with large collections that don't need to be fully loaded
- Use `StackInventory<T>` for smaller collections with stackable items
- Use `Inventory<T>` for simple, single-item slot scenarios
- Always use `CanX` methods to validate operations before executing them
- Subscribe to events selectively to avoid performance overhead

## Extension and Customization

### Extending Built-in Classes

You can extend the built-in inventory classes to add custom functionality:

- [Extending Inventory](docs/inventory/extending.md)
- [Extending Stack Inventory](docs/stack_inventory/extending.md)

### Implementing Custom Inventories

Create fully custom implementations by implementing the interfaces:

- [Implementing Custom Inventory](docs/inventory/implementing.md)
- [Implementing Custom Stack Inventory](docs/stack_inventory/implementing.md)
- [Implementing Custom Lazy Stack Inventory](docs/lazy_stack_inventory/implementing.md)

### Example Custom Implementation

```csharp
public class MyCustomInventory : Inventory<int>
{
    public MyCustomInventory(IInventorySlot<int>[] slots) : base(slots)
    {
        if (slots.Length != 10)
            throw new ArgumentException("Invalid inventory size");
    }

    public override bool Add(int item)
    {
        if (item <= 0)
            return false;
        
        return base.Add(item);
    }
}
```

## Troubleshooting

### Inventory is Full

If you're getting failures when trying to add items:
- Check inventory capacity with `CanAdd()` before adding
- Use `CanAddAt()` to check specific slots
- Consider using a larger inventory or clearing items

### Items Not Moving

Movement issues usually stem from:
- Destination slot being full
- Source slot being empty
- Using `CanMove()` to validate before moving

```csharp
// Always validate first
if (inventory.CanMove(sourceIndex, targetIndex))
{
    inventory.Move(sourceIndex, targetIndex);
}
```

### Event Handlers Not Firing

Ensure events are subscribed before operations:
```csharp
// Subscribe BEFORE performing operations
inventory.OnAdd += (sender, args) => { /* handle event */ };

// Then perform the operation
inventory.Add("Item");
```

### Stack Size Issues

For `StackInventory<T>`, ensure max stack size is appropriate:
```csharp
// Max stack size must be > 0
var slot = new InventoryStackSlot<string>(items, maxStackAmount: 5);
```

## Contributing

Contributions are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Additional Resources

- [GitHub Issues](https://github.com/The-Chest/TheChest.Inventories/issues)
- [GitHub Project Board](https://github.com/orgs/The-Chest/projects/19/views/2)
- [Detailed Documentation](docs/)
- [TheChest Core Library](https://github.com/The-Chest/TheChest.Core)

## Future Plans

Future version plans are available on the [GitHub Project Board](https://github.com/orgs/The-Chest/projects/19/views/2).
