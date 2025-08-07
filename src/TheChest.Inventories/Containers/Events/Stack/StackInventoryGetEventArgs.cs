namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for the Event args on <see cref="StackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index"></param>
    public record struct StackInventoryGetItemEventData<T>(T[] Items, int Index);

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public sealed class StackInventoryGetEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Data sent throught the event.
        /// </summary>
        public IReadOnlyCollection<StackInventoryGetItemEventData<T>> Data { get; }
        /// <summary>
        /// Creates event args for the <see cref="StackInventoryGetEventHandler{T}"/> event.
        /// </summary>
        /// <param name="data"></param>
        public StackInventoryGetEventArgs(IReadOnlyCollection<StackInventoryGetItemEventData<T>> data)
        {
            Data = data;
        }
        /// <summary>
        /// Implicit conversion from an array of tuples to <see cref="StackInventoryGetEventArgs{T}"/>.
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator StackInventoryGetEventArgs<T>((T[] Items, int Index) data)
        {
            return new StackInventoryGetEventArgs<T>(
                new StackInventoryGetItemEventData<T>[] {
                    new(data.Items, data.Index)
                }
            );
        }
    }

    /// <summary>
    /// Event handler for the <see cref="StackInventoryGetEventArgs{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void StackInventoryGetEventHandler<T>(object? sender, StackInventoryGetEventArgs<T> e);
}
