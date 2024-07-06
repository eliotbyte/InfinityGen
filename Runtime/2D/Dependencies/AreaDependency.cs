using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class AreaDependency<TChunk> : IDependency2D where TChunk : IChunk2D
	{
		public AreaDependency(Rectangle area = default)
		{
			Area = area;
		}

		public Rectangle Area { get; set; }

		public bool IsLoaded(LayerRegistry<Vector2Int> layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(Area);
		}

		public void Load(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, Area);
		}

		public void Unload(LayerRegistry<Vector2Int> layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, Area);
		}
	}
}
