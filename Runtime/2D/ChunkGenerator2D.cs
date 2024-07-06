using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkGenerator2D
	{
		private readonly HashSet<IChunkViewport> _viewports = new();
		private readonly DistanceComparer2D _comparer = new();

		public LayerRegistry<Vector2Int> LayerRegistry { get; } = new();

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
				IPositionsComparer<Vector2Int> comparer = RandomComparer<Vector2Int>.Instance;

				foreach (IChunkViewport viewport in _viewports)
				{
					layer.RequestUnload(viewport, new Rectangle(viewport.PreviousPosition, viewport.Radius, viewport.Radius));

					if (viewport.IsActive)
					{
						comparer = _comparer;
						_comparer.Target = viewport.Position;
						_comparer.ChunkSize = layer.ChunkSize;
						layer.RequestLoad(viewport, new Rectangle(viewport.Position, viewport.Radius, viewport.Radius));
					}
				}

				layer.ProcessRequests(comparer);
			}
		}
	}
}
