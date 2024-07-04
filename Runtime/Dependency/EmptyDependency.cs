namespace EliotByte.InfinityGen
{
	public class EmptyDependency : IDependency
	{
		public static IDependency Instance { get; } = new EmptyDependency();

		public bool IsLoaded() => true;

		public void Load() { }

		public void Unload() { }
	}
}
