namespace TheChest.Inventories.Containers.Events
{
    public record struct InventoryMoveItemEventData<T>(T Item, int OldIndex, int NewIndex);

    public sealed class InventoryMoveEventArgs<T> : EventArgs
    {
        public InventoryMoveItemEventData<T> From { get; }
        public InventoryMoveItemEventData<T> To { get; }
        public InventoryMoveEventArgs(
            InventoryMoveItemEventData<T> from,
            InventoryMoveItemEventData<T> to
        )
        {
            From = from;
            To = to;
        }
    }
}
