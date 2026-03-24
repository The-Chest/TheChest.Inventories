using TheChest.Inventories.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories.Interfaces;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Factories
{
    public class StackInventoryFactory<Inventory, Item> : IStackInventoryFactory<Item> 
        where Inventory : StackInventory<Item>
    {
        protected readonly IInventoryStackSlotFactory<Item> slotFactory;

        public StackInventoryFactory(IInventoryStackSlotFactory<Item> slotFactory) 
        {
            this.slotFactory = slotFactory;
        }

        public virtual IStackInventory<Item> EmptyContainer(int size = 20, int stackSize = 10)
        {
            var inventoryType = typeof(Inventory).GetContainerType(typeof(IStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryStackSlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.EmptySlot(stackSize));

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );
            
            return (IStackInventory<Item>)inventory!;
        }

        public virtual IStackInventory<Item> FullContainer(int size, int stackSize, Item item = default!)
        {
            var inventoryType = typeof(Inventory).GetContainerType(typeof(IStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryStackSlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.WithItem(item, stackSize, stackSize));

            var inventory = Activator.CreateInstance(inventoryType, slots);

            return (IStackInventory<Item>)inventory!;
        }

        public virtual IStackInventory<Item> ShuffledItemsContainer(int size, int stackSize, params Item[] items)
        {
            if (items.Length > size * stackSize)
                throw new ArgumentException($"Item amount ({items.Length}) cannot be bigger than the container size ({size})");

            var inventoryType = typeof(Inventory).GetContainerType(typeof(IStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryStackSlot<Item>>();

            var slots = slotType
                .CreateSlots(
                    size: size,
                    factory:
                        index => index < items.Length
                            ? slotFactory.WithItem(items[index], stackSize, stackSize)
                            : slotFactory.EmptySlot(),
                    shuffle: true
                );

            var inventory = Activator.CreateInstance(inventoryType, slots);

            return (IStackInventory<Item>)inventory!;
        }
    }
}
