namespace EliotByte.InfinityGen
{
	public interface IPositionCost<TDimension>
	{
		int Evaluate(TDimension position);
	}
}
