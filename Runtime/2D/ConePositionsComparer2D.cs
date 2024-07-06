using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ConePositionsComparer2D : IPositionsComparer<Vector2Int>
	{
		private int SortingSign { get; set; } = 1;

		public Vector2 Target { get; set; }

		public int ChunkSize { get; set; }

		public SortingOrder Order
		{
			get => SortingSign == 1 ? SortingOrder.Ascending : SortingOrder.Descending;
			set
			{
				if (value == SortingOrder.Ascending)
				{
					SortingSign = 1;
				}
				else
				{
					SortingSign = -1;
				}
			}
		}

		public int Compare(Vector2Int x, Vector2Int y)
		{
			float distanceToX = ((Vector2)x * ChunkSize - Target).sqrMagnitude;
			float distanceToY = ((Vector2)y * ChunkSize - Target).sqrMagnitude;
			
			return distanceToX.CompareTo(distanceToY) * SortingSign;
		}
	}
}
