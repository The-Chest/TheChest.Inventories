namespace TheChest.Inventories.Containers.Events
{
    public record struct InventoryAddItemEventData<T>(T Item, int Index);

    public sealed class InventoryAddEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryAddItemEventData<T>> Data { get; }
        public InventoryAddEventArgs(IReadOnlyCollection<InventoryAddItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator InventoryAddEventArgs<T>((T Item, int Index) data)
        {
            return new InventoryAddEventArgs<T>(
                new InventoryAddItemEventData<T>[] { 
                    new(data.Item, data.Index) 
                }
            );
        }

        public static implicit operator InventoryAddEventArgs<T>((T[] Items, int[] Indexes) data)
        {
            return new InventoryAddEventArgs<T>(
                data.Items.Select(
                (item, i) =>
                    new InventoryAddItemEventData<T>(
                        Item: item,
                        Index: data.Indexes[i]
                    )
                ).ToArray() 
            );
        }
    }
}
