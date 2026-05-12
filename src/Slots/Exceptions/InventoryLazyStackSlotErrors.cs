namespace TheChest.Inventories.Slots.Exceptions
{
    internal static class InventoryLazyStackSlotErrors
    {
        #region Adding Items
        internal const string SlotIsFull = "The slot is full.";
        internal const string AddDifferentItemsFromSlot = "Cannot add items that are different from the items already in the slot";
        internal const string AddMoreThanAvailableAmount = "Cannot add more items than the available amount";
        #endregion
    }
}
