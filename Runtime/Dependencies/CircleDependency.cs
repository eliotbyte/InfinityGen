using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class CircleDependency<TChunk> : IDependency2D where TChunk : IChunk2D
	{
		private readonly Circle _circle;

		public CircleDependency(LayerRegistry2D layerRegistry, Circle circle)
		{
			_circle = circle;
		}

		public bool IsLoaded(LayerRegistry<Vector2Int> layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(_circle);
		}

		public void Load(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, _circle);
		}

		public void Unload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, _circle);
		}
	}
}
