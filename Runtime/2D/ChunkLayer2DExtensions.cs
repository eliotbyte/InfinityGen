using System;
using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public static class ChunkLayer2DExtensions
	{
		public const float Padding = 0.001f;
		
		public static bool IsLoaded(this IChunkLayer<Vector2Int> chunkLayer, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(chunkLayer.ChunkSize, circle.Position, circle.Radius))
			{
				if (!chunkLayer.IsLoaded(position))
					return false;
			}

			return true;
		}

		public static bool IsLoaded(this IChunkLayer<Vector2Int> chunkLayer, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(chunkLayer.ChunkSize, area))
			{
				if (!chunkLayer.IsLoaded(position))
					return false;
			}

			return true;
		}

		public static void RequestLoad(this IChunkLayer<Vector2Int> chunkLayer, object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(chunkLayer.ChunkSize, circle.Position, circle.Radius))
			{
				chunkLayer.RequestLoad(requestSource, position);
			}
		}

		public static void RequestLoad(this IChunkLayer<Vector2Int> chunkLayer, object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(chunkLayer.ChunkSize, area))
			{
				chunkLayer.RequestLoad(requestSource, position);
			}
		}

		public static void RequestUnload(this IChunkLayer<Vector2Int> chunkLayer, object requestSource, Circle circle)
		{
			foreach (Vector2Int position in GetChunksInRadius(chunkLayer.ChunkSize, circle.Position, circle.Radius))
			{
				chunkLayer.RequestUnload(requestSource, position);
			}
		}

		public static void RequestUnload(this IChunkLayer<Vector2Int> chunkLayer, object requestSource, Rectangle area)
		{
			foreach (Vector2Int position in GetChunksInArea(chunkLayer.ChunkSize, area))
			{
				chunkLayer.RequestUnload(requestSource, position);
			}
		}

		public static IEnumerable<TChunk> GetChunks<TChunk>(this IChunkLayer<TChunk, Vector2Int> chunkLayer, Rectangle area) where TChunk : IChunk2D
		{
			var chunkPositions = GetChunksInArea(chunkLayer.ChunkSize, area);
			TChunk[] chunks = new TChunk[chunkPositions.Length];

			for (var i = 0; i < chunkPositions.Length; i++)
			{
				chunks[i] = chunkLayer.GetChunk(chunkPositions[i]);
			}

			return chunks;
		}

		private static Vector2Int[] _positionsBuffer = new Vector2Int[200 * 200];

		private static Span<Vector2Int> GetChunksInArea(int chunkSize, Rectangle area)
		{
			int minChunkX = Mathf.FloorToInt(area.MinX / chunkSize + Padding);
			int maxChunkX = Mathf.FloorToInt(area.MaxX / chunkSize - Padding);
			int minChunkY = Mathf.FloorToInt(area.MinY / chunkSize + Padding);
			int maxChunkY = Mathf.FloorToInt(area.MaxY / chunkSize - Padding);

			int amount = 0;
			for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
			for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
			{
				_positionsBuffer[amount++] = new(chunkX, chunkY);
			}

			return _positionsBuffer.AsSpan(0, amount);
		}

		private static Span<Vector2Int> GetChunksInRadius(float chunkSize, Vector2 userPosition, float radius)
		{
			float playerX = userPosition.x;
			float playerY = userPosition.y;

			int minChunkX = Mathf.FloorToInt((playerX - radius) / chunkSize + Padding);
			int maxChunkX = Mathf.FloorToInt((playerX + radius) / chunkSize - Padding);
			int minChunkY = Mathf.FloorToInt((playerY - radius) / chunkSize + Padding);
			int maxChunkY = Mathf.FloorToInt((playerY + radius) / chunkSize - Padding);

			int amount = 0;
			for (int x = minChunkX; x <= maxChunkX; x++)
			for (int y = minChunkY; y <= maxChunkY; y++)
				if (IsChunkInRadius(x, y, chunkSize, userPosition, radius))
				{
					_positionsBuffer[amount++] = new(x, y);
				}

			return _positionsBuffer.AsSpan(0, amount);
		}

		private static bool IsChunkInRadius(int chunkX, int chunkY, float chunkSize, Vector2 userPosition, float radius)
		{
			Span<Vector2> chunkCorners = stackalloc Vector2[]
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
