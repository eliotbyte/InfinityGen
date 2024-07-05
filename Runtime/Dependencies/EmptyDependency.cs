namespace EliotByte.InfinityGen
{
	public class EmptyDependency<TDimension> : IDependency<TDimension>
	{
		public bool IsLoaded(LayerRegistry<TDimension> layerRegistry) => true;

		public void Load(LayerRegistry<TDimension> layerRegistry)
		{
		}

		public void Unload(LayerRegistry<TDimension> layerRegistry)
		{
		}
	}
}
