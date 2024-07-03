namespace EliotByte.InfinityGen
{
	public interface IChunkLayer
	{
		void AddDependency(LayerDependency dependency);

		bool IsLoaded(Circle circle);
		bool IsLoaded(Rectangle area);

		void RequestLoad(object requestSource, Circle circle);
		void RequestLoad(object requestSource, Rectangle area);

		void RequestUnload(object requestSource, Circle circle);
		void RequestUnload(object requestSource, Rectangle area);

		void ProcessRequests();
	}
}
