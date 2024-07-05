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

		public void Load(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, Circle);
		}

		public void Unload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, Circle);
		}
	}
}
