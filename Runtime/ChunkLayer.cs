using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public struct LayerDependency
	{
		public IChunkLayer Layer;
		public Vector2 Padding;
	}

	public class ChunkLayer<T> : IChunkLayer
	{
		private readonly IRandomFactory _randomFactory;
		private readonly HashSet<LayerDependency> _dependencies = new();
		private Dictionary<Vector2Int, Chunk<T>> _chunks = new();
		private HashSet<Chunk<T>> _chunksToProcess = new();

		public int ChunkSize { get; }

		public ChunkLayer(int chunkSize, IRandomFactory randomFactory)
		{
			_randomFactory = randomFactory;
			ChunkSize = chunkSize;
		}

		public void AddDependency(LayerDependency dependency)
		{
			_dependencies.Add(dependency);
		}

		public bool IsLoaded(Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				if (!_chunks.TryGetValue(position, out var chunk) || chunk.Status != ChunkStatus.Loaded)
					return false;
			}

			return true;
		}

		public bool IsLoaded(Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				if (!_chunks.TryGetValue(position, out var chunk) || chunk.Status != ChunkStatus.Loaded)
					return false;
			}

			return true;
		}

		public void RequestLoad(object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				if (!_chunks.TryGetValue(position, out var chunk))
				{
					chunk = new Chunk<T>(new ChunkPosition(position, ChunkSize));
					_chunks.Add(position, chunk);
				}

				// TODO: Don't process chunk if it is loaded
				chunk.AddLoadRequest(requestSource);
				_chunksToProcess.Add(chunk);
			}
		}

		public void RequestLoad(object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				if (!_chunks.TryGetValue(position, out var chunk))
				{
					chunk = new Chunk<T>(new ChunkPosition(position, ChunkSize));
					_chunks.Add(position, chunk);
				}

				// TODO: Don't process chunk if it is loaded
				chunk.AddLoadRequest(requestSource);
				_chunksToProcess.Add(chunk);
			}
		}

		public void RequestUnload(object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(ChunkSize, circle.Position, circle.Radius))
			{
				if (!_chunks.TryGetValue(position, out var chunk))
				{
					continue;
				}

				chunk.RemoveLoadRequest(requestSource);
				_chunksToProcess.Add(chunk);
			}
		}

		public void RequestUnload(object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(ChunkSize, area))
			{
				if (!_chunks.TryGetValue(position, out var chunk))
				{
					continue;
				}

				chunk.RemoveLoadRequest(requestSource);
				_chunksToProcess.Add(chunk);
			}
		}

		private readonly List<Chunk<T>> _processedChunksBuffer = new();

		public void ProcessRequests()
		{
			// TODO: Sort chunks by distance
			foreach (var chunk in _chunksToProcess)
			{
				if (chunk.Status == ChunkStatus.Processing)
				{
					continue;
				}

				bool needLoad = chunk.LoadRequests > 0;
				bool isLoaded = chunk.Status == ChunkStatus.Loaded;

				if (needLoad && isLoaded)
				{
					_processedChunksBuffer.Add(chunk);
					continue;
				}

				if (needLoad)
				{
					bool hasUnloadedDependencies = false;
					foreach (var dependency in _dependencies)
					{
						// TODO: Add padding
						var loadArea = chunk.Position.Area;
						if (!dependency.Layer.IsLoaded(loadArea))
						{
							hasUnloadedDependencies = true;
							dependency.Layer.RequestLoad(chunk, loadArea);
						}
					}

					if (!hasUnloadedDependencies)
					{
						chunk.Load(_randomFactory);
					}
				}
				else
				{
					chunk.Unload();

					foreach (var dependency in _dependencies)
					{
						// TODO: Add padding
						var loadArea = chunk.Position.Area;
						dependency.Layer.RequestUnload(chunk, loadArea);
					}
				}
			}

			foreach (var chunk in _processedChunksBuffer)
			{
				_chunksToProcess.Remove(chunk);
				// TODO: Add chunk pooling
				if (chunk.Status == ChunkStatus.Unloaded)
				{
					_chunks.Remove(chunk.Position.Position);
				}
			}
			_processedChunksBuffer.Clear();
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
