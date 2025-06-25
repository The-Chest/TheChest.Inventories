# Events

## Inventory events

### InventoryGetEventHandler

Fires when an item is returned to the inventory.

#### Signature
```csharp
public delegate void InventoryGetEventHandler<T>(object? sender, InventoryGetEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryGetEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryGetItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Found item                                         |
| args.Data[].Index         | `Integer`                                             | Index number where the `Item` was found            |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);
inventory.OnGet += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} got from index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.Get(0);
Console.WriteLine($"Item {result} got from index {0}");
```

### InventoryAddEventHandler

Fires when an item is added to the inventory.

#### Signature
```csharp
public delegate void InventoryAddEventHandler<T>(object? sender, InventoryAddEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryAddEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryAddItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Item added                                         |
| args.Data[].Index         | `Integer`                                             | Index number where the `Item` was added            |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length - 2; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);
inventory.OnAdd += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} added on index {action.Index}");
    }
};

//Any Add method will fire the event (if sucessful)
var result = inventory.Add("Item");
Console.WriteLine($"Item {result} added");
```

### InventoryMoveEventHandler

Fires when an item is moved from an index to another to the inventory.

#### Signature
```csharp
public delegate void InventoryMoveEventHandler<T>(object? sender, InventoryMoveEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                  | Description                                        |
|---------------------------|-------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                              | Inventory responsible for firing the event         |
| args                      | `InventoryMoveEventArgs`                              | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<InventoryMoveItemEventData<T>>`  | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                             | Item that were moved                               |
| args.Data[].FromIndex     | `Integer`                                             | Index number where the `Item` started              |
| args.Data[].ToIndex       | `Integer`                                             | Index number where the `Item` was moved to         |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length - 1; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);

inventory.OnMove += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Item {action.Item} got from index {action.Index}");
    }
};

//This will fire an event with the two items moved
inventory.Move(0, 1);
//This will fire an event with the one item moved (the index 9 is empty)
inventory.Move(9, 2);
```

## Stack Inventory events

### StackInventoryGetEventHandler

Fires when any amount of item is returned from the inventory.

#### Signature
```csharp
public delegate void StackInventoryGetEventHandler<T>(object? sender, StackInventoryGetEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                     | Description                                        |
|---------------------------|----------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                 | Inventory responsible for firing the event         |
| args                      | `StackInventoryGetEventArgs`                             | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<StackInventoryGetItemEventData<T>>` | An array with all items and its respective indexes |
| args.Data[].Items[]       | `Array.Generic`                                          | Found items                                        |
| args.Data[].Index         | `Integer`                                                | Index number where the `Items` were found          |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventoryStackSlot<string>($"Item_{i}", 1, 10);
}

var inventory = new StackInventory<string>(slots);
inventory.OnGet += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Items.Length} Items returned from index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.Get(0, 10);
Console.WriteLine($"{result.Length} Items returned from index {0}");
```

### StackInventoryAddEventHandler

Fires when any amount of item is added to a slot on the inventory.

#### Signature
```csharp
public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                     | Description                                        |
|---------------------------|----------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                 | Inventory responsible for firing the event         |
| args                      | `StackInventoryAddEventArgs`                             | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<StackInventoryAddItemEventData<T>>` | An array with all items and its respective indexes |
| args.Data[].Items[]       | `Array.Generic`                                          | Items added                                        |
| args.Data[].Index         | `Integer`                                                | Index number where the `Items` were added          |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventoryStackSlot<string>();
}

var inventory = new StackInventory<string>(slots);
inventory.OnAdd += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Items.Length} Items added to the slot {action.Index}");
    }
};

var result = inventory.Add("item_1", "item_2");
Console.WriteLine($"{result.Length} Items were not added");
```

### StackInventoryMoveEventHandler

Fires when any amount of item is moved to a slot on the inventory.

#### Signature
```csharp
public delegate void StackInventoryMoveEventHandler<T>(object? sender, StackInventoryMoveEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                      | Description                                          |
|---------------------------|-----------------------------------------------------------|------------------------------------------------------|
| sender                    | `object`                                                  | Inventory responsible for firing the event           |
| args                      | `StackInventoryMoveEventArgs`                             | Class that holds data of the event                   |
| args.Data                 | `IReadOnlyCollection<StackInventoryMoveItemEventData<T>>` | An array with all items and its respective indexes   |
| args.Data[].Items[]       | `Array.Generic`                                           | Items added                                          |
| args.Data[].FromIndex     | `Integer`                                                 | Index number where the `Items` were originally from  |
| args.Data[].ToIndex       | `Integer`                                                 | Index number where the `Items` were moved            |

## Warning
* It fires an event for each item that was moved.
* If the `FromIndex` is the same as the `ToIndex`, it will not fire the event.
* If there is no item in one of the indexes, it will not fire the event for that index.

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventoryStackSlot<string>();
}

var inventory = new StackInventory<string>(slots);
inventory.OnMove += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Items.Length} Items were moved from the slot {action.FromIndex} to the slot {action.ToIndex} ");
    }
};

inventory.Move(0, 1);//the method has no return
```

## LazyStackInventory events

### LazyStackInventoryGetEventHandler

Fires when an item is returned from the inventory.

#### Signature
```csharp
public delegate void LazyStackInventoryGetEventHandler<T>(object? sender, LazyStackInventoryGetEventArgs<T> e);
```

#### EventArgs

| Property                  | Type                                                           | Description                                        |
|---------------------------|----------------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                       | Inventory responsible for firing the event         |
| args                      | `LazyStackInventoryGetEventArgs`                               | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>>`   | An array with all items and its respective indexes |
| args.Data[].Item          | `Generic`                                                      | Found item                                         |
| args.Data[].Index         | `Integer`                                                      | Index number where the `Item` was found            |
| args.Data[].Amount        | `Integer`                                                      | Amount of `Item` found                             |

#### Example

```csharp
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventoryLazyStackSlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}", 5, 10);
}

var inventory = new LazyStackInventory<string>(slots);
inventory.OnGet += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"{action.Amount} amount of the Item {action.Item} returned from index {action.Index}");
    }
};

//Any method except GetCount will fire the event
var result = inventory.Get(0);
Console.WriteLine($"{result.Length} amoung of the Item {result} returned from index {0}");
```