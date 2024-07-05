using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	public class Viewport : IChunkViewport
	{
		public Vector2 PreviousPosition { get; private set; }
		public Vector2 Position { get; private set; }
		public float Radius { get; }
		public bool IsActive { get; }

		public Viewport(Vector2 position, float radius, bool isActive = true)
		{
			PreviousPosition = Position = position;
			Radius = radius;
			IsActive = isActive;
		}

		public void UpdatePosition(Vector2 position)
		{
			PreviousPosition = Position;
			Position = position;
		}
	}
}
