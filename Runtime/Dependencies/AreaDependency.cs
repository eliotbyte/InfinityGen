using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class AreaDependency<TChunk> : IDependency2D where TChunk : IChunk2D
	{
		private readonly Rectangle _area;

		public AreaDependency(Rectangle area)
		{
			_area = area;
		}

		public bool IsLoaded(LayerRegistry<Vector2Int> layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(_area);
		}

		public void Load(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, _area);
		}

		public void Unload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, _area);
		}
	}
}
