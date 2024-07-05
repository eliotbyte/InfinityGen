namespace EliotByte.InfinityGen
{
	public class CircleDependency<TChunk> : IDependency where TChunk : IChunk
	{
		private readonly Circle _circle;

		public CircleDependency(LayerRegistry2D layerRegistry, Circle circle)
		{
			_circle = circle;
		}

		public bool IsLoaded(LayerRegistry2D layerRegistry)
		{
			return layerRegistry.Get<TChunk>().IsLoaded(_circle);
		}

		public void Load(LayerRegistry2D layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestLoad(this, _circle);
		}

		public void Unload(LayerRegistry2D layerRegistry)
		{
			layerRegistry.Get<TChunk>().RequestUnload(this, _circle);
		}
	}
}
