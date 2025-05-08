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
            throw new NotImplementedException();
        }

        public virtual T[] Add(T item, int amount)
        {
            throw new NotImplementedException();
        }

        public virtual T[] AddAt(T item, int index, int amount, bool replace = true)
        {
            throw new NotImplementedException();
        }

        public virtual T[] Clear()
        {
            throw new NotImplementedException();
        }

        public virtual T? Get(int index)
        {
            throw new NotImplementedException();
        }

        public virtual T? Get(T item)
        {
            throw new NotImplementedException();
        }

        public virtual T[] Get(T item, int amount)
        {
            throw new NotImplementedException();
        }

        public virtual T[] Get(int index, int amount)
        {
            throw new NotImplementedException();
        }

        public virtual T[] GetAll(T item)
        {
            throw new NotImplementedException();
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
