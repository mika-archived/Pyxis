namespace Pyxis.Models.Database
{
    public class Cache
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public ulong Size { get; set; }
        public CacheType Type { get; set; }
    }
}