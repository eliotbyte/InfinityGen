using UnityEngine;

namespace EliotByte.InfinityGen
{
	public interface IChunkFactory<TChunk>
	{
		TChunk Create(Vector2Int position, int size, LayerRegistry layerRegistry);
	}
}
