﻿using TheChest.Core.Tests.Slots.Factories.Interfaces;
using TheChest.Inventories.Tests.Containers;

namespace TheChest.ConsoleApp.Tests.Containers
{
    [TestFixtureSource(nameof(FixtureArgs))]
    public class InventoryTests : InventoryTests<Item>
    {
        static readonly object[] FixtureArgs = {
            new object[] {
                new InventoryFactory<Inventory, Item>(
                    new InventorySlotFactory<InventorySlot, Item>()
                ),
                new SlotItemFactory<Item>(),
            }
        };
        public InventoryTests(IInventoryFactory<Item> containerFactory, ISlotItemFactory<Item> itemFactory)
            : base(containerFactory, itemFactory)
        {
        }
    }
}
