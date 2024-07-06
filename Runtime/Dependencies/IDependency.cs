namespace EliotByte.InfinityGen
{
	public interface IDependency<TDimension>
	{
		public static IDependency<TDimension> Empty { get; } = new EmptyDependency<TDimension>();

		bool IsLoaded(LayerRegistry<TDimension> layerRegistry);

		void RequestLoad(LayerRegistry<TDimension> layerRegistry);

		void RequestUnload(LayerRegistry<TDimension> layerRegistry);
	}
}
