using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public class Chunk<T> : IChunk
    {
        private readonly HashSet<object> _loadRequests = new();

        public Chunk(ChunkPosition chunkPosition)
        {
            Position = chunkPosition;
        }

        public void AddLoadRequest(object source)
        {
            _loadRequests.Add(source);
        }

        public void RemoveLoadRequest(object source)
        {
            _loadRequests.Remove(source);
        }

        public int LoadRequests => _loadRequests.Count;

        public ChunkPosition Position { get; }

        public ChunkStatus Status { get; private set; }

        public void Load(IRandomFactory randomFactory)
        {
            Status = ChunkStatus.Processing;
        }

        public void Unload()
        {
            Status = ChunkStatus.Processing;
        }
    }
}
