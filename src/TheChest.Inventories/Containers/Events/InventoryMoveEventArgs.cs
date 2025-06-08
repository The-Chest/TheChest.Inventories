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

        public static implicit operator InventoryMoveEventArgs<T>(((T Item, int Index) Origin, (T Item, int Index) Target) data)
        {
            var origin = data.Origin;
            var target = data.Target;
            return new InventoryMoveEventArgs<T>(
                new InventoryMoveItemEventData<T>(origin.Item, origin.Index, target.Index),
                new InventoryMoveItemEventData<T>(target.Item, target.Index, origin.Index)
            );
        }
    }
}
