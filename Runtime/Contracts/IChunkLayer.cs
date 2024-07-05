using UnityEngine;

namespace EliotByte.InfinityGen
{
	public interface IChunkLayer<TChunkPosition>
	{
		float ChunkSize { get; }

		bool IsLoaded(TChunkPosition position);

		void RequestLoad(object requestSource, TChunkPosition position);

		void RequestUnload(object requestSource, TChunkPosition position);

		void ProcessRequests();
	}
}
