namespace EliotByte.InfinityGen
{
	public class CircleDependency<TChunk> : IDependency where TChunk : IChunk
	{
		private readonly Circle _circle;

		public CircleDependency(LayerRegistry layerRegistry, Circle circle)
		{
			_circle = circle;
		}

		public bool IsLoaded(LayerRegistry layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(_circle);
		}

		public void Load(LayerRegistry layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, _circle);
		}

		public void Unload(LayerRegistry layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, _circle);
		}
	}
}
