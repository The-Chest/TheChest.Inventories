namespace TheChest.Inventories.Containers.Events
{
    public record struct InventoryMoveItemEventData<T>(T Item, int FromIndex, int ToIndex);

    public sealed class InventoryMoveEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<InventoryMoveItemEventData<T>> Data { get; }
        public InventoryMoveEventArgs(IReadOnlyCollection<InventoryMoveItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator InventoryMoveEventArgs<T>(
            (
                (T Item, int Index) Origin, 
                (T Item, int Index) Target
            ) data
        )
        {
            var origin = data.Origin;
            var target = data.Target;
            return new InventoryMoveEventArgs<T>(
                new InventoryMoveItemEventData<T>[2]
                {
                    new(origin.Item, origin.Index, target.Index),
                    new(target.Item, target.Index, origin.Index)
                }
            );
        }

        public static implicit operator InventoryMoveEventArgs<T>(
            (T Item, int OriginIndex, int TargetIndex) data
        )
        {
            return new InventoryMoveEventArgs<T>(
                new InventoryMoveItemEventData<T>[1]
                {
                    new(data.Item, data.OriginIndex, data.TargetIndex),
                }
            );
        }
    }
}
