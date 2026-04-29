using System;

namespace TheChest.Tests.Common.Items.ReferenceType
{
    /// <summary>
    /// A simple reference-type test item used within the test utilities.
    /// </summary>
    public sealed class TestItem
    {
        /// <summary>
        /// Gets the unique identifier for the test item.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Gets the name for the test item.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the description associated with the test item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TestItem"/> with the specified values.
        /// </summary>
        /// <param name="id">Unique identifier for the item.</param>
        /// <param name="name">Name of the item.</param>
        /// <param name="description">Description of the item.</param>
        public TestItem(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TestItem"/> with empty string properties.
        /// </summary>
        public TestItem()
        {
            this.Id = "";
            this.Name = "";
            this.Description = "";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="TestItem"/>.
        /// Equality is based solely on the <see cref="Id"/> property.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the objects are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is TestItem))
                return false;

            var item = obj as TestItem;
            return item?.Id == this.Id;
        }

        /// <summary>
        /// Gets a hash code for the current <see cref="TestItem"/>.
        /// </summary>
        /// <returns>A hash code based on <see cref="Id"/>, <see cref="Name"/>, and <see cref="Description"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description);
        }
    }
}
