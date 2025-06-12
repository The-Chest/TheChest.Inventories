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
| sender                    | object                                                | Inventory responsible for firing the event         |
| args                      | InventoryGetEventArgs                                 | Class that holds data of the event                 |
| args.Data                 | IReadOnlyCollection<InventoryGetItemEventData<T>>     | An array with all items and its respective indexes |
| args.Data[].Item          | Generic                                               | Found item                                         |
| args.Data[].Index         | Integer                                               | Index number where the `Item` was found            |

#### Example

```csharp
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots.Interfaces;

var slots = new InventorySlot<string>[10];
for (int i = 0; i < slots.Length; i++)
{
    slots[i] = new InventorySlot<string>($"Item_{i}");
}

var inventory = new Inventory<string>();
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