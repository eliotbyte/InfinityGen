namespace EliotByte.InfinityGen
{
	public class AreaDependency<TChunk> : IDependency where TChunk : IChunk
	{
		private readonly IChunkLayer _layer;
		private readonly Rectangle _area;

		public AreaDependency(LayerRegistry layerRegistry, Rectangle area)
		{
			_layer = layerRegistry.Get<TChunk>();
			_area = area;
		}

		public bool IsLoaded()
		{
			return _layer.IsLoaded(_area);
		}

		public void Load()
		{
			_layer.RequestLoad(this, _area);
		}

		public void Unload()
		{
			_layer.RequestUnload(this, _area);
		}
	}
}