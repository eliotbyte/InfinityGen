namespace EliotByte.InfinityGen
{
	public interface IChunkFactory<TChunk, TDimension>
	{
		TChunk Create(TDimension position, float chunkSize, LayerRegistry<TDimension> layerRegistry);
	}
}
