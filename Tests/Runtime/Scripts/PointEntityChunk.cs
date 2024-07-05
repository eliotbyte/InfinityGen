using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen.Tests
{
	public class PointEntityChunk : IChunk
	{
		private readonly ChunkPosition _chunkPosition;
		private readonly LayerRegistry _layerRegistry;
		private readonly int _count;
        private readonly int _seed;

		public PointEntityChunk(ChunkPosition chunkPosition, LayerRegistry layerRegistry, int count, int seed)
		{
			_chunkPosition = chunkPosition;
			_layerRegistry = layerRegistry;
			_count = count;
            _seed = (chunkPosition.Position.x * 73856093) ^ (chunkPosition.Position.y * 19349663) ^ seed;

			Dependency = new AreaDependency<FloatEntityChunk>(_chunkPosition.Area);
		}

		public LoadStatus Status { get; private set; }

		public IDependency Dependency { get; }

		public List<PointEntity> Points { get; } = new List<PointEntity>();

		public void Load()
		{
			Status = LoadStatus.Processing;

            Random random = new(_seed);

			var floatsAround = _layerRegistry.Get<FloatEntityChunk>()
				.GetChunks(_chunkPosition.Area)
				.SelectMany(chunk => chunk.Entities).ToArray();

			var chunkArea = _chunkPosition.Area;
			for (int i = 0; i < _count; i++)
			{
				float x = (float)(random.NextDouble() * chunkArea.Width) + chunkArea.X;
				float y = (float)(random.NextDouble() * chunkArea.Height) + chunkArea.Y;
				Points.Add(new PointEntity(new Vector2(x, y)));
			}

			Status = LoadStatus.Loaded;
		}

		public void Unload()
		{
			Status = LoadStatus.Processing;

			Points.Clear();

			Status = LoadStatus.Loaded;
		}

		public class Factory : IChunkFactory<PointEntityChunk>
		{
			private readonly int _count;
            private readonly int _seed;

			public Factory(int count, int seed)
			{
				_count = count;
                _seed = seed;
			}

			public PointEntityChunk Create(ChunkPosition position, LayerRegistry layerRegistry)
			{
				return new PointEntityChunk(position, layerRegistry, _count, _seed);
			}
		}
	}
}
