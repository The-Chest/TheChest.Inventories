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
    }
}
