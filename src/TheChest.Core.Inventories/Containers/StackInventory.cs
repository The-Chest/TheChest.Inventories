using TheChest.Core.Containers;
using TheChest.Core.Inventories.Containers.Interfaces;
using TheChest.Core.Inventories.Slots.Interfaces;

namespace TheChest.Core.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class StackInventory<T> : StackContainer<T>, IStackInventory<T>
    {
        protected readonly IInventoryStackSlot<T>[] slots;

        public override IInventoryStackSlot<T> this[int index] => this.slots[index];

        public override IInventoryStackSlot<T>[] Slots => this.slots.ToArray();

        /// <summary>
        /// Creates an Inventory with <see cref="IInventoryStackSlot{T}"/> slots
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc/></exception>
        public StackInventory(IInventoryStackSlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns>true if is possible to add the items</returns>
        /// <exception cref="ArgumentNullException">When param <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            for (var i = 0; i < this.Size; i++)
            {
                if (this.slots[i].CanAdd(item))
                {
                    this.slots[i].Add(ref item);
                    return true; 
                }
            }

            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="replace"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual T[] AddAt(T item, int index, bool replace = true)
        {
            if (index > this.Size || index <= 0)
                throw new IndexOutOfRangeException();
            
            var slot = this.Slots[index];

            if (slot.CanAdd(item))
                slot.Add(ref item);

            if (slot.CanReplace(item) && replace)
                return slot.Replace(ref item);

            return Array.Empty<T>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentException">When the param array is empty</exception>
        public virtual T[] Add(params T[] items)
        {
            if(items.Length == 0) 
                throw new ArgumentException("No items to add", nameof(items));

            for (var i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.CanAdd(items))
                {
                    slot.Add(ref items);
                    if(items.Length == 0)
                        break;
                }
            }

            return items;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// If <paramref name="replace"/> is true, it replaces the item from <paramref name="index"/>
        /// </para>
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="replace"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T[] AddAt(T[] items, int index, bool replace = true)
        {
            if(items.Length == 0)
                throw new ArgumentException("No items to be added", nameof(items));

            if (index <= 0 || index > this.Size)
                throw new IndexOutOfRangeException();

            var slot = this.slots[index];

            if (slot.CanAdd(items))
                slot.Add(ref items);

            if (replace && slot.CanReplace(items))
                return slot.Replace(ref items);

            return items;
        }

        public virtual T[] Clear()
        {
            var items = new List<T>();

            for (int i = 0; i < this.Size; i++)
            {
                items.AddRange(this.slots[i].GetAll());
            }

            return items.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Inventory Size or smaller than zero</exception>
        public virtual T[] GetAll(int index)
        {
            if (index > this.Size || index <= 0)
                throw new IndexOutOfRangeException();

            return this.slots[index].GetAll();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    items.AddRange(this.slots[i].GetAll());
                }
            }

            return items.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T? Get(int index)
        {
            if (index > this.Size || index < 0)
                throw new IndexOutOfRangeException();
            
            return this.slots[index].Get();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T? Get(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    return this.slots[i].Get();
                }
            }
            return default;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var remainingAmount = amount;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    var slotItems = this.slots[i].Get(remainingAmount);
                    items.AddRange(slotItems);
                    remainingAmount -= slotItems.Length;
                    if (remainingAmount <= 0)
                        break;
                }

            }

            return items.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="amount"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Inventory Size or smaller than zero</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(int index, int amount)
        {
            if (index > this.Size || index < 0)
                throw new IndexOutOfRangeException();

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return this.slots[index].Get(amount);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var amount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    amount += this.slots[i].StackAmount;
                }
            }
            return amount;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="origin"><inheritdoc/></param>
        /// <param name="target"><inheritdoc/></param>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));

            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            var items = this.slots[origin].GetAll();
            var oldItems = this.slots[target].Replace(ref items);
            this.slots[origin].Replace(ref oldItems);
        }
    }
}
