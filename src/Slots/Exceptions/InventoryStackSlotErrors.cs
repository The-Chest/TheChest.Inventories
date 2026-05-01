namespace TheChest.Inventories.Slots.Exceptions
{
    internal class InventoryStackSlotErrors
    {
        #region Add(T[] items)
        internal const string CannotAddEmptyItems = "Cannot add empty list of items";
        internal const string CannotAddArrayWithNullValues = "Cannot add an array of items with null values";
        internal const string CannotAddArrayWithDifferentTypes = "Cannot add an array of items with different types";
        internal const string CannotAddMoreThanAvailableAmount = "Cannot add more items than the available amount";
        #endregion

        #region Add(T[] items) & Add(T item)
        internal const string SlotIsFull = "The slot is full";
        internal const string CannotAddDifferentItemsFromSlot = "Cannot add items that are different from the items already in the slot";
        #endregion
    }
}
