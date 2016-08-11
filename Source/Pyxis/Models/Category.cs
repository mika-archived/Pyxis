namespace Pyxis.Models
{
    internal class Category
    {
        public string Key { get; }
        public string Name { get; }
        public int Index { get; }

        public Category(string key, string name, int index)
        {
            Key = key;
            Name = name;
            Index = index;
        }
    }
}