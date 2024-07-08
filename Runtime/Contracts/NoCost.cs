namespace EliotByte.InfinityGen
{
	public class NoCost<TDimension> : IPositionCost<TDimension>
	{
		public static NoCost<TDimension> Instance { get; } = new();

		public int Evaluate(TDimension position)
		{
			return 0;
		}
	}
}
