namespace TheChest.Inventories.Containers.Events.Stack
{
    public record struct StackInventoryAddItemEventData<T>(T Item, int Index, int Amount);

    public sealed class StackInventoryAddEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<StackInventoryAddItemEventData<T>> Data { get; }
        public StackInventoryAddEventArgs(IReadOnlyCollection<StackInventoryAddItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator StackInventoryAddEventArgs<T>((T Item, int Index, int Amount) data)
        {
            return new StackInventoryAddEventArgs<T>(
                new StackInventoryAddItemEventData<T>[] {
                    new(
                        Item: data.Item,
                        Index: data.Index,
                        Amount: data.Amount
                    )
                }
            );
        }

        public static implicit operator StackInventoryAddEventArgs<T>((T[] Items, int[] Indexes, int[] Amounts) data)
        {
            return new StackInventoryAddEventArgs<T>(
                data.Items.Select(
                (item, i) =>
                    new StackInventoryAddItemEventData<T>(
                        Item: item,
                        Index: data.Indexes[i],
                        Amount: data.Amounts[i]
                    )
                ).ToArray()
            );
        }
    }

    public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
}
