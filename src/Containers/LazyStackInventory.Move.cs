using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Containers.Exceptions;

namespace TheChest.Inventories.Containers
{
    public partial class LazyStackInventory<T>
    {
        /// <inheritdoc/>
        public event LazyStackInventoryMoveEventHandler<T> OnMove;

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual bool CanMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            if (origin == target)
                return false;
            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return false;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return false;

            return true;
        }

        /// <summary>
        /// Moves an item from one slot to another
        /// </summary>
        /// <param name="origin">Origin slot that will be moved to <paramref name="target"/></param>
        /// <param name="target">Target slot that will be moved to <paramref name="origin"/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> index are smaller than zero or bigger than <see cref="StackContainer{T}.Size"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> and <paramref name="target"/> are equal</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                throw new ArgumentException(LazyStackInventoryErrors.CannotMoveItemToSameIndex, nameof(target));

            var originSlot = this.slots[origin];
            var targetSlot = this.slots[target];

            if (originSlot.IsEmpty && targetSlot.IsEmpty)
                throw new InvalidOperationException(LazyStackInventoryErrors.CannotMoveEmptySlots);
            if (originSlot.MaxAmount != targetSlot.MaxAmount)
                throw new InvalidOperationException(LazyStackInventoryErrors.CannotMoveToDifferentMaxStackSize);

            var events = new List<LazyStackInventoryMoveItemEventData<T>>();

            var originItems = originSlot.GetAll();
            var targetItems = targetSlot.GetAll();

            if (originItems.Length > 0)
            {
                var originItem = originItems.FirstOrDefault();
                targetSlot.Add(originItem, originItems.Length);
                events.Add(new LazyStackInventoryMoveItemEventData<T>(originItem, originItems.Length, origin, target));
            }

            if (targetItems.Length > 0)
            {
                var targetItem = targetItems.FirstOrDefault();
                originSlot.Add(targetItem, targetItems.Length);
                events.Add(new LazyStackInventoryMoveItemEventData<T>(targetItem, targetItems.Length, target, origin));
            }

            this.OnMove?.Invoke(this, new LazyStackInventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}
