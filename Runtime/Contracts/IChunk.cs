using System.Collections;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public interface IChunk
    {
        ChunkPosition Position { get; }
        bool Loaded { get; }
        void Load(IRandomFactory random);
        void Unload();
        bool DependenciesLoaded();
        IList GetEntities();
    }
}
