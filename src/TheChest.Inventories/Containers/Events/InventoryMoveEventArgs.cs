namespace TheChest.Inventories.Containers.Events
{
    /// <summary>
    /// Data for the Event args on <see cref="InventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="Item"/></typeparam>
    /// <param name="Item"></param>
    /// <param name="FromIndex"></param>
    /// <param name="ToIndex"></param>
    public record struct InventoryMoveItemEventData<T>(T Item, int FromIndex, int ToIndex);

    /// <summary>
    /// Event arguments for the <see cref="InventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InventoryMoveEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryMoveItemEventData<T>> Data { get; }
        public InventoryMoveEventArgs(IReadOnlyCollection<InventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator InventoryMoveEventArgs<T>(
            (T Item, int OriginIndex, int TargetIndex) data
        )
        {
            return new InventoryMoveEventArgs<T>(
                new InventoryMoveItemEventData<T>[1]
                {
                    new(data.Item, data.OriginIndex, data.TargetIndex),
                }
            );
        }

        public static implicit operator InventoryMoveEventArgs<T>(
            (T Item, int OriginIndex, int TargetIndex)[] data
        )
        {
            return new InventoryMoveEventArgs<T>(
                data.Select(
                    item => new InventoryMoveItemEventData<T>(
                        Item: item.Item,
                        FromIndex: item.OriginIndex,
                        ToIndex: item.TargetIndex
                    )
                ).ToArray()
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is moved from one index to another in the inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void InventoryMoveEventHandler<T>(object? sender, InventoryMoveEventArgs<T> e);
}
