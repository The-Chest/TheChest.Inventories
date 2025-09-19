# Extending bult-in Inventory
You can extend the built-ind class `Inventory<T>` to override with your own features.

## Inventory
```csharp
public class MyInventory : Inventory<int>
{
    public MyInventory(IInventorySlot<int>[] slots) : base(slots)
    {
        if (slots.Length != 10)
            throw new System.ArgumentException("Invalid inventory size");
    }

    public override bool Add(int item)
    {
        return this.slots[item].Add(item);
    }
    
    // You can override other methods as needed
}
```

## InventorySlot

```csharp
public class MySlot : InventorySlot<int>
{
    public MySlot(int content = 0) : base(content) { }

    public virtual bool Add(int item)
    {
        if(this.IsFull || item <= 0){
            return false;
        }
        this.content = item;
        return true;
    }

    // You can override other methods as needed
}
```