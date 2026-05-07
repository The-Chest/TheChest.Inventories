namespace TheChest.Inventories.Slots.Exceptions
{
    internal static class InventoryStackSlotErrors
    {
        #region Add Items
        internal const string AddEmptyItems = "Cannot add empty list of items";
        internal const string AddArrayWithNullValues = "Cannot add an array of items with null values";
        internal const string AddArrayWithDifferentTypes = "Cannot add an array of items with different types";
        internal const string AddMoreThanAvailableAmount = "Cannot add more items than the available amount";

        internal const string SlotIsFull = "The slot is full";
        internal const string AddDifferentItemsFromSlot = "Cannot add items that are different from the items already in the slot";
        #endregion
    }
}
