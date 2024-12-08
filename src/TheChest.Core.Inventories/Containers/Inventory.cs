using TheChest.Core.Containers;
using TheChest.Core.Inventories.Containers.Interfaces;
using TheChest.Core.Inventories.Slots.Interfaces;

namespace TheChest.Core.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class Inventory<T> : Container<T>, IInventory<T>
    {
        protected readonly IInventorySlot<T>[] slots;

        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventorySlot{T}"/></param>
        public Inventory(IInventorySlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        public override IInventorySlot<T> this[int index] => this.slots[index];

        public override IInventorySlot<T>[] Slots => this.slots.ToArray();

        /// <summary>
        /// <inheritdoc/>.
        /// </summary>
        /// <param name="items"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentException">When <paramref name="items"/> is empty</exception>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0)
                throw new ArgumentException("No items to add", nameof(items));

            var addedAmount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var added = this.slots[i].Add(items[addedAmount]);
                if (added)
                {
                    addedAmount++;
                    if (addedAmount >= items.Length)
                    {
                        break;
                    }
                }
            }

            if (addedAmount < items.Length)
            {
                return items[addedAmount..];
            }

            return Array.Empty<T>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Add(T item)
        {
            for (int i = 0; i < this.Size ; i ++)
            {
                var added = this.slots[i].Add(item);
                if (added)
                {
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
        public virtual T? AddAt(T item, int index, bool replace = true)
        {
            if (index < 0 || index >= this.Size)
                throw new IndexOutOfRangeException();

            T? result = default;
            if (replace)
            {
                result = this.slots[index].Replace(item);
            }
            else
            {
                var added = this.slots[index].Add(item);
                if (!added)
                {
                    result = item;
                }
            }

            return result;
        }

        public virtual T[] Clear()
        {
            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                var item = this.slots[i].Get();
                if(item != null)
                {
                    items.Add(item);
                }
            }
            return items.ToArray();
        }

        public virtual T[] GetAll(T item)
        {
            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    items.Add(this.slots[i].Get()!);
                }
            }
            return items.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="IndexOutOfRangeException">When index is smaller than zero or bigger than the container size</exception>
        public virtual T? Get(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new IndexOutOfRangeException();

            return this.slots[index].Get();
        }

        public virtual T? Get(T item)
        {
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
        public virtual T[] Get(T item, int amount = 1)
        {
            if(amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            for (int i = 0; i < this.Size; i++)
            {
                if (!this.slots[i].Contains(item))
                    continue;

                var slotItem = this.slots[i].Get();
                if(slotItem == null)
                    continue;

                items.Add(slotItem);

                if (items.Count == amount)
                    break;
            }
            return items.ToArray();
        }

        public virtual int GetCount(T item)
        {
            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="origin"><inheritdoc/></param>
        /// <param name="target"><inheritdoc/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            var item = this.slots[origin].Get();

            var oldItem = this.slots[target].Replace(item);
            this.slots[origin].Replace(oldItem);
        }
    }
}
