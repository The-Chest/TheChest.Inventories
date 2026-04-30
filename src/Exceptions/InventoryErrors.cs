namespace TheChest.Inventories.Exceptions
{
    internal static class InventoryErrors
    {
        #region Constructor
        internal static string ItemsBiggerThanInventorySize(int items, int inventorySize) => $"The number of items ({items}) exceeds the inventory size ({inventorySize}).";
        #endregion

        #region Parameters Validation
        internal const string ItemArrayContainsNull = "One of the items to add is null";
        #endregion

        #region Adding Items
        internal static string CannotAddItemAtIndex(int index) => $"The item cannot be added to the slot at index {index}.";
        #endregion
    }
}