using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen
{
	public static class ChunkLayer2DExtensions
	{
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
			foreach (Vector2Int position in GetChunksInArea(chunkLayer.ChunkSize, area))
			{
				yield return chunkLayer.GetChunk(position);
			}
		}

		private static Vector2Int[] GetChunksInArea(float chunkSize, Rectangle area)
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

		private static Vector2Int[] GetChunksInRadius(float chunkSize, Vector2 userPosition, float radius)
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

		private static bool IsChunkInRadius(int chunkX, int chunkY, float chunkSize, Vector2 userPosition, float radius)
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
