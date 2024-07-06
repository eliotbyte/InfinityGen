namespace EliotByte.InfinityGen
{
	public class EmptyDependency<TDimension> : IDependency<TDimension>
	{
		public bool IsLoaded(LayerRegistry<TDimension> layerRegistry) => true;

		public void RequestLoad(LayerRegistry<TDimension> layerRegistry)
		{
		}

		public void RequestUnload(LayerRegistry<TDimension> layerRegistry)
		{
		}
	}
}
