namespace TheChest.Inventories.Tests.Common.Extensions
{
    internal static class IndexExtensions
    {
        internal static (int origin, int target) GetRandomOriginAndTarget(this Random random, int size)
        {
            if (size <= 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than 1.");
            
            var origin = random.Next(0, size);

            int target;
            do
            {
                target = random.Next(0, size);
            } while (target == origin);

            return (origin, target);
        }
    }
}
