namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for the Event args on <see cref="StackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="Item"/></typeparam>
    /// <param name="Items"></param>
    /// <param name="FromIndex"></param>
    /// <param name="ToIndex"></param>
    public record struct StackInventoryMoveItemEventData<T>(T[] Items, int FromIndex, int ToIndex);

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryMoveEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class StackInventoryMoveEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<StackInventoryMoveItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="StackInventoryMoveEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public StackInventoryMoveEventArgs(IReadOnlyCollection<StackInventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from a tuple to <see cref="StackInventoryMoveEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator StackInventoryMoveEventArgs<T>(
            (T[] Items, int OriginIndex, int TargetIndex) data
        )
        {
            return new StackInventoryMoveEventArgs<T>(
                new StackInventoryMoveItemEventData<T>[1]
                {
                    new(data.Items, data.OriginIndex, data.TargetIndex),
                }
            );
        }
    }

    /// <summary>
    /// Event handler triggered when an item is added to an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void StackInventoryMoveEventHandler<T>(object? sender, StackInventoryMoveEventArgs<T> e);
}
