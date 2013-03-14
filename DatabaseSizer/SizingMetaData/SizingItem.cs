namespace DatabaseSizer.SizingMetaData
{
    /// <summary>
    ///     Base class for all sizing items
    /// </summary>
    public class SizingItem
    {
        public SizingItem(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}