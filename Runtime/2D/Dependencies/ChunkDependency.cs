using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkDependency<TChunk> : IDependency2D where TChunk : IChunk2D
	{
		public ChunkDependency(Vector2Int position = default)
		{
			Position = position;
		}

		public Vector2Int Position { get; set; }

		public bool IsLoaded(LayerRegistry<Vector2Int> layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(Position);
		}

		public void RequestLoad(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, Position);
		}

		public void RequestUnload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, Position);
		}
	}
}
