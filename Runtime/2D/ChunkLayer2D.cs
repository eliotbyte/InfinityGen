using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkLayer2D<TChunk> : ChunkLayer<TChunk, Vector2Int> where TChunk : IChunk2D
	{
		public ChunkLayer2D(int chunkSize, IChunkFactory2D<TChunk> chunkFactory, LayerRegistry2D layerRegistry) : base(chunkSize, chunkFactory, layerRegistry)
		{
		}
	}
}
