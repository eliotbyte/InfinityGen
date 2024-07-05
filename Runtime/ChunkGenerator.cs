using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public class ChunkGenerator
	{
		private readonly HashSet<IChunkViewport> _viewports = new();

        public LayerRegistry LayerRegistry { get; } = new();

        public void RegisterLayer<TChunk>(int chunkSize, IChunkFactory<TChunk> chunkFactory) where TChunk : IChunk
		{
			LayerRegistry.Register(chunkSize, chunkFactory);
		}

		public void RegisterViewport(IChunkViewport viewport)
		{
			_viewports.Add(viewport);
		}

		public void Generate()
		{
			foreach (IChunkViewport viewport in _viewports)
			{
				// TODO: Request unload when viewport is not acitve
				if (!viewport.IsActive)
					continue;

				foreach (IChunkLayer layer in LayerRegistry.AllLayers)
				{
					layer.RequestLoad(viewport, new Circle(viewport.Position, viewport.Radius));
					layer.ProcessRequests();
				}
			}
		}
	}
}
