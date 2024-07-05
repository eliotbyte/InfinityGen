using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class LayerRegistry2D
	{
		private readonly Dictionary<Type, IChunkLayer<Vector2Int>> _layersByChunkType = new();

		public IEnumerable<IChunkLayer<Vector2Int>> AllLayers => _layersByChunkType.Values;

		public void Register<TChunk>(int chunkSize, IChunkFactory2D<TChunk> factory) where TChunk : IChunk
		{
			_layersByChunkType.Add(typeof(TChunk), new ChunkLayer2D<TChunk>(chunkSize, factory, this));
		}

		public ChunkLayer2D<TChunk> Get<TChunk>() where TChunk : IChunk
		{
			return (ChunkLayer2D<TChunk>)_layersByChunkType[typeof(TChunk)];
		}
	}
}
