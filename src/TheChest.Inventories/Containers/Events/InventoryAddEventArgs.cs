namespace TheChest.Inventories.Containers.Events
{
    /// <summary>
    /// Data for the Event args on <see cref="InventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Item"></param>
    /// <param name="Index"></param>
    public record struct InventoryAddItemEventData<T>(T Item, int Index);

    /// <summary>
    /// Event arguments for the <see cref="InventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InventoryAddEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryAddItemEventData<T>> Data { get; }
        public InventoryAddEventArgs(IReadOnlyCollection<InventoryAddItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator InventoryAddEventArgs<T>((T Item, int Index) data)
        {
            return new InventoryAddEventArgs<T>(
                new InventoryAddItemEventData<T>[] { 
                    new(data.Item, data.Index) 
                }
            );
        }

        public static implicit operator InventoryAddEventArgs<T>((T[] Items, int[] Indexes) data)
        {
            return new InventoryAddEventArgs<T>(
                data.Items.Select(
                (item, i) =>
                    new InventoryAddItemEventData<T>(
                        Item: item,
                        Index: data.Indexes[i]
                    )
                ).ToArray() 
            );
        }
    }

    /// <summary>
    /// Event handler triggered when one or more items of the same type are added to an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void InventoryAddEventHandler<T>(object? sender, InventoryAddEventArgs<T> e);
}
