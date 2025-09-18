# Stack Inventory events

## StackInventoryGetEventHandler

Fires when any amount of item is returned from the inventory.

### Signature
```csharp
public delegate void StackInventoryGetEventHandler<T>(object? sender, StackInventoryGetEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                     | Description                                        |
|---------------------------|----------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                 | Inventory responsible for firing the event         |
| args                      | `StackInventoryGetEventArgs`                             | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<StackInventoryGetItemEventData<T>>` | An array with all items and its respective indexes |
| args.Data[].Items[]       | `Array.Generic`                                          | Found items                                        |
| args.Data[].Index         | `Integer`                                                | Index number where the `Items` were found          |

### Example

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

## StackInventoryAddEventHandler

Fires when any amount of item is added to a slot on the inventory.

### Signature
```csharp
public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                     | Description                                        |
|---------------------------|----------------------------------------------------------|----------------------------------------------------|
| sender                    | `object`                                                 | Inventory responsible for firing the event         |
| args                      | `StackInventoryAddEventArgs`                             | Class that holds data of the event                 |
| args.Data                 | `IReadOnlyCollection<StackInventoryAddItemEventData<T>>` | An array with all items and its respective indexes |
| args.Data[].Items[]       | `Array.Generic`                                          | Items added                                        |
| args.Data[].Index         | `Integer`                                                | Index number where the `Items` were added          |

### Example

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

## StackInventoryMoveEventHandler

Fires when any amount of item is moved to a slot on the inventory.

### Signature
```csharp
public delegate void StackInventoryMoveEventHandler<T>(object? sender, StackInventoryMoveEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                      | Description                                          |
|---------------------------|-----------------------------------------------------------|------------------------------------------------------|
| sender                    | `object`                                                  | Inventory responsible for firing the event           |
| args                      | `StackInventoryMoveEventArgs`                             | Class that holds data of the event                   |
| args.Data                 | `IReadOnlyCollection<StackInventoryMoveItemEventData<T>>` | An array with all items and its respective indexes   |
| args.Data[].Items[]       | `Array.Generic`                                           | Items added                                          |
| args.Data[].FromIndex     | `Integer`                                                 | Index number where the `Items` were originally from  |
| args.Data[].ToIndex       | `Integer`                                                 | Index number where the `Items` were moved            |

### Warning
* It fires an event for each item that was moved.
* If the `FromIndex` is the same as the `ToIndex`, it will not fire the event.
* If there is no item in one of the indexes, it will not fire the event for that index.

### Example

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

## StackInventoryReplaceEventHandler

Fires when any amount of item is replaced in a slot on the inventory.

### Signature
```csharp
public delegate void StackInventoryReplaceEventHandler<T>(object? sender, StackInventoryReplaceEventArgs<T> e);
```

### EventArgs

| Property                  | Type                                                      | Description                                               |
|---------------------------|-----------------------------------------------------------|-----------------------------------------------------------|
| sender                    | `object`                                                  | Inventory responsible for firing the event                |
| args                      | `InventoryReplaceEventArgs`                               | Class that holds data of the event                        |
| args.Data                 | `IReadOnlyCollection<InventoryReplaceItemEventData<T>>`   | An array with all items and its respective indexes        |
| args.Data[].OldItems[]    | `Generic`                                                 | The items that were inside the slot at `Index` previously |
| args.Data[].NewItems[]    | `Generic`                                                 | The items that are now inside the slot at `Index`         |
| args.Data[].Index         | `Integer`                                                 | Index that the `OldItems` were replaced for the `NewItems`|

### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length - 1; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>(slots);

inventory.OnReplace += (sender, args) =>
{
    foreach(var action in args.Data){
        Console.WriteLine($"Items removed from index {action.Index}:");
        foreach(var oldItems in action.OldItems){
            Console.WriteLine($"- Item : {oldItems}");
        }

        Console.WriteLine($"Items added to index {action.Index}:");
        foreach(var newItems in action.NewItems){
            Console.WriteLine($"- Item : {newItems}");
        }
    }
};

inventory.Replace(new string[] { "NewItem_1", "NewItem_2" }, 0);
```

