using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EliotByte.InfinityGen
{
	public class MultiDimensionalChunkLayer<TChunk, TChunkPosition> : IChunkLayer<TChunkPosition> where TChunk : IChunk
	{
		private struct ChunkHandle
		{
			public TChunk Chunk;
			public HashSet<object> LoadRequests;

			public ChunkHandle(TChunk chunk, HashSet<object> loadRequests)
			{
				Chunk = chunk;
				LoadRequests = loadRequests;
			}
		}

		private readonly LayerRegistry2D _layerRegistry;
		private readonly IChunkFactory<TChunk, TChunkPosition> _chunkFactory;
		private readonly Dictionary<TChunkPosition, ChunkHandle> _chunkHandles = new();
		private readonly HashSet<TChunkPosition> _positionsToProcess = new();

		public float ChunkSize { get; }

		public MultiDimensionalChunkLayer(int chunkSize, IChunkFactory<TChunk, TChunkPosition> chunkFactory, LayerRegistry2D layerRegistry)
		{
			_chunkFactory = chunkFactory;
			_layerRegistry = layerRegistry;
			ChunkSize = chunkSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsLoaded(TChunkPosition position)
		{
			return _chunkHandles.TryGetValue(position, out var handle) && handle.Chunk.Status == LoadStatus.Loaded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestLoad(object requestSource, TChunkPosition position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				handle = new(_chunkFactory.Create(position, ChunkSize, _layerRegistry), new HashSet<object>());
				_chunkHandles.Add(position, handle);
			}

			handle.LoadRequests.Add(requestSource);

			ProcessIfNeeded(position, handle);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestUnload(object requestSource, TChunkPosition position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				return;
			}

			handle.LoadRequests.Remove(requestSource);

			ProcessIfNeeded(position, handle);
		}

		private void ProcessIfNeeded(TChunkPosition position, ChunkHandle handle)
		{
			bool needLoad = handle.LoadRequests.Count > 0;
			bool needUnload = !needLoad;
			bool isLoaded = handle.Chunk.Status == LoadStatus.Loaded;
			bool isUnloaded = handle.Chunk.Status == LoadStatus.Unloaded;

			if (needLoad && !isLoaded || needUnload && !isUnloaded)
			{
				_positionsToProcess.Add(position);
			}
			else
			{
				_positionsToProcess.Remove(position);

				// TODO: Add chunk pooling
				if (isUnloaded)
				{
					_chunkHandles.Remove(position);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TChunk GetChunk(TChunkPosition position)
		{
			return _chunkHandles[position].Chunk;
		}

		private readonly List<TChunkPosition> _processedPositions = new();

		public void ProcessRequests()
		{
			// TODO: Sort chunks by distance
			foreach (var position in _positionsToProcess)
			{
				var handle = _chunkHandles[position];

				if (handle.Chunk.Status == LoadStatus.Processing)
				{
					continue;
				}

				bool needLoad = handle.LoadRequests.Count > 0;
				bool isLoaded = handle.Chunk.Status == LoadStatus.Loaded;

				if (needLoad && isLoaded)
				{
					_processedPositions.Add(position);
					continue;
				}

				if (needLoad)
				{
					if (!handle.Chunk.Dependency.IsLoaded(_layerRegistry))
					{
						handle.Chunk.Dependency.Load(_layerRegistry);
					}
					else
					{
						handle.Chunk.Load();
					}
				}
				else
				{
					handle.Chunk.Unload();
					handle.Chunk.Dependency.Unload(_layerRegistry);
				}
			}

			foreach (var position in _processedPositions)
			{
				_positionsToProcess.Remove(position);

				// TODO: Add chunk pooling
				if (_chunkHandles[position].Chunk.Status == LoadStatus.Unloaded)
				{
					_chunkHandles.Remove(position);
				}
			}

			_processedPositions.Clear();
		}
	}
}
