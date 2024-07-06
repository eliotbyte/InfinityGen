using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen.Tests
{
	public class PointEntityChunk : IChunk2D
	{
		private readonly Vector2Int _position;
		private readonly Rectangle _area;
		private readonly LayerRegistry<Vector2Int> _layerRegistry;
		private readonly int _count;
		private readonly int _seed;

		public PointEntityChunk(Vector2Int position, int size, LayerRegistry<Vector2Int> layerRegistry, int count, int seed)
		{
			_position = position;
			_layerRegistry = layerRegistry;
			_count = count;
			_area = new Rectangle(position.x * size, position.y * size, size);
			_seed = (_position.x * 73856093) ^ (_position.y * 19349663) ^ seed;

			Dependency = new AreaDependency<FloatEntityChunk>(_area);
		}

		public LoadStatus Status { get; private set; }

		public IDependency<Vector2Int> Dependency { get; }

		public List<PointEntity> Points { get; } = new();

		public void Load()
		{
			Random random = new(_seed);
			Status = LoadStatus.Loading;

			var floatsAround = _layerRegistry.Get<FloatEntityChunk>()
				.GetChunks(_area)
				.SelectMany(chunk => chunk.Entities).ToArray();

			for (int i = 0; i < _count; i++)
			{
				float x = (float)(random.NextDouble() * _area.Width) + _area.MinX;
				float y = (float)(random.NextDouble() * _area.Height) + _area.MinY;
				Points.Add(new PointEntity(new Vector2(x, y)));
			}

			Status = LoadStatus.Loaded;
		}

		public void Unload()
		{
			Status = LoadStatus.Unloading;

			Points.Clear();

			Status = LoadStatus.Unloaded;
		}

		public class Factory : IChunkFactory2D<PointEntityChunk>
		{
			private readonly int _count;
			private readonly int _seed;

			public Factory(int count, int seed)
			{
				_count = count;
				_seed = seed;
			}

			public PointEntityChunk Create(Vector2Int position, int size, LayerRegistry<Vector2Int> layerRegistry)
			{
				return new PointEntityChunk(position, size, layerRegistry, _count, _seed);
			}

			public void Dispose(PointEntityChunk chunk)
			{
			}
		}
	}
}
