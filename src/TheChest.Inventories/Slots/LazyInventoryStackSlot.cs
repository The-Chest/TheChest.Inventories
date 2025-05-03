using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots
{
    public class LazyInventoryStackSlot<T> : ILazyInventoryStackSlot<T>
    {
        protected T? content;
        public virtual T[] Content =>
            this.content is null ?
            Array.Empty<T>() :
            Enumerable.Repeat(this.content, this.StackAmount).ToArray();

        public virtual int StackAmount
        {
            get;
            protected set;
        }

        public virtual int MaxStackAmount
        {
            get;
            protected set;
        }

        public bool IsFull => this.content is not null && this.StackAmount == this.MaxStackAmount;

        public bool IsEmpty => this.content is null || this.StackAmount == 0;

        public LazyInventoryStackSlot(T? content, int amount, int maxStackAmount)
        {
            this.content = content;
            this.StackAmount = amount;
            this.MaxStackAmount = maxStackAmount;
        }

        protected void SetContent(T? item, int amount)
        {
            this.content = item;
            this.StackAmount = amount;
        }

        public virtual int Add(T item, int amount = 1)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (this.IsFull)
                return amount;
            if (!this.IsEmpty && !this.content!.Equals(item))
                return amount;

            var leftAmount = 0;
            if(this.StackAmount + amount > this.MaxStackAmount)
            {
                leftAmount = this.StackAmount + amount - this.MaxStackAmount;
                this.SetContent(this.content, this.MaxStackAmount);
            }
            else
            {
                this.SetContent(this.content, this.StackAmount + amount);
            }

            return leftAmount;
        }

        public virtual bool CanAdd(T item, int amount = 1)
        {
            if(item is null)
                return false;

            if (this.IsFull)
                return false;

            if (amount <= 0)
                return false;

            if (!this.IsEmpty)
                return this.content!.Equals(item);

            return true;
        }

        public virtual bool CanReplace(T item, int amount = 1)
        {
            if (item is null)
                return false;

            if (amount <= 0 || amount > this.MaxStackAmount)
                return false;

            return true;
        }

        public virtual bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public virtual T[] Get(int amount = 1)
        {
            throw new NotImplementedException();
        }

        public virtual T[] GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual T[] Replace(T item, int amount = 1)
        {
            throw new NotImplementedException();
        }
    }
}
