using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public class LayerRegistry<TDimension>
	{
		private readonly Dictionary<Type, IChunkLayer<TDimension>> _layersByChunkType = new();

		public IEnumerable<IChunkLayer<TDimension>> AllLayers => _layersByChunkType.Values;

		public void Register<TChunk>(int chunkSize, IChunkFactory<TChunk, TDimension> factory) where TChunk : IChunk<TDimension>
		{
			_layersByChunkType.Add(typeof(TChunk), new ChunkLayer<TChunk, TDimension>(chunkSize, factory, this));
		}

		public IChunkLayer<TChunk, TDimension> Get<TChunk>() where TChunk : IChunk<TDimension>
		{
			return (IChunkLayer<TChunk, TDimension>)_layersByChunkType[typeof(TChunk)];
		}
	}
}
