﻿namespace TheChest.Inventories.Containers.Events.Stack
{
    /// <summary>
    /// Data for the Event args on <see cref="StackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index"></param>
    public record struct StackInventoryAddItemEventData<T>(T[] Items, int Index);

    /// <summary>
    /// Event arguments for the <see cref="StackInventoryAddEventHandler{T}"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class StackInventoryAddEventArgs<T> : EventArgs
    {
        public IReadOnlyCollection<StackInventoryAddItemEventData<T>> Data { get; }
        public StackInventoryAddEventArgs(IReadOnlyCollection<StackInventoryAddItemEventData<T>> data)
        {
            Data = data;
        }

        public static implicit operator StackInventoryAddEventArgs<T>((T[] Items, int Index) data)
        {
            return new StackInventoryAddEventArgs<T>(
                new StackInventoryAddItemEventData<T>[] {
                    new(
                        Items: data.Items,
                        Index: data.Index
                    )
                }
            );
        }
    }
    
    /// <summary>
    /// Event handler triggered when one or more items of the same type are added to an inventory.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="sender">Inventory that sent the event</param>
    /// <param name="e">Data that were sent throught the event</param>
    public delegate void StackInventoryAddEventHandler<T>(object? sender, StackInventoryAddEventArgs<T> e);
}
