namespace EliotByte.InfinityGen
{
	public interface IChunkFactory<TChunk, TDimension>
	{
		TChunk Create(TDimension position, int chunkSize, LayerRegistry<TDimension> layerRegistry);
	}
}
