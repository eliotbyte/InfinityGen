namespace EliotByte.InfinityGen
{
	public interface IChunk<TDimension>
	{
		LoadStatus Status { get; }

		IDependency<TDimension> Dependency => EmptyDependency<TDimension>.Instance;

		void Load();

		void Unload();
	}
}
