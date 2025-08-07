namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    /// <summary>
    /// Data for the Event args on <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Item"></param>
    /// <param name="Index">Index item that where get</param>
    /// <param name="Amount"></param>
    public record struct LazyStackInventoryGetItemEventData<T>(T Item, int Index, int Amount);

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public sealed class LazyStackInventoryGetEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public LazyStackInventoryGetEventArgs(IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> data)
        {
            Data = data;
        }

        /// <summary>
        /// Implicit conversion from an array of tuples to <see cref="LazyStackInventoryGetEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator LazyStackInventoryGetEventArgs<T>((T Item, int Index, int Amount) data)
        {
            return new LazyStackInventoryGetEventArgs<T>(
                new LazyStackInventoryGetItemEventData<T>[] {
                    new(data.Item, data.Index, data.Amount)
                }
            );
        }
    }

    /// <summary>
    /// Event handler for the <see cref="LazyStackInventoryGetEventArgs{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LazyStackInventoryGetEventHandler<T>(object? sender, LazyStackInventoryGetEventArgs<T> e);
}
