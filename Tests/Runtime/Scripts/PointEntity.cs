using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	public struct PointEntity
	{
		public PointEntity(Vector2 position)
		{
			Position = position;
		}

		public Vector2 Position { get; }
	}
}