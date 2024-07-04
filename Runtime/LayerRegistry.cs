using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public class LayerRegistry
	{
		private readonly IRandomFactory _random;
		private readonly Dictionary<Type, IChunkLayer> _layersByChunkType = new();

		public IEnumerable<IChunkLayer> AllLayers => _layersByChunkType.Values;

		public LayerRegistry(IRandomFactory random)
		{
			_random = random;
		}

		public void Register<TChunk>(int chunkSize, IChunkFactory<TChunk> factory) where TChunk : IChunk
		{
			_layersByChunkType.Add(typeof(TChunk), new ChunkLayer<TChunk>(chunkSize, factory, _random, this));
		}

		public ChunkLayer<TChunk> Get<TChunk>() where TChunk : IChunk
		{
			return (ChunkLayer<TChunk>)_layersByChunkType[typeof(TChunk)];
		}
	}
}
