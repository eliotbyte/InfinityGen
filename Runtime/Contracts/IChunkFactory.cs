namespace EliotByte.InfinityGen
{
	public interface IChunkFactory<TChunk>
	{
		TChunk Create(ChunkPosition position, LayerRegistry layerRegistry);
	}
}
