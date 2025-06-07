namespace TheChest.Inventories.Containers.Events
{
    public sealed class InventoryGetOneEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public int Index { get; }
        public InventoryGetOneEventArgs(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}
