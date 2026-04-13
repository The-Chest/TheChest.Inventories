using TheChest.Core.Slots.Interfaces;

namespace TheChest.Tests.Common.Extensions.Slots
{
    public static class IStackSlotExtensions
    {
        /// <summary>
        /// Retrieves a copy of the contents stored in the specified stack slot.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the stack slot.</typeparam>
        /// <param name="slot">The stack slot from which to retrieve the contents.</param>
        /// <returns>A new array containing the elements stored in the stack slot, or <see langword="null"/> if the slot is empty.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the underlying field type of the stack slot is not assignable to an array of type <typeparamref name="T"/>.</exception>
        public static T[] GetContents<T>(this IStackSlot<T> slot)
        {
            var field = slot.GetContentField();

            var fieldType = field.FieldType;
            if (fieldType != typeof(T[]) && !typeof(T[]).IsAssignableFrom(fieldType))
                throw new InvalidOperationException($"Field type '{fieldType}' is not assignable to '{typeof(T[])}'.");
            
            var original = (T[])field.GetValue(slot);
            if (original is null)
                return Array.Empty<T>();

            return (T[])original.Clone();
        }
    }
}
