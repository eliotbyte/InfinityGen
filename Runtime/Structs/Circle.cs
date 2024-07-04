using UnityEngine;

namespace EliotByte.InfinityGen
{
	public readonly struct Circle
	{
		public Vector2 Position { get; }
		public float Radius { get; }

		public Circle(Vector2 position, float radius)
		{
			if (radius <= 0)
			{
				throw new System.ArgumentException("Radius must be greater than zero.");
			}

			Position = position;
			Radius = radius;
		}

		public bool Contains(Vector2 point)
		{
			float distanceSquared = (point - Position).sqrMagnitude;
			return distanceSquared < Radius * Radius;
		}
	}
}
