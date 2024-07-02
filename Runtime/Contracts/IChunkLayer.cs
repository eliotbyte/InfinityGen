using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public interface IChunkLayer
    {
        Type Type { get; }
        void AddDependency(IChunkLayer dependentLayer, int padding);
        IEnumerable<IChunk> GetChunksForRendering(Circle userArea);
        IEnumerable<IChunk> GetChunksInArea(Rectangle area);
    }
}
