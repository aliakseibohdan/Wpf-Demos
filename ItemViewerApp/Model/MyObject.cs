namespace ItemViewerApp.Model
{
    /// <summary>
    /// Represents the data model for a single named item with a corresponding count.
    /// </summary>
    public class MyObject
    {
        /// <summary>
        /// Gets or sets the name of the item. This field is required.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the count of items.
        /// </summary>
        public int ItemsCount { get; set; }
    }
}