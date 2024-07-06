using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public interface IPositionsComparer<TDimension> : IComparer<TDimension>
	{
		public SortingOrder Order { get; set; }
	}
}
