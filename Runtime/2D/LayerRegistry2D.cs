using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class LayerRegistry2D : LayerRegistry<Vector2Int>
	{
		public void Register<TChunk>(int chunkSize, IChunkFactory2D<TChunk> factory) where TChunk : IChunk2D
		{
			LayersByChunkType.Add(typeof(TChunk), new ChunkLayer2D<TChunk>(chunkSize, factory, this));
		}
	}
}
