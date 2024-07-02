using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
    public class ChunkLayer<T> : IChunkLayer where T : IChunkEntity
    {
        public int ChunkSize { get; }
        public Type Type => typeof(T);

        private Dictionary<IChunkLayer, int> _dependencies = new();
        private Dictionary<Vector2Int, IChunk> _chunks = new();
        private IChunkEntityGenerator<T> _generator;

        public ChunkLayer(int chunkSize, IChunkEntityGenerator<T> generator)
        {
            ChunkSize = chunkSize;
            _generator = generator;
        }

        public void AddDependency(IChunkLayer dependentLayer, int padding)
        {
            _dependencies[dependentLayer] = padding;
        }

        public IEnumerable<IChunk> GetChunksForRendering(Circle userArea)
        {
            HashSet<IChunk> chunksForRendering = new();
            Vector2Int[] chunkPositions = GetChunksInRadius(ChunkSize, userArea.Position, userArea.Radius);

            foreach (Vector2Int position in chunkPositions)
            {
                if (!_chunks.TryGetValue(position, out var chunk))
                    chunk = CreateChunk(position);

                if (chunk.DependenciesLoaded())
                    chunksForRendering.Add(chunk);
            }

            return chunksForRendering;
        }

        public IEnumerable<IChunk> GetChunksInArea(Rectangle area)
        {
            HashSet<IChunk> chunks = new();
            Vector2Int[] chunkPositions = GetChunksInArea(ChunkSize, area);

            foreach (Vector2Int position in chunkPositions)
            {
                if (!_chunks.TryGetValue(position, out var chunk))
                    chunk = CreateChunk(position);

                chunks.Add(chunk);
            }

            return chunks;
        }

        private IChunk CreateChunk(Vector2Int position)
        {
            if (_dependencies.Count == 0)
            {
                IChunk newChunk = new Chunk<T>(new ChunkPosition(position, ChunkSize), _generator);
                _chunks[position] = newChunk;
                return newChunk;
            }
            else
            {
                List<ChunkDependencies> dependencies = new();
                foreach ((IChunkLayer layer, int padding) in _dependencies)
                {
                    IEnumerable<IChunk> chunks = layer.GetChunksInArea(new Rectangle(
                        position.x * ChunkSize - padding,
                        position.y * ChunkSize - padding,
                        ChunkSize + padding * 2));
                    ChunkDependencies dependency = new(layer.Type, chunks, padding);
                    dependencies.Add(dependency);
                }

                IChunk newChunk = new Chunk<T>(new ChunkPosition(position, ChunkSize), _generator, dependencies);
                _chunks[position] = newChunk;
                return newChunk;
            }
        }

        private static Vector2Int[] GetChunksInArea(int chunkSize, Rectangle area)
        {
            List<Vector2Int> chunkPositions = new();

            float xEnd = area.X + area.Width;
            float yEnd = area.Y + area.Height;

            int minChunkX = Mathf.FloorToInt(area.X / chunkSize);
            int maxChunkX = Mathf.FloorToInt((xEnd - 1) / chunkSize);
            int minChunkY = Mathf.FloorToInt(area.Y / chunkSize);
            int maxChunkY = Mathf.FloorToInt((yEnd - 1) / chunkSize);

            for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
                for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
                    chunkPositions.Add(new Vector2Int(chunkX, chunkY));

            return chunkPositions.ToArray();
        }

        private static Vector2Int[] GetChunksInRadius(int chunkSize, Vector2 userPosition, float radius)
        {
            float playerX = userPosition.x;
            float playerY = userPosition.y;
            List<Vector2Int> chunks = new();

            int minChunkX = Mathf.FloorToInt((playerX - radius) / chunkSize);
            int maxChunkX = Mathf.FloorToInt((playerX + radius) / chunkSize);
            int minChunkY = Mathf.FloorToInt((playerY - radius) / chunkSize);
            int maxChunkY = Mathf.FloorToInt((playerY + radius) / chunkSize);

            for (int x = minChunkX; x <= maxChunkX; x++)
            for (int y = minChunkY; y <= maxChunkY; y++)
                if (IsChunkInRadius(x, y, chunkSize, userPosition, radius))
                    chunks.Add(new Vector2Int(x, y));

            return chunks.ToArray();
        }

        private static bool IsChunkInRadius(int chunkX, int chunkY, int chunkSize, Vector2 userPosition, float radius)
        {
            Vector2[] chunkCorners = {
                new(chunkX * chunkSize, chunkY * chunkSize),
                new((chunkX + 1) * chunkSize, chunkY * chunkSize),
                new(chunkX * chunkSize, (chunkY + 1) * chunkSize),
                new((chunkX + 1) * chunkSize, (chunkY + 1) * chunkSize)
            };

            foreach (Vector2 corner in chunkCorners)
                if (Vector2.Distance(corner, userPosition) <= radius)
                    return true;

            float chunkXMin = chunkX * chunkSize;
            float chunkXMax = (chunkX + 1) * chunkSize;
            float chunkYMin = chunkY * chunkSize;
            float chunkYMax = (chunkY + 1) * chunkSize;

            return (chunkXMin <= userPosition.x && userPosition.x <= chunkXMax &&
                    chunkYMin <= userPosition.y && userPosition.y <= chunkYMax);
        }
    }
}
