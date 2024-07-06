using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkGenerator2D
	{
		private readonly HashSet<IChunkViewport> _viewports = new();

		public LayerRegistry<Vector2Int> LayerRegistry { get; } = new(new DistanceComparer2D());

		public void RegisterLayer<TChunk>(int chunkSize, IChunkFactory2D<TChunk> chunkFactory) where TChunk : IChunk2D
		{
			LayerRegistry.Register(chunkSize, chunkFactory);
		}

		public void RegisterViewport(IChunkViewport viewport)
		{
			_viewports.Add(viewport);
		}

		public void Generate()
		{
			foreach (IChunkLayer<Vector2Int> layer in LayerRegistry.AllLayers)
			{
				Vector2Int processingCenter = Vector2Int.zero;

				foreach (IChunkViewport viewport in _viewports)
				{
					layer.RequestUnload(viewport, new Circle(viewport.PreviousPosition, viewport.Radius));

					if (viewport.IsActive)
					{
						processingCenter = Vector2Int.RoundToInt(viewport.Position / layer.ChunkSize);
						layer.RequestLoad(viewport, new Circle(viewport.Position, viewport.Radius));
					}
				}

				layer.ProcessRequests(processingCenter);
			}
		}
	}
}
