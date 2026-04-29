using TheChest.Inventories.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Extensions;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.Factories
{
    public class LazyStackInventoryFactory<Inventory, Item> : ILazyStackInventoryFactory<Item>
        where Inventory : LazyStackInventory<Item>
    {
        private readonly IInventoryLazyStackSlotFactory<Item> slotFactory;
        public LazyStackInventoryFactory(IInventoryLazyStackSlotFactory<Item> slotFactory)
        {
            this.slotFactory = slotFactory;
        }

        public virtual ILazyStackInventory<Item> EmptyContainer(int size = 20, int stackSize = 5)
        {
            var inventoryType = typeof(Inventory).GetContainerType(typeof(ILazyStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryLazyStackSlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.Empty(stackSize));

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );
            
            return (ILazyStackInventory<Item>)inventory!;
        }

        public virtual ILazyStackInventory<Item> FullContainer(int size, int stackSize, Item item)
        {
            var inventoryType = typeof(Inventory).GetContainerType(typeof(ILazyStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryLazyStackSlot<Item>>();

            var slots = slotType.CreateSlots(size, _ => slotFactory.WithItem(item, stackSize, stackSize));

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );
            return (ILazyStackInventory<Item>)inventory!;
        }

        public virtual ILazyStackInventory<Item> ShuffledItemsContainer(int size, int stackSize, params Item[] items)
        {
            if(items.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(items), "Items cannot be empty.");

            var inventoryType = typeof(Inventory).GetContainerType(typeof(ILazyStackInventory<Item>));
            var slotType = inventoryType.GetSlotTypeByConstructor<IInventoryLazyStackSlot<Item>>();


            var slots = slotType
                .CreateSlots(
                    size: size,
                    factory:
                        index => index < items.Length
                            ? slotFactory.WithItem(
                                item: items[index],
                                amount: Random.Shared.Next(1, stackSize),
                                maxAmount: stackSize
                            )
                            : slotFactory.Empty(stackSize),
                    shuffle: true
                );

            var inventory = Activator.CreateInstance(
                type: inventoryType,
                args: new object[1] { slots }
            );

            return (ILazyStackInventory<Item>)inventory!;
        }
    }
}
