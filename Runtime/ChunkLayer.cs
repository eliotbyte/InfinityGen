using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public class ChunkLayer<TChunk> : IChunkLayer where TChunk : IChunk
	{
		public struct ChunkHandle
		{
			public TChunk Chunk;
			public HashSet<object> LoadRequests;

			public ChunkHandle(TChunk chunk, HashSet<object> loadRequests)
			{
				Chunk = chunk;
				LoadRequests = loadRequests;
			}
		}

		private readonly IChunkFactory<TChunk> _chunkFactory;
		private readonly IRandomFactory _randomFactory;
		private readonly LayerRegistry _layerRegistry;
		private readonly Dictionary<Vector2Int, ChunkHandle> _chunkHandles = new();
		private readonly HashSet<Vector2Int> _positionsToProcess = new();

		public int ChunkSize { get; }

		public ChunkLayer(int chunkSize, IChunkFactory<TChunk> chunkFactory, IRandomFactory randomFactory, LayerRegistry layerRegistry)
		{
			_chunkFactory = chunkFactory;
			_randomFactory = randomFactory;
			_layerRegistry = layerRegistry;
			ChunkSize = chunkSize;
		}

		public bool IsLoaded(Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				if (!IsLoaded(position))
					return false;
			}

			return true;
		}

		public bool IsLoaded(Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				if (!IsLoaded(position))
					return false;
			}

			return true;
		}

		public bool IsLoaded(Vector2Int position)
		{
			return _chunkHandles.TryGetValue(position, out var handle) && handle.Chunk.Status == LoadStatus.Loaded;
		}

		public void RequestLoad(object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				RequestLoad(requestSource, position);
			}
		}

		public void RequestLoad(object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				RequestLoad(requestSource, position);
			}
		}

		public void RequestUnload(object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				RequestUnload(requestSource, position);
			}
		}

		public void RequestUnload(object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				RequestUnload(requestSource, position);
			}
		}
		
		public void RequestLoad(object requestSource, Vector2Int position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				handle = new(_chunkFactory.Create(new ChunkPosition(position, ChunkSize), _layerRegistry), new HashSet<object>());
				_chunkHandles.Add(position, handle);
			}

			// TODO: Don't process chunk if it is loaded
			handle.LoadRequests.Add(requestSource);
			_positionsToProcess.Add(position);
		}

		public void RequestUnload(object requestSource, Vector2Int position)
		{
			if (!_chunkHandles.TryGetValue(position, out var handle))
			{
				return;
			}

			handle.LoadRequests.Remove(requestSource);
			_positionsToProcess.Add(position);
		}

		public IEnumerable<TChunk> GetChunks(Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				yield return _chunkHandles[position].Chunk;
			}
		}

		private readonly List<Vector2Int> _processedPositions = new();

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
						handle.Chunk.Load(_randomFactory.WorldPointRandom(position));
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

		private static Vector2Int[] GetChunksInArea(int chunkSize, Rectangle area)
		{
			// TODO: Add caching
			List<Vector2Int> chunkPositions = new();

			float xEnd = area.X + area.Width;
			float yEnd = area.Y + area.Height;

			int minChunkX = Mathf.FloorToInt(area.X / chunkSize);
			int maxChunkX = Mathf.FloorToInt((xEnd - 1) / chunkSize);
			int minChunkY = Mathf.FloorToInt(area.Y / chunkSize);
			int maxChunkY = Mathf.FloorToInt((yEnd - 1) / chunkSize);

			for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
			for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
				chunkPositions.Add(new Vector2Int(chunkX, chunkY));

			return chunkPositions.ToArray();
		}

		private static Vector2Int[] GetChunksInRadius(int chunkSize, Vector2 userPosition, float radius)
		{
			float playerX = userPosition.x;
			float playerY = userPosition.y;
			
			// TODO: Add caching
			List<Vector2Int> chunks = new();

			int minChunkX = Mathf.FloorToInt((playerX - radius) / chunkSize);
			int maxChunkX = Mathf.FloorToInt((playerX + radius) / chunkSize);
			int minChunkY = Mathf.FloorToInt((playerY - radius) / chunkSize);
			int maxChunkY = Mathf.FloorToInt((playerY + radius) / chunkSize);

			for (int x = minChunkX; x <= maxChunkX; x++)
			for (int y = minChunkY; y <= maxChunkY; y++)
				if (IsChunkInRadius(x, y, chunkSize, userPosition, radius))
					chunks.Add(new Vector2Int(x, y));

			return chunks.ToArray();
		}

		private static bool IsChunkInRadius(int chunkX, int chunkY, int chunkSize, Vector2 userPosition, float radius)
		{
			Vector2[] chunkCorners =
			{
				new(chunkX * chunkSize, chunkY * chunkSize),
				new((chunkX + 1) * chunkSize, chunkY * chunkSize),
				new(chunkX * chunkSize, (chunkY + 1) * chunkSize),
				new((chunkX + 1) * chunkSize, (chunkY + 1) * chunkSize)
			};

			foreach (Vector2 corner in chunkCorners)
				if (Vector2.Distance(corner, userPosition) <= radius)
					return true;

			float chunkXMin = chunkX * chunkSize;
			float chunkXMax = (chunkX + 1) * chunkSize;
			float chunkYMin = chunkY * chunkSize;
			float chunkYMax = (chunkY + 1) * chunkSize;

			return (chunkXMin <= userPosition.x && userPosition.x <= chunkXMax &&
			        chunkYMin <= userPosition.y && userPosition.y <= chunkYMax);
		}
	}
}
