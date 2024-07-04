namespace EliotByte.InfinityGen
{
	public class AreaDependency<TChunk> : IDependency where TChunk : IChunk
	{
		private readonly Rectangle _area;

		public AreaDependency(Rectangle area)
		{
			_area = area;
		}

		public bool IsLoaded(LayerRegistry layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(_area);
		}

		public void Load(LayerRegistry layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, _area);
		}

		public void Unload(LayerRegistry layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, _area);
		}
	}
}