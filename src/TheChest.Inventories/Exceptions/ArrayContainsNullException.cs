using System;

namespace TheChest.Inventories.Exceptions
{
    internal class ArrayContainsNullException : ArgumentNullException
    {
        public ArrayContainsNullException(string paramName) : base(paramName, "One of the items is null")
        {
            
        }
    }
}
