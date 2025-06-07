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
    }
}
