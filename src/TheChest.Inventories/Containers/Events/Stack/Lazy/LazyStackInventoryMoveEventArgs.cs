namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// Data for the Event args on <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="Item"/></typeparam>
    /// <param name="Item"></param>
    /// <param name="Amount"></param>
    /// <param name="FromIndex"></param>
    /// <param name="ToIndex"></param>
    public record struct LazyStackInventoryMoveItemEventData<T>(T Item, int Amount, int FromIndex, int ToIndex);

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyStackInventoryMoveEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryMoveItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="LazyStackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryMoveEventArgs(IReadOnlyCollection<LazyStackInventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="LazyStackInventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryMoveEventArgs<T>(
            (T Item, int Amount, int OriginIndex, int TargetIndex) data
        )
        {
            return new LazyStackInventoryMoveEventArgs<T>(
                new LazyStackInventoryMoveItemEventData<T>[1]
                {
                    new(data.Item, data.Amount, data.OriginIndex, data.TargetIndex),
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is moved in an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void LazyStackInventoryMoveEventHandler<T>(object? sender, LazyStackInventoryMoveEventArgs<T> e);
}
