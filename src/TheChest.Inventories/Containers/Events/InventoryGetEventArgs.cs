namespace TheChest.Inventories.Containers.Events
{
    public record struct InventoryGetItemEventData<T>(T Item, int Index);

    public sealed class InventoryGetEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryGetItemEventData<T>> Data { get; }
        public InventoryGetEventArgs(IReadOnlyCollection<InventoryGetItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator InventoryGetEventArgs<T>((T Item, int Index) data)
        {
            return new InventoryGetEventArgs<T>(
                new InventoryGetItemEventData<T>[] {
                    new(data.Item, data.Index)
                }
            );
        }

        public static implicit operator InventoryGetEventArgs<T>((T[] Items, int[] Indexes) data)
        {
            return new InventoryGetEventArgs<T>(
                data.Items.Select(
                (item, i) =>
                    new InventoryGetItemEventData<T>(
                        Item: item,
                        Index: data.Indexes[i]
                    )
                ).ToArray()
            );
        }
    }
}
