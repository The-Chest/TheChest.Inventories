using System;
using System.Collections.Generic;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class Inventory<T>
    {
        /// <inheritdoc/>
        public event InventoryMoveEventHandler<T> OnMove;

        /// <summary>
        /// Checks if the specified item can be moved from the origin index to the target index.
        /// </summary>
        /// <param name="origin">The zero-based index representing the item's current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        /// <returns>true if the item can be moved to the target index; otherwise, false.</returns>
        protected bool CanMoveItems(int origin, int target)
        {
            if (origin == target)
                return false;
            if (this.slots[origin].IsEmpty && this.slots[target].IsEmpty)
                return false;

            return true;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        public virtual bool CanMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            
            return this.CanMoveItems(origin, target);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        public virtual bool TryMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            if (!this.CanMoveItems(origin, target))
                return false;

            var originItem = this.slots[origin].IsEmpty ? default : this.slots[origin].Get();
            var targetItem = this.slots[target].IsEmpty ? default : this.slots[target].Get();

            var events = new List<InventoryMoveItemEventData<T>>(1);
            if (!originItem.IsNull())
            {
                this.slots[target].Add(originItem);
                events.Add(new InventoryMoveItemEventData<T>(originItem, origin, target));
            }

            if (!targetItem.IsNull())
            {
                this.slots[origin].Add(targetItem);
                events.Add(new InventoryMoveItemEventData<T>(targetItem, target, origin));
            }

            if (events.Count > 0)
                this.OnMove?.Invoke(this, new InventoryMoveEventArgs<T>(events.ToArray()));

            return true;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnMove"/> event.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> and <paramref name="target"/> are the same</exception>
        /// <exception cref="InvalidOperationException">When both <paramref name="origin"/> and <paramref name="target"/> slots are empty</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            if (origin == target)
                throw new ArgumentException(InventoryErrors.CannotMoveItemToSameIndex, nameof(target));
            if(this.slots[origin].IsEmpty && this.slots[target].IsEmpty)
                throw new InvalidOperationException(InventoryErrors.CannotMoveEmptySlots);

            var originItem = this.slots[origin].IsEmpty ? default : this.slots[origin].Get();
            var targetItem = this.slots[target].IsEmpty ? default : this.slots[target].Get();

            var events = new List<InventoryMoveItemEventData<T>>(1);
            if (!originItem.IsNull())
            {
                this.slots[target].Add(originItem);
                events.Add(new InventoryMoveItemEventData<T>(originItem, origin, target));
            }

            if (!targetItem.IsNull())
            {
                this.slots[origin].Add(targetItem);
                events.Add(new InventoryMoveItemEventData<T>(targetItem, target, origin));
            }

            if (events.Count > 0)
                this.OnMove?.Invoke(this, new InventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}
