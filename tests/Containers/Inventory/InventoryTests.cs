using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Tests.Common;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventoryTests<T> : BaseTest<T>
    {
        protected readonly IInventoryFactory<T> inventoryFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_SIZE_TEST = 10;
        protected const int MAX_SIZE_TEST = 20;

        public InventoryTests() : base(configure =>
        {
            configure
                .Register<IInventorySlotFactory<T>, InventorySlotFactory<InventorySlot<T>, T>>()
                .Register<IInventoryFactory<T>, InventoryFactory<Inventory<T>, T>>();
        })
        {
            this.inventoryFactory = this.configurations.Resolve<IInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }

        protected int GenerateRandomSize() => this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);

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
