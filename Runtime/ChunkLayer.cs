using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EliotByte.InfinityGen
{
	public class ChunkLayer<TChunk, TDimension> : IChunkLayer<TChunk, TDimension> where TChunk : IChunk<TDimension>
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

		private readonly LayerRegistry<TDimension> _layerRegistry;
		private readonly int _processesLimit;
		private readonly float _loadCoefficient;
		private readonly IDistanceComparer<TDimension> _distanceComparer;
		private readonly IChunkFactory<TChunk, TDimension> _chunkFactory;
		private readonly Dictionary<TDimension, ChunkHandle> _chunkHandles = new();
		private readonly HashSet<TDimension> _positionsToProcess = new();
		private readonly Pool<HashSet<object>> _hashsetPool = new(() => new());

		public int ChunkSize { get; }

		public ChunkLayer(int chunkSize, IChunkFactory<TChunk, TDimension> chunkFactory, IDistanceComparer<TDimension> distanceComparer, LayerRegistry<TDimension> layerRegistry,
			int processesLimit = 12, float loadCoefficient = 3f)
		{
			_chunkFactory = chunkFactory;
			_layerRegistry = layerRegistry;
			_processesLimit = processesLimit;
			_loadCoefficient = loadCoefficient;
			_distanceComparer = distanceComparer;
			ChunkSize = chunkSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsLoaded(TDimension position)
		{
			return _chunkHandles.TryGetValue(position, out var handle) && handle.Chunk.Status == LoadStatus.Loaded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestLoad(object requestSource, TDimension position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				handle = new(_chunkFactory.Create(position, ChunkSize, _layerRegistry), _hashsetPool.Get());
				_chunkHandles.Add(position, handle);
			}

			handle.LoadRequests.Add(requestSource);

			ProcessIfNeeded(position, handle);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestUnload(object requestSource, TDimension position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				return;
			}

			handle.LoadRequests.Remove(requestSource);

			ProcessIfNeeded(position, handle);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TChunk GetChunk(TDimension position)
		{
			return _chunkHandles[position].Chunk;
		}

		private void ProcessIfNeeded(TDimension position, ChunkHandle handle)
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

				if (handle.Chunk.Status == LoadStatus.Unloaded)
				{
					handle.Chunk.Dependency.Unload(_layerRegistry);
					_hashsetPool.Return(handle.LoadRequests);
					_chunkFactory.Dispose(handle.Chunk);
					_chunkHandles.Remove(position);
				}
			}
		}

		private readonly List<TDimension> _processedPositions = new();
		private readonly List<TDimension> _positionToLoad = new();
		private readonly List<TDimension> _positionToUnload = new();

		public void ProcessRequests(TDimension processingCenter)
		{
			int currentlyProcessing = 0;

			foreach (var position in _positionsToProcess)
			{
				var handle = _chunkHandles[position];

				if (handle.Chunk.Status == LoadStatus.Loading || handle.Chunk.Status == LoadStatus.Unloading)
				{
					currentlyProcessing += 1;
					continue;
				}

				bool needLoad = handle.LoadRequests.Count > 0;
				bool needUnload = !needLoad;
				bool isLoaded = handle.Chunk.Status == LoadStatus.Loaded;
				bool isUnloaded = handle.Chunk.Status == LoadStatus.Unloaded;

				if (needLoad && isLoaded || needUnload && isUnloaded)
				{
					_processedPositions.Add(position);
					continue;
				}

				if (needLoad)
				{
					_positionToLoad.Add(position);
				}
				else
				{
					_positionToUnload.Add(position);
				}
			}

			_distanceComparer.Target = processingCenter;
			_distanceComparer.SortingSign = 1;
			_positionToLoad.Sort(_distanceComparer); // Load first by closeness
			_distanceComparer.SortingSign = -1;
			_positionToUnload.Sort(_distanceComparer); // Unload first by remoteness

			// Distribute available processes
			int availableProcesses = Math.Max(0, _processesLimit - currentlyProcessing);
			int processesToUnload = availableProcesses / 2;
			int processesToLoad = availableProcesses - processesToUnload;
			float loadBalance = _positionToLoad.Count * _loadCoefficient - _positionToUnload.Count;
			if (loadBalance < 0)
			{
				(processesToUnload, processesToLoad) = (processesToLoad, processesToUnload);
			}

			int unusedLoads = Math.Max(0, processesToLoad - _positionToLoad.Count);
			int unusedUnloads = Math.Max(0, processesToUnload - _positionToUnload.Count);
			processesToLoad += unusedUnloads;
			processesToUnload += unusedLoads;

			for (var i = 0; i < _positionToLoad.Count && i < processesToLoad; i++)
			{
				var handle = _chunkHandles[_positionToLoad[i]];

				if (!handle.Chunk.Dependency.IsLoaded(_layerRegistry))
				{
					handle.Chunk.Dependency.Load(_layerRegistry);
				}
				else
				{
					handle.Chunk.Load();
				}
			}

			for (var i = 0; i < _positionToUnload.Count && i < processesToUnload; i++)
			{
				var handle = _chunkHandles[_positionToUnload[i]];

				handle.Chunk.Unload();
				handle.Chunk.Dependency.Unload(_layerRegistry);
			}

			_positionToLoad.Clear();
			_positionToUnload.Clear();

			foreach (var position in _processedPositions)
			{
				_positionsToProcess.Remove(position);

				var handle = _chunkHandles[position];
				if (handle.Chunk.Status == LoadStatus.Unloaded)
				{
					handle.Chunk.Dependency.Unload(_layerRegistry);
					_hashsetPool.Return(handle.LoadRequests);
					_chunkFactory.Dispose(handle.Chunk);
					_chunkHandles.Remove(position);
				}
			}
			_processedPositions.Clear();
		}
	}
}
