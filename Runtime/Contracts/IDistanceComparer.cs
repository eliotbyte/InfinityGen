using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public interface IDistanceComparer<TDimension> : IComparer<TDimension>
	{
		public TDimension Target { get; set; }
	}
}
