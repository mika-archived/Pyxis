namespace Pyxis.Models
{
    public class AdaptiveSize
    {
        public int MinWindowWidth { get; }
        public int Size { get; }

        public AdaptiveSize(int minWindowWidth, int size)
        {
            MinWindowWidth = minWindowWidth;
            Size = size;
        }
    }
}