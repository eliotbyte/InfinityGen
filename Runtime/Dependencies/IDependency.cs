namespace EliotByte.InfinityGen
{
	public interface IDependency<TDimension>
	{
		public static IDependency<TDimension> Empty { get; } = new EmptyDependency<TDimension>();

		bool IsLoaded(LayerRegistry<TDimension> layerRegistry);

		void Load(LayerRegistry<TDimension> layerRegistry);

		void Unload(LayerRegistry<TDimension> layerRegistry);
	}
}
