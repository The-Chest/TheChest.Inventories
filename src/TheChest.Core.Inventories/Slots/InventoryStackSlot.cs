﻿using TheChest.Core.Inventories.Slots.Interfaces;
using TheChest.Core.Slots;
using TheChest.Core.Slots.Interfaces;

namespace TheChest.Core.Inventories.Slots
{
    /// <summary>
    /// Slot with with <see cref="IInventoryStackSlot{T}"/> implementation with a collection of items
    /// <para>
    /// This slot has behaviors of <seealso cref="StackSlot{T}"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T">The item collection inside the slot accepts</typeparam>
    public class InventoryStackSlot<T> : StackSlot<T>, IInventoryStackSlot<T>
    {
        /// <summary>
        /// Creates an Inventory Slot with default items stacked
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        public InventoryStackSlot(T[] items) : base(items) { }
        /// <summary>
        /// Creates an Inventory Slot with default items stacked
        /// </summary>
        /// <param name="items"><inheritdoc cref="StackSlot{T}.StackSlot(T[], int)" path="/param[@name='items']" /></param>
        /// <param name="maxStackAmount"><inheritdoc cref="StackSlot{T}.StackSlot(T[], int)" path="/param[@name='maxStackAmount']"/></param>
        public InventoryStackSlot(T[] items, int maxStackAmount) : base(items, maxStackAmount) { }

        /// <summary>
        /// Adds an array of items inside the Content with no previous validation.
        /// <para>
        /// It's recommended to use <see cref="IInventoryStackSlot{T}.Add(ref T[])"/> or <see cref="IInventoryStackSlot{T}.TryAdd(ref T[])"/> to ensure no invalid items are added
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        protected virtual void AddItems(ref T[] items)
        {
            var availableAmount = this.MaxStackAmount - this.StackAmount;

            var addAmount = items.Length > availableAmount ? 
                availableAmount : 
                items.Length;

            var itemIndex = 0;
            for (int i = 0; i < this.MaxStackAmount; i++)
            {
                if (this.content[i] is null)
                {
                    this.content[i] = items[itemIndex++];
                }

                if (itemIndex == addAmount)
                {
                    break;
                }
            }

            items = items[addAmount..];
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <para>
        /// The items must be the same in it and in the slot or it'll not add and return false
        /// </para>
        /// <param name="items"><inheritdoc/></param>
        /// <returns>false if is not possible to add all of the items.</returns>
        public virtual bool TryAdd(ref T[] items)
        {
            if(!this.CanAdd(items))
            {
                return false;
            }

            this.AddItems(ref items);
            
            return items.Length == 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// The items must be the same in it and in the slot (if is not empty) or it'll throw an <see cref="ArgumentException"/>. 
        /// </para>
        /// <para>
        /// Use <see cref="IInventoryStackSlot{T}.TryAdd(ref T[])"/> if you don't want to handle these exceptions or after <see cref="IInventoryStackSlot{T}.CanAdd(T[])"/> 
        /// </para>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <exception cref="ArgumentException">When the item array is empty or has different items inside it or has any that is not equal to the items inside <see cref="ISlot{T}.Content"/></exception>
        public virtual void Add(ref T[] items)
        {
            //TODO: improve this method
            if (items.Length == 0)
            {
                throw new ArgumentException("Cannot add empty list of items", nameof(items));
            }

            for (int i = 1; i < items.Length; i++)
            {
                if (!items[0]!.Equals(items[i]))
                {
                    throw new ArgumentException($"Param \"items\" have items that are not equal ({i})", nameof(items));
                }

                if (!this.IsEmpty && !this.Content.First()!.Equals(items[i]))//TODO: use Contains
                {
                    throw new ArgumentException($"Param \"items\" must have every item equal to the Current item on the Slot ({i})", nameof(items));
                }
            }           

            this.AddItems(ref items);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns>Returns false if is Full or Contains item of a different type than <paramref name="item"/></returns>
        public virtual bool CanAdd(T item)
        {
            if (item == null)
                return false;

            if (this.IsFull)
                return false;

            if (!this.IsEmpty)
                return this.Content.First()!.Equals(item);//TODO: Use Contains

            return true;
        }

        /// <summary>
        /// <inheritdoc/>.
        /// Uses <see cref="IInventoryStackSlot{T}.CanAdd(T)"/> validation for each one.
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool CanAdd(T[] items)
        {
            if (items.Length == 0)
                return false;

            if (this.IsFull)
                return false;

            var firstItem = items[0]!;
            for (int i = 0; i < items.Length; i++)
            {
                if (!this.CanAdd(items[i]))
                    return false;

                if(!firstItem.Equals(items[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets and removes all items from <see cref="ISlot{T}.Content"/>
        /// </summary>
        /// <returns>All items from <see cref="ISlot{T}.Content"/></returns>
        public virtual T[] GetAll()
        {
            var result = this.Content.Where(x => x is not null).ToArray();
            Array.Clear(this.content);
            return result;
        }

        /// <summary>
        /// Gets an removes amount of items from <see cref="ISlot{T}.Content"/>.
        /// If is bigger than <see cref="IStackSlot{T}.StackAmount"/> it returns the maximum amount possible.
        /// </summary>
        /// <param name="amount">Amount of items to get from <see cref="ISlot{T}.Content"/></param>
        /// <returns>An array with the max amount possible from <see cref="ISlot{T}.Content"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] GetAmount(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (amount >= this.StackAmount)
            {
                return this.GetAll();
            }

            var result = this.content
                .Where(x => x is not null)
                .Take(amount)
                .ToArray();
            for (int i = 0; i < amount; i++)
            {
                this.content[i] = default!;
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <returns>The current items from <see cref="ISlot{T}.Content"/> or <paramref name="items"/> if is not possible to replace</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="items"/> dize is zero or bigger than <see cref="IStackSlot{T}.MaxStackAmount"/></exception>
        /// <exception cref="ArgumentException">When any of items in param are invalid</exception>
        public virtual T[] Replace(ref T[] items)
        {
            if (items.Length == 0)
            {
                throw new ArgumentException("Cannot replace the slot for empty item array", nameof(items));
            }

            if (items.Length > this.MaxStackAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(items));
            }

            var firstItem = items[0]!;
            for (int i = 1; i < items.Length; i++)
            {
                if (!firstItem.Equals(items[i]))//TODO: use Contains
                {
                    throw new ArgumentException($"Param \"items\" have items that are not equal ({i})", nameof(items));
                }
            }

            if(this.IsEmpty) 
            {
                this.Add(ref items);
                return Array.Empty<T>();
            }

            var result = this.GetAll();
            if (this.TryAdd(ref items))
            {
                return result; 
            }

            this.AddItems(ref result);
            return items;
        }

        public bool Contains(T item)
        {
            if (this.IsEmpty)
                return false;

            return this.Content.First()!.Equals(item);
        }
    }
}
