namespace EliotByte.InfinityGen
{
	public interface IDependency
	{
		bool IsLoaded();
		void Load();
		void Unload();
	}
}
