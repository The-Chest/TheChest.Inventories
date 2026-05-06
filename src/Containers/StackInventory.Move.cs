using System;
using System.Collections.Generic;
using TheChest.Inventories.Containers.Events.Stack;

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

            if (origin == target)
                return false;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return false;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return false;

            return true;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                return;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            //TODO: improve these checks
            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return;

            var originItems = slotOrigin.GetAll();
            var targetItems = slotTarget.GetAll();

            var events = new List<StackInventoryMoveItemEventData<T>>();

            if (originItems.Length > 0 && slotTarget.CanAdd(originItems))
            {
                slotTarget.Add(originItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        originItems,
                        origin,
                        target
                    )
                );
            }

            if (targetItems.Length > 0 && slotOrigin.CanAdd(targetItems))
            {
                slotOrigin.Add(targetItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        targetItems,
                        target,
                        origin
                    )
                );
            }

            if (events.Count > 0)
                this.OnMove?.Invoke(this, new StackInventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}
