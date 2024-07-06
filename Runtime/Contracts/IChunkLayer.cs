namespace EliotByte.InfinityGen
{
	public interface IChunkLayer<TDimension>
	{
		int ChunkSize { get; }

		bool IsLoaded(TDimension position);

		void RequestLoad(object requestSource, TDimension position);

		void RequestUnload(object requestSource, TDimension position);

		void ProcessRequests(IPositionsComparer<TDimension> comparer);
	}

	public interface IChunkLayer<TChunk, TDimension> : IChunkLayer<TDimension>
	{
		TChunk GetChunk(TDimension position);
	}
}
