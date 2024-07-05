using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public class LayerRegistry<TDimension>
	{
		protected Dictionary<Type, IChunkLayer<TDimension>> LayersByChunkType { get; } = new();

		public IEnumerable<IChunkLayer<TDimension>> AllLayers => LayersByChunkType.Values;

		public IChunkLayer<TChunk, TDimension> Get<TChunk>() where TChunk : IChunk<TDimension>
		{
			return (IChunkLayer<TChunk, TDimension>)LayersByChunkType[typeof(TChunk)];
		}
	}
}
