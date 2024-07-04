using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen.Tests
{
	public class FloatEntityChunk : IChunk
	{
		private readonly ChunkPosition _chunkPosition;
		private readonly int _count;

		public FloatEntityChunk(ChunkPosition chunkPosition, int count)
		{
			_chunkPosition = chunkPosition;
			_count = count;
		}

		public List<FloatEntity> Entities { get; } = new List<FloatEntity>();

		public LoadStatus Status { get; private set; }

		public void Load(Random random)
		{
			Status = LoadStatus.Processing;

			var chunkArea = _chunkPosition.Area;
			for (int i = 0; i < _count; i++)
			{
				float x = (float)(chunkArea.X + random.NextDouble() * chunkArea.Width);
				float y = (float)(chunkArea.Y + random.NextDouble() * chunkArea.Height);
				Entities.Add(new FloatEntity(new Vector2(x, y), (float)random.NextDouble()));
			}

			Status = LoadStatus.Loaded;
		}

		public void Unload()
		{
			Status = LoadStatus.Processing;

			Entities.Clear();

			Status = LoadStatus.Loaded;
		}

		public class Factory : IChunkFactory<FloatEntityChunk>
		{
			private readonly int _count;

			public Factory(int count)
			{
				_count = count;
			}

			public FloatEntityChunk Create(ChunkPosition position, LayerRegistry layerRegistry)
			{
				return new FloatEntityChunk(position, _count);
			}
		}
	}
}
