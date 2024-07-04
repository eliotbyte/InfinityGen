namespace EliotByte.InfinityGen
{
	public interface IDependency
	{
		bool IsLoaded(LayerRegistry layerRegistry);
		void Load(LayerRegistry layerRegistry);
		void Unload(LayerRegistry layerRegistry);
	}
}
