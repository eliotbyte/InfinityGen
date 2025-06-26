using Unity.Mathematics;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public readonly struct Circle
	{
		public float2 Position { get; }
		public float Radius { get; }

		public Circle(float2 position, float radius)
		{
			if (radius <= 0)
			{
				throw new System.ArgumentException("Radius must be greater than zero.");
			}

			Position = position;
			Radius = radius;
		}

		public bool Contains(float2 point)
		{
			float distanceSquared = math.distancesq(point, Position);
			return distanceSquared < Radius * Radius;
		}
	}
}
