namespace EliotByte.InfinityGen
{
	public class EmptyDependency : IDependency
	{
		public static IDependency Instance { get; } = new EmptyDependency();

		public bool IsLoaded(LayerRegistry2D layerRegistry) => true;

		public void Load(LayerRegistry2D layerRegistry)
		{
		}

		public void Unload(LayerRegistry2D layerRegistry)
		{
		}
	}
}
