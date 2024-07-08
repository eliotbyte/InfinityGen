using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkGenerator2D
	{
		private readonly HashSet<IChunkViewport> _viewports = new();
		private readonly DistanceCost2D _distanceCost = new();

		public LayerRegistry<Vector2Int> LayerRegistry { get; } = new();

		public void RegisterLayer<TChunk>(int chunkSize, IChunkFactory2D<TChunk> chunkFactory, int processesLimit = 1, float loadCoefficient = 1f) where TChunk : IChunk2D
		{
			LayerRegistry.Register(chunkSize, chunkFactory, processesLimit, loadCoefficient);
		}

		public void RegisterViewport(IChunkViewport viewport)
		{
			_viewports.Add(viewport);
		}

		public void Generate()
		{
			foreach (IChunkLayer<Vector2Int> layer in LayerRegistry.AllLayers)
			{
				IPositionCost<Vector2Int> cost = NoCost<Vector2Int>.Instance;

				foreach (IChunkViewport viewport in _viewports)
				{
					layer.RequestUnload(viewport, new Circle(viewport.PreviousPosition, viewport.Radius));

					if (viewport.IsActive)
					{
						cost = _distanceCost;
						_distanceCost.Target = Vector2Int.RoundToInt(viewport.Position);
						_distanceCost.ChunkSize = layer.ChunkSize;
						layer.RequestLoad(viewport, new Circle(viewport.Position, viewport.Radius));
					}
				}

				layer.ProcessRequests(cost);
			}
		}
	}
}
