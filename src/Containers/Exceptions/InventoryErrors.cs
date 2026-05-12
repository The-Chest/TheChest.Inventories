namespace TheChest.Inventories.Containers.Exceptions
{
    internal static class InventoryErrors
    {
        #region Constructor
        internal const string ItemsBiggerThanInventorySize = "The number of items exceeds the inventory size.";
        #endregion

        #region Parameters Validation
        internal const string ItemArrayContainsNull = "One of the items to add is null";
        #endregion

        #region Adding Items
        internal const string CannotAddItemAtIndex = "The item cannot be added to the slot at index.";
        internal const string InventoryIsFull = "The inventory is full";
        internal const string NotEnoughFreeSlots = "There are not enough free slots to add all the items.";
        internal const string CannotAddEmptyArray = "Cannot add an empty array of items.";
        #endregion
    }
}