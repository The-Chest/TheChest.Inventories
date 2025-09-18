# Implementing a custom IStackInventory<T>
You can implement the interface `IStackInventory<T>` directly if you need more control.

## IStackInventory

```csharp
public class MyStackInventory : IStackInventory<int>{
    protected readonly IInventoryStackSlot<int>[] slots;

    public MyStackInventory(IInventoryStackSlot<int>[] slots)
    {
        this.slots = slots;
    }

    public bool Add(int item)
    {
        if(item < 1)
            return false;
        if(this.slot[item].IsFull)
            return false;
        if(!this.slot[item].CanAdd(item))
            return false;

        return this.slot[item].Add(item);
    }
    /// all other methods will need to be implemented too
}
```

## IInventoryStackSlot

```csharp
public class MyStackSlot : IInventoryStackSlot<int>
{
    protected readonly int[] contents;
    public bool IsFull => this.content.Length > 0;

    public MyStackSlot(int[] contents)
    {
        this.contents = contents;
    }

    public virtual bool Add(int item)
    {
        if(item <= 0 || this.IsFull)
            return false;

        for (int i = 0; i < this.contents; i++)
        {
            if (this.contents[i] == 0)
            {
                this.contents[i] += item;
                return true;
             }
        }
        return false;
    }
    /// all other methods will need to be implemented too
}
```