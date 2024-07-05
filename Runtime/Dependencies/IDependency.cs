namespace EliotByte.InfinityGen
{
	public interface IDependency<TDimension>
	{
		bool IsLoaded(LayerRegistry<TDimension> layerRegistry);
		void Load(LayerRegistry<TDimension> layerRegistry);
		void Unload(LayerRegistry<TDimension> layerRegistry);
	}
}
