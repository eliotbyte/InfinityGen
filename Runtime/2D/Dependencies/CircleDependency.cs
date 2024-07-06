using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class CircleDependency<TChunk> : IDependency2D where TChunk : IChunk2D
	{
		public CircleDependency(Circle circle)
		{
			Circle = circle;
		}

		public Circle Circle { get; set; }

		public bool IsLoaded(LayerRegistry<Vector2Int> layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(Circle);
		}

		public void RequestLoad(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, Circle);
		}

		public void RequestUnload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, Circle);
		}
	}
}
