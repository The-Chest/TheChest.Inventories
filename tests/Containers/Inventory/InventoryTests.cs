using System;
using System.Linq;
using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventoryTests<T> : IInventoryTests<T>
    {
        public InventoryTests() : base(configure =>
        {
            configure
                .Register<IInventorySlotFactory<T>, InventorySlotFactory<InventorySlot<T>, T>>()
                .Register<IInventoryFactory<T>, InventoryFactory<Inventory<T>, T>>();
        }) { }

        protected (T First, T Second) CreateDistinctItems()
        {
            if (typeof(T).IsEnum)
            {
                var values = Enum.GetValues(typeof(T)).Cast<T>().Skip(1).Take(2).ToArray();
                if (values.Length >= 2)
                    return (values[0], values[1]);
            }

            var first = this.itemFactory.CreateRandom();
            for (var attempt = 0; attempt < 20; attempt++)
            {
                var second = this.itemFactory.CreateRandom();
                if (!object.Equals(first, second))
                    return (first, second);
            }

            throw new InvalidOperationException($"Could not create distinct items for {typeof(T).FullName}.");
        }
    }
}
