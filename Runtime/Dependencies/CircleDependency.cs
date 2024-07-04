namespace EliotByte.InfinityGen
{
	public class CircleDependency<TChunk> : IDependency where TChunk : IChunk
	{
		private readonly IChunkLayer _layer;
		private readonly Circle _circle;

		public CircleDependency(LayerRegistry layerRegistry, Circle circle)
		{
			_layer = layerRegistry.Get<TChunk>();
			_circle = circle;
		}

		public bool IsLoaded()
		{
			return _layer.IsLoaded(_circle);
		}

		public void Load()
		{
			_layer.RequestLoad(this, _circle);
		}

		public void Unload()
		{
			_layer.RequestUnload(this, _circle);
		}
	}
}