using System;
using System.Collections.Generic;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Exceptions;

namespace TheChest.Inventories.Containers
{
    public partial class StackInventory<T>
    {
        /// <inheritdoc/>
        public event StackInventoryMoveEventHandler<T> OnMove;

        /// <inheritdoc/>
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
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> and <paramref name="target"/> are the same</exception>
        /// <exception cref="InvalidOperationException">When both slots are empty or when the max stack size of the two slots are different</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                throw new ArgumentException(StackInventoryErrors.CannotMoveItemToSameIndex, nameof(target));

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                throw new InvalidOperationException(StackInventoryErrors.CannotMoveEmptySlots);
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                throw new InvalidOperationException(StackInventoryErrors.CannotMoveToDifferentMaxStackSize);

            var originItems = slotOrigin.GetAll();
            var targetItems = slotTarget.GetAll();

            var events = new List<StackInventoryMoveItemEventData<T>>();

            if (originItems.Length > 0)
            {
                // maybe improve performance by using AddItems and making it internal?
                slotTarget.Add(originItems);
                events.Add(new StackInventoryMoveItemEventData<T>(originItems, origin, target));
            }

            if (targetItems.Length > 0)
            {
                slotOrigin.Add(targetItems);
                events.Add(new StackInventoryMoveItemEventData<T>(targetItems, target, origin));
            }

            if (events.Count > 0)
                this.OnMove?.Invoke(this, new StackInventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}
