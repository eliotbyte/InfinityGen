using System;

namespace EliotByte.InfinityGen
{
	public interface IChunk
	{
		LoadStatus Status { get; }

		IDependency Dependency => EmptyDependency.Instance;

		void Load(Random random);

		void Unload();
	}
}
