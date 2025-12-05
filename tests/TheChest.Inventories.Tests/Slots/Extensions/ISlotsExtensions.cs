using System.Reflection;
using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Extensions
{
    public static class ISlotsExtensions
    {
        private static FieldInfo GetContentField<T>(this ISlot<T> slot)
        {
            if (slot is null)
                throw new ArgumentNullException(nameof(slot));
            var type = slot.GetType();
            var field = type.GetField(
                "content",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy
            ) ??
            throw new InvalidOperationException("Field 'content' not found.");

            return field;
        }
        public static T? GetContent<T>(this ISlot<T> slot)
        {
            if (slot is null) 
                throw new ArgumentNullException(nameof(slot));

            var field = slot.GetContentField();
            var fieldType = field.FieldType;
            if (fieldType != typeof(T) && !typeof(T).IsAssignableFrom(fieldType))
                throw new InvalidOperationException($"Field type '{fieldType}' is not assignable to '{typeof(T)}'.");

            return (T?)field.GetValue(slot);
        }

        public static T[]? GetContents<T>(this ISlot<T> slot)
        {
            if (slot is null)
                throw new ArgumentNullException(nameof(slot));

            var field = slot.GetContentField();
            var fieldType = field.FieldType;
            if (fieldType != typeof(T[]) && !typeof(T[]).IsAssignableFrom(fieldType))
                throw new InvalidOperationException($"Field type '{fieldType}' is not assignable to '{typeof(T[])}'.");

            var original = (T[]?)field.GetValue(slot);
            if(original is null)
                return null;

            return (T[])original.Clone();
        }
    }
}