namespace TheChest.Inventories.Containers.Events
{
    public record struct InventoryItemEventData<T>(T Item, int Index);
}
