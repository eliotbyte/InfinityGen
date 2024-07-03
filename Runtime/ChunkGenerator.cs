using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public class ChunkGenerator
    {
        private readonly IRandomFactory _random;
        private readonly Dictionary<Type, IChunkLayer> _layersByChunkType = new();
        private readonly HashSet<IChunkViewport> _viewports = new();

        public ChunkGenerator(IRandomFactory random)
        {
            _random = random;
        }

        public void RegisterLayer<TChunk>(int chunkSize)
        {
            _layersByChunkType.Add(typeof(TChunk), new ChunkLayer<TChunk>(chunkSize, _random));
        }

        public void RegisterViewport(IChunkViewport viewport)
        {
            _viewports.Add(viewport);
        }

        public void Generate()
        {
            foreach (IChunkViewport viewport in _viewports)
            {
                if (!viewport.IsActive)
                    continue;

                foreach (IChunkLayer layer in _layersByChunkType.Values)
                {
                    layer.ProcessRequests();
                }
            }
        }
    }
}
