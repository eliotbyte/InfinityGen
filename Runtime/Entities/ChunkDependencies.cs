using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public struct ChunkDependencies
    {
        public Type EntityType { get; }
        public IReadOnlyList<IChunk> Chunks { get; }
        public int Padding { get; }

        public ChunkDependencies(Type type, int padding = 0)
        {
            EntityType = type;
            Padding = padding;
            Chunks = new List<IChunk>();
        }

        public ChunkDependencies(Type type, IEnumerable<IChunk> chunks, int padding = 0)
        {
            EntityType = type;
            Padding = padding;
            Chunks = new List<IChunk>(chunks);
        }
    }
}
