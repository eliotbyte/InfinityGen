namespace EliotByte.InfinityGen
{
	public class RandomComparer<TDimension> : IPositionsComparer<TDimension>
	{
		public static RandomComparer<TDimension> Instance { get; } = new();

		public SortingOrder Order { get; set; }

		public int Compare(TDimension x, TDimension y)
		{
			return 0;
		}
	}
}
