using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	public class Viewport : IChunkViewport
	{
		public Vector2 PreviousPosition { get; private set; }
		public Vector2 Position { get; private set; }
		public Vector2 Direction { get; private set; }
		public float Radius { get; }
		public bool IsActive { get; }

		public Viewport(Vector2 position, Vector2 direction, float radius, bool isActive = true)
		{
			PreviousPosition = Position = position;
			Direction = direction.normalized;
			Radius = radius;
			IsActive = isActive;
		}

		public void Update(Vector2 position, Vector2 direction)
		{
			PreviousPosition = Position;
			Position = position;

			Direction = direction.normalized;
		}
	}
}
