using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkGenerator2D
	{
		private readonly HashSet<IChunkViewport> _viewports = new();

        public LayerRegistry2D LayerRegistry { get; } = new();

        public void RegisterLayer<TChunk>(int chunkSize, IChunkFactory2D<TChunk> chunkFactory) where TChunk : IChunk
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

				foreach (IChunkLayer<Vector2Int> layer in LayerRegistry.AllLayers)
				{
					layer.RequestLoad(viewport, new Circle(viewport.Position, viewport.Radius));
					layer.ProcessRequests();
				}
			}
		}
	}
}
