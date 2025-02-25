using TheChest.Core.Inventories.Containers;
using TheChest.Core.Inventories.Containers.Interfaces;
using TheChest.Core.Inventories.Slots.Interfaces;
using TheChest.Core.Tests.Containers.Factories;

namespace TheChest.Core.Inventories.Tests.Containers.Factories
{
    public class StackInventoryFactory<T, Y> : StackContainerFactory<T, Y>, IStackInventoryFactory<Y>
        where T : StackInventory<Y>
    {
        public StackInventoryFactory(IStackSlotFactory<Y> slotFactory) : base(slotFactory) { }

        private static Type GetInventoryType()
        {
            var inventoryType = typeof(T);
            if (!typeof(IStackInventory<Y>).IsAssignableFrom(inventoryType))
            {
                throw new ArgumentException($"Type '{inventoryType.FullName}' does not implement IInventory<{typeof(Y).Name}>.");
            }

            return inventoryType;
        }

        private static Type GetSlotTypeFromConstructor()
        {
            var inventoryType = GetInventoryType();

            var constructor = inventoryType.GetConstructors()
                    .FirstOrDefault(ctor =>
                    {
                        var parameters = ctor.GetParameters();
                        return
                            parameters.Length == 1 &&
                            parameters[0].ParameterType.IsArray &&
                            typeof(IInventoryStackSlot<Y>).IsAssignableFrom(parameters[0].ParameterType.GetElementType());
                    })
                    ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a suitable constructor.");

            var slotParameter = constructor.GetParameters().FirstOrDefault(x => x.Name == "slots")
                ?? throw new ArgumentException($"Inventory type '{inventoryType.FullName}' does not have a constructor with IInventorySlot<{typeof(Y).Name}>[].");

            var slotType = slotParameter.ParameterType.GetElementType();
            if (!typeof(IInventoryStackSlot<Y>).IsAssignableFrom(slotType))
            {
                throw new ArgumentException($"Type '{slotType!.FullName}' does not implement IInventoryStackSlot<{typeof(Y).Name}>.");
            }

            return slotType;
        }

        public override IStackInventory<Y> EmptyContainer(int size)
        {
            var containerType = GetInventoryType();
            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int index = 0; index < size; index++)
            {
                slots.SetValue(slotFactory.EmptySlot(), index);
            }

            var container = Activator.CreateInstance(
                type: containerType,
                args: new object[1] { slots }
            );
            return (IStackInventory<Y>)container!;
        }

        public override IStackInventory<Y> FullContainer(int size, int stackSize, Y item)
        {
            var containerType = GetInventoryType();
            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int i = 0; i < size; i++)
            {
                slots.SetValue(slotFactory.FullSlot(item), i);
            }

            var container = Activator.CreateInstance(containerType, slots);

            return (IStackInventory<Y>)container!;
        }

        public override IStackInventory<Y> ShuffledItemsContainer(int size, int stackSize, params Y[] items)
        {
            if (items.Length > size)
            {
                throw new ArgumentException($"Item amount ({items.Length}) cannot be bigger than the container size ({size})");
            }

            var containerType = GetInventoryType();

            var slotType = GetSlotTypeFromConstructor();

            Array slots = Array.CreateInstance(slotType, size);
            for (int index = 0; index < size; index++)
            {
                if (index < items.Length)
                {
                    slots.SetValue(
                        slotFactory.FullSlot(items[index]),
                        index
                    );
                }
                else
                {
                    slots.SetValue(
                        slotFactory.EmptySlot(),
                        index
                    );
                }
            }
            ShuffleItems(slots);

            var container = Activator.CreateInstance(containerType, slots);

            return (IStackInventory<Y>)container!;
        }
    }
}
