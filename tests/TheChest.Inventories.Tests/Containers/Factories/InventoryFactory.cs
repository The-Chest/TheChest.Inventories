using TheChest.Inventories.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Tests.Common.Extensions.Containers;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories.Interfaces;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Factories
{
    public class InventoryFactory<Inventory, Item> : IInventoryFactory<Item>
        where Inventory : Inventory<Item>
    {
        private readonly IInventorySlotFactory<Item> slotFactory;
        public InventoryFactory(IInventorySlotFactory<Item> slotFactory) 
        {
            this.slotFactory = slotFactory;
        }

        public virtual IInventory<Item> EmptyContainer(int size = 20)
        {
            var inventoryType = typeof(Inventory).GetContainerType(typeof(IInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventorySlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.EmptySlot());

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );
            
            return (IInventory<Item>)inventory!;
        }

        public virtual IInventory<Item> FullContainer(int size, Item item)
        {
            if (item is null)
                throw new ArgumentException("Item cannot be null");

            var inventoryType = typeof(Inventory).GetContainerType(typeof(IInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventorySlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.FullSlot(item));

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );
            
            return (IInventory<Item>)inventory!;
        }

        public virtual IInventory<Item> ShuffledItemContainer(int size, Item item)
        {
            if (item is null)
                throw new ArgumentException("Item cannot be null");

            var inventoryType = typeof(Inventory).GetContainerType(typeof(IInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventorySlot<Item>>();

            var slots = slotType
                .CreateSlots(
                    size: size,
                    factory:
                        index => index == new Random().Next(0, size - 1)
                            ? slotFactory.FullSlot(item)
                            : slotFactory.EmptySlot(),
                    shuffle: true
                );

            var inventory = Activator.CreateInstance(inventoryType, slots);

            return (IInventory<Item>)inventory!;
        }

        public virtual IInventory<Item> ShuffledItemsContainer(int size, params Item[] items)
        {
            if (items.Length > size)
                throw new ArgumentException($"Item amount ({items.Length}) cannot be bigger than the container size ({size})");

            var inventoryType = typeof(Inventory).GetContainerType(typeof(IInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventorySlot<Item>>();

            var slots = slotType
                .CreateSlots(
                    size: size,
                    factory:
                        index => index < items.Length
                            ? slotFactory.FullSlot(items[index])
                            : slotFactory.EmptySlot(),
                    shuffle: true
                );

            var inventory = Activator.CreateInstance(inventoryType, slots);

            return (IInventory<Item>)inventory!;
        }
    }
}
