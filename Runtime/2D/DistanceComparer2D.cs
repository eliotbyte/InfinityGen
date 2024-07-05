using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class DistanceComparer2D : IDistanceComparer<Vector2Int>
	{
		public Vector2Int Target { get; set; }

		public int SortingSign { get; set; }

		public int Compare(Vector2Int x, Vector2Int y)
		{
			int distanceA = (x - Target).sqrMagnitude;
			int distanceB = (y - Target).sqrMagnitude;

			return distanceA.CompareTo(distanceB) * SortingSign;
		}
	}
}
