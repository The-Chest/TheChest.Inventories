namespace TheChest.Inventories.Containers.Exceptions
{
    internal static class StackInventoryErrors
    {
        #region Constructor
        internal const string MaxStackSizeMustBeGreaterThanZero = "The max stack size must be greater than zero.";
        #endregion

        #region Parameters Validation
        internal const string ItemArrayContainsNull = "One of the items to add is null";
        #endregion

        #region Add Items
        internal const string CannotAddEmptyArray = "Cannot add using an empty item array";
        internal const string InventoryIsFull = "The inventory is full";
        internal const string NotEnoughFreeSlots = "Not enough free slots to add all the items.";

        internal const string NotPossibleToAddItem = "It is not possible to add the item to the inventory.";
        internal const string NotPossibleToAddAllItems = "It is not possible to add all the items to the inventory.";
        internal const string CannotAddArrayWithDifferentItems = "Cannot add an array of items with different types.";
        #endregion

        #region Replacing Items
        internal const string CannotReplaceEmptyArray = "Cannot replace using an empty item array";
        internal const string MaxStackSizeSmallerThanItemsToReplace = "The max stack size is smaller than the number of items to replace.";
        #endregion
    }
}
