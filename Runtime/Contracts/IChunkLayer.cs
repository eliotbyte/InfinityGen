using UnityEngine;

namespace EliotByte.InfinityGen
{
	public interface IChunkLayer
	{
		bool IsLoaded(Circle circle);
		bool IsLoaded(Rectangle area);
		bool IsLoaded(Vector2Int position);

		void RequestLoad(object requestSource, Circle circle);
		void RequestLoad(object requestSource, Rectangle area);
		void RequestLoad(object requestSource, Vector2Int position);

		void RequestUnload(object requestSource, Circle circle);
		void RequestUnload(object requestSource, Rectangle area);
		void RequestUnload(object requestSource, Vector2Int position);

		void ProcessRequests();
	}
}
