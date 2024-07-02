using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public interface IChunkEntityGenerator<T> where T : IChunkEntity
    {
        List<T> Generate(System.Random random, Rectangle area);
        void Dispose(List<T> entities);
    }
}
