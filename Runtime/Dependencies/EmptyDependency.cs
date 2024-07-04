namespace EliotByte.InfinityGen
{
	public class EmptyDependency : IDependency
	{
		public static IDependency Instance { get; } = new EmptyDependency();

		public bool IsLoaded(LayerRegistry layerRegistry) => true;

		public void Load(LayerRegistry layerRegistry)
		{
		}

		public void Unload(LayerRegistry layerRegistry)
		{
		}
	}
}
