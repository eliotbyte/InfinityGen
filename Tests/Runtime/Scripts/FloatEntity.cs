using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	public struct FloatEntity
	{
		public FloatEntity(Vector2 position, float floatValue)
		{
			Position = position;
			FloatValue = floatValue;
		}

		public Vector2 Position { get; }
		public float FloatValue { get; }
	}
}
