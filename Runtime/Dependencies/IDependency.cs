namespace EliotByte.InfinityGen
{
	public interface IDependency
	{
		bool IsLoaded(LayerRegistry2D layerRegistry);
		void Load(LayerRegistry2D layerRegistry);
		void Unload(LayerRegistry2D layerRegistry);
	}
}
