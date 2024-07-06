using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class DirectionalComparer2D : IPositionsComparer<Vector2Int>
	{
		private int SortingSign { get; set; } = 1;

		public Vector2 Target { get; set; }

		public Vector2 Direction { get; set; }

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
			float factorX = Vector2.Dot(Direction, Target - (Vector2)x * ChunkSize);
			float factorY = Vector2.Dot(Direction, Target - (Vector2)y * ChunkSize);
			
			return factorX.CompareTo(factorY) * SortingSign;
		}
	}
}
