namespace EliotByte.InfinityGen
{
	public interface IChunk<TDimension>
	{
		LoadStatus Status { get; }

		IDependency<TDimension> Dependency { get; }

		void Load();

		void Unload();
	}
}
