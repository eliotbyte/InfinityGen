using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
    public class ChunkGenerator
    {
        private readonly HashSet<IChunkLayer> _layers = new();
        private readonly HashSet<IChunkViewport> _viewports = new();
        private readonly ChunkRenderPipeline _renderPipeline;

        public ChunkGenerator(IRandomFactory random)
        {
            _renderPipeline = new ChunkRenderPipeline(random);
        }

        public void RegisterLayer(IChunkLayer layer)
        {
            _layers.Add(layer);
        }

        public void RegisterViewport(IChunkViewport viewport)
        {
            _viewports.Add(viewport);
        }

        public void RegisterDependency(IChunkLayer layer, IChunkLayer dependency, int padding)
        {
            layer.AddDependency(dependency, padding);
        }

        public void Generate()
        {
            if (_renderPipeline.IsRendering)
                return;

            foreach (IChunkViewport viewport in _viewports)
            {
                if (!viewport.IsActive)
                    continue;

                foreach (IChunkLayer layer in _layers)
                {
                    IEnumerable<IChunk> chunksForRendering = layer.GetChunksForRendering(new Circle(viewport.Position, viewport.Radius));
                    _renderPipeline.AddChunksToQueue(chunksForRendering, viewport);
                }
            }

            _renderPipeline.Render();
        }
    }
}
