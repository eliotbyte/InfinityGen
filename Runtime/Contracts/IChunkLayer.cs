namespace EliotByte.InfinityGen
{
	public interface IChunkLayer<TDimension>
	{
		float ChunkSize { get; }

		bool IsLoaded(TDimension position);

		void RequestLoad(object requestSource, TDimension position);

		void RequestUnload(object requestSource, TDimension position);

		void ProcessRequests();
	}

	public interface IChunkLayer<TChunk, TDimension> : IChunkLayer<TDimension>
	{
		TChunk GetChunk(TDimension position);
	}
}
