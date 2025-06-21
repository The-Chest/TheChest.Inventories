namespace TheChest.Inventories.Containers.Events.Stack
{
    public record struct StackInventoryGetItemEventData<T>(T[] Items, int Index);

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryGetEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T">Items type</typeparam>
    public sealed class StackInventoryGetEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<StackInventoryGetItemEventData<T>> Data { get; }
        public StackInventoryGetEventArgs(IReadOnlyCollection<StackInventoryGetItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator StackInventoryGetEventArgs<T>((T[] Items, int Index) data)
        {
            return new StackInventoryGetEventArgs<T>(
                new StackInventoryGetItemEventData<T>[] {
                    new(data.Items, data.Index)
                }
            );
        }
    }

    public delegate void StackInventoryGetEventHandler<T>(object? sender, StackInventoryGetEventArgs<T> e);
}
