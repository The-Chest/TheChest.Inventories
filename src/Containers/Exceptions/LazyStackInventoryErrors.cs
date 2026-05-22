namespace TheChest.Inventories.Containers.Exceptions
{
    internal static class LazyStackInventoryErrors
    {
        #region Adding Items
        internal const string InventoryIsFull = "The inventory is full";
        internal const string NotEnoughSpace = "There is not enough space to add the items.";

        internal const string NotPossibleToAddItem = "It is not possible to add the item to the inventory.";
        #endregion

        #region Moving Items
        internal const string CannotMoveEmptySlots = "Cannot move empty slots.";
        internal const string CannotMoveItemToSameIndex = "Cannot move an item to the same index.";
        internal const string CannotMoveToDifferentMaxStackSize = "Cannot move items to a slot with a different max stack size.";
        #endregion
    }
}
