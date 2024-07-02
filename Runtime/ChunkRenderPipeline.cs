using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EliotByte.InfinityGen
{
    public class ChunkRenderPipeline
    {
        private struct ChunkDistance
        {
            public IChunk Chunk { get; }
            public float Distance { get; }

            public ChunkDistance(IChunk chunk, float distance)
            {
                Chunk = chunk;
                Distance = distance;
            }
        }

        public bool IsRendering { get; private set; }

        private readonly IRandomFactory _random;
        private readonly HashSet<IChunk> _chunkQueue = new();
        private readonly HashSet<IChunk> _tempChunkQueue = new();
        private readonly List<ChunkDistance> _tempUnloadedChunkDistances = new();

        public ChunkRenderPipeline(IRandomFactory random)
        {
            _random = random;
        }

        public void AddChunksToQueue(IEnumerable<IChunk> chunks, IChunkViewport viewport)
        {
            if (IsRendering)
                throw new InvalidOperationException("Chunks are already in the process of rendering.");

            foreach (IChunk chunk in chunks)
            {
                _tempChunkQueue.Add(chunk);

                if (chunk.Loaded)
                    continue;

                float distance = Vector2.Distance(chunk.Position.GlobalPosition, viewport.Position);
                _tempUnloadedChunkDistances.Add(new ChunkDistance(chunk, distance));
            }
        }

        public void Render()
        {
            if (IsRendering)
                throw new InvalidOperationException("Chunks are already in the process of rendering.");

            if (_tempUnloadedChunkDistances.Count == 0)
                return;

            IsRendering = true;

            SortChunksByDistance(_tempUnloadedChunkDistances);
            LoadChunks(_tempUnloadedChunkDistances);
            UnloadChunks();
            SaveChunkQueue();
            _tempUnloadedChunkDistances.Clear();
            _tempChunkQueue.Clear();
            IsRendering = false;
        }

        private void LoadChunks(List<ChunkDistance> chunkDistances)
        {
            foreach (ChunkDistance chunkDistance in chunkDistances)
                chunkDistance.Chunk.Load(_random);
        }

        private void UnloadChunks()
        {
            foreach (IChunk chunk in _chunkQueue)
                if (!_tempChunkQueue.Contains(chunk))
                    chunk.Unload();
        }

        private static void SortChunksByDistance(List<ChunkDistance> chunkDistances)
        {
            chunkDistances.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        private void SaveChunkQueue()
        {
            _chunkQueue.Clear();
            foreach (IChunk chunk in _tempChunkQueue)
                _chunkQueue.Add(chunk);
        }
    }
}
