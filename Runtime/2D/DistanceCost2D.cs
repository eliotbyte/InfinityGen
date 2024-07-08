using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class DistanceCost2D : IPositionCost<Vector2Int>
	{
		public Vector2Int Target { get; set; }

		public int ChunkSize { get; set; }

		public int Evaluate(Vector2Int position)
		{
			return (position * ChunkSize - Target).sqrMagnitude;
		}
	}
}
