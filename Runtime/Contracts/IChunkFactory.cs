using UnityEngine;

namespace EliotByte.InfinityGen
{
	public interface IChunkFactory<TChunk, TChunkPosition>
	{
		TChunk Create(TChunkPosition position, float chunkSize, LayerRegistry2D layerRegistry);
	}

	public interface IChunkFactory2D<TChunk> : IChunkFactory<TChunk, Vector2Int>
	{
	}
}
