using TheChest.Core.Containers;
using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    public class LazyStackInventory<T> : StackContainer<T>, ILazyStackInventory<T>
    {
        protected IInventoryLazyStackSlot<T>[] slots;
        public override IStackSlot<T> this[int index] => this.slots[index];
        public override IInventoryLazyStackSlot<T>[] Slots => this.slots;

        public LazyStackInventory(IInventoryLazyStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        public virtual bool Add(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.CanAdd(item))
                    return slot.Add(item) == 1;
            }

            return false;
        }

        public virtual T[] Add(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.CanAdd(item))
                {
                    var notAdded = slot.Add(item, amount);
                    if (notAdded == 0)
                    {
                        return Array.Empty<T>();
                    }
                    amount = notAdded;
                }
            }

            return Enumerable.Repeat(item, amount).ToArray();
        }

        public virtual T[] AddAt(T item, int index, int amount, bool replace = true)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.CanAdd(item, amount))
            {
                var notAdded = slot.Add(item, amount);
                if(notAdded == 0)
                    return Array.Empty<T>();
            }
            else if(replace && slot.CanReplace(item, amount))
            {
                return slot.Replace(item, amount);
            }

            return Enumerable.Repeat(item, amount).ToArray();
        }

        public virtual T[] Clear()
        {
            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (!slot.IsEmpty)
                {
                    var slotItems = slot.GetAll();
                    items.AddRange(slotItems);
                }
            }
            
            return items.ToArray();
        }

        public virtual T? Get(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].Get().FirstOrDefault();
        }

        public virtual T? Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                    return slot.Get().FirstOrDefault();
            }

            return default;
        }

        public virtual T[] Get(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    var slotItems = slot.Get(amount);

                    items.AddRange(slotItems);
                    amount -= slotItems.Length;
                    if (amount == 0)
                        break;
                }
            }

            return items.ToArray();
        }

        public virtual T[] Get(int index, int amount)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return this.slots[index].Get(amount);
        }

        public virtual T[] GetAll(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    items.AddRange(slotItems);
                }
            }

            return items.ToArray();
        }

        public virtual T[] GetAll(int index)
        {
            throw new NotImplementedException();
        }

        public virtual int GetCount(T item)
        {
            throw new NotImplementedException();
        }

        public virtual void Move(int origin, int target)
        {
            throw new NotImplementedException();
        }
    }
}
