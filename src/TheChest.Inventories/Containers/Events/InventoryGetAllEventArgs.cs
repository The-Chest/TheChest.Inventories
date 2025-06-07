namespace TheChest.Inventories.Containers.Events
{
    public sealed class InventoryGetAllEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryItemEventData<T>> Data { get; }
        public InventoryGetAllEventArgs(IReadOnlyCollection<InventoryItemEventData<T>> data)
        {
            Data = data;
        }
    }
}
