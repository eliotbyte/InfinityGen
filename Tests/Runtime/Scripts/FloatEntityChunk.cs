using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen.Tests
{
	public class FloatEntityChunk : IChunk
	{
        private readonly Vector2Int _position;
        private readonly Rectangle _area;
        private readonly int _count;
        private readonly int _seed;

        public FloatEntityChunk(Vector2Int position, int size, int count, int seed)
		{
            _position = position;
            _area = new Rectangle(position.x * size, position.y * size, size);
            _count = count;
            _seed = (_position.x * 73856093) ^ (_position.y * 19349663) ^ seed;
        }

		public List<FloatEntity> Entities { get; } = new();

		public LoadStatus Status { get; private set; }

		public void Load()
        {
			Status = LoadStatus.Processing;

            Random random = new(_seed);

			for (int i = 0; i < _count; i++)
			{
				float x = (float)(_area.X + random.NextDouble() * _area.Width);
				float y = (float)(_area.Y + random.NextDouble() * _area.Height);
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
            private readonly int _seed;

            public Factory(int count, int seed)
            {
                _count = count;
                _seed = seed;
            }

			public FloatEntityChunk Create(Vector2Int position, int size, LayerRegistry layerRegistry)
			{
				return new FloatEntityChunk(position, size, _count, _seed);
			}
		}
	}
}
