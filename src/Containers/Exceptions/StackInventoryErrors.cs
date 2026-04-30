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
        
        #region Replacing Items
        internal const string CannotReplaceEmptyArray = "Cannot replace using an empty item array";
        internal const string MaxStackSizeSmallerThanItemsToReplace = "The max stack size is smaller than the number of items to replace.";
        #endregion
    }
}
