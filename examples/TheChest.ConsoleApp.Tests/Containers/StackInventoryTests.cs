using TheChest.ConsoleApp.Inventories.Stack;
using TheChest.ConsoleApp.Items;
using TheChest.Core.Inventories.Tests.Containers;
using TheChest.Core.Tests.Slots.Factories.Interfaces;

namespace TheChest.ConsoleApp.Tests.Containers
{
    [TestFixtureSource(nameof(FixtureArgs))]
    public class StackInventoryTests : StackInventoryTests<Item>
    {
        static readonly object[] FixtureArgs = new object[]{
            new object[] {
                new StackInventoryFactory<StackInventory, Item>(
                    new InventoryStackSlotFactory<InventoryStackSlot, Item>()
                ),
                new SlotItemFactory<Item>(),
            },
        };

        public StackInventoryTests(IStackInventoryFactory<Item> containerFactory, ISlotItemFactory<Item> itemFactory) : base(containerFactory, itemFactory)
        {
        }
    }
}
