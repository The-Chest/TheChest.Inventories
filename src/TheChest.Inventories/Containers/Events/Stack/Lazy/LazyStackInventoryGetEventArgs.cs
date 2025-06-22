namespace TheChest.Inventories.Containers.Events.Stack.Lazy
{
    public record struct LazyStackInventoryGetItemEventData<T>(T Item, int Index, int Amount);

    /// <summary>
    /// Event arguments for the <see cref="LazyStackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public sealed class LazyStackInventoryGetEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> Data { get; }
        public LazyStackInventoryGetEventArgs(IReadOnlyCollection<LazyStackInventoryGetItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator LazyStackInventoryGetEventArgs<T>((T Item, int Index, int Amount) data)
        {
            return new LazyStackInventoryGetEventArgs<T>(
                new LazyStackInventoryGetItemEventData<T>[] {
                    new(data.Item, data.Index, data.Amount)
                }
            );
        }
    }

    public delegate void LazyStackInventoryGetEventHandler<T>(object? sender, LazyStackInventoryGetEventArgs<T> e);
}
