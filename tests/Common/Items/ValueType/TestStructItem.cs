using System;

namespace TheChest.Tests.Common.Items.ValueType
{
    /// <summary>
    /// A simple value-type test item used by the common testing utilities.
    /// Contains an <see cref="Id"/>, <see cref="Name"/>, and <see cref="Description"/>.
    /// Equality is based on the <see cref="Id"/> property.
    /// </summary>
    public readonly struct TestStructItem
    {
        /// <summary>
        /// Gets the unique identifier for the test struct item.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Gets the human-readable name for the test struct item.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the description associated with the test struct item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TestStructItem"/> with the given values.
        /// </summary>
        /// <param name="id">Unique identifier for the struct item.</param>
        /// <param name="name">Name of the struct item.</param>
        /// <param name="description">Description of the struct item.</param>
        public TestStructItem(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="TestStructItem"/>.
        /// Equality is based on the <see cref="Id"/> property.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is TestStructItem))
                return false;

            var item = (TestStructItem)obj;
            return item.Id == this.Id;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="TestStructItem"/>.
        /// </summary>
        /// <returns>A hash code computed from <see cref="Id"/>, <see cref="Name"/>, and <see cref="Description"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description);
        }
    }
}
