namespace TheChest.Inventories.Containers.Events.Stack
{
    public record struct StackInventoryAddItemEventData<T>(T[] Items, int Index);

    public sealed class StackInventoryAddEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<StackInventoryAddItemEventData<T>> Data { get; }
        public StackInventoryAddEventArgs(IReadOnlyCollection<StackInventoryAddItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator StackInventoryAddEventArgs<T>((T[] Items, int Index) data)
        {
            return new StackInventoryAddEventArgs<T>(
                new StackInventoryAddItemEventData<T>[] {
                    new(
                        Items: data.Items,
                        Index: data.Index
                    )
                }
            );
        }
    }

    public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
}
