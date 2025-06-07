namespace TheChest.Inventories.Containers.Events
{
    public sealed class InventoryGetEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryItemEventData<T>> Data { get; }
        public InventoryGetEventArgs(IReadOnlyCollection<InventoryItemEventData<T>> data)
        {
            Data = data;
        }
    }
}
