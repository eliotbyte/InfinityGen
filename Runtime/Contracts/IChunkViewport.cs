using UnityEngine;

namespace EliotByte.InfinityGen
{
	public interface IChunkViewport
	{
		Vector2 Position { get; }
		float Radius { get; }
		bool IsActive { get; }
	}
}
