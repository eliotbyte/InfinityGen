using System;

namespace EliotByte.InfinityGen
{
	public interface IChunk
	{
		LoadStatus Status { get; }

		bool IsDependenciesLoaded() => true;

		void LoadDependencies() { }

		void UnloadDependencies() { }

		void Load(Random random);

		void Unload();
	}
}
