using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	public class PointEntityChunk : IChunk
	{
		private readonly ChunkPosition _chunkPosition;
		private readonly LayerRegistry _layerRegistry;
		private readonly int _count;

		public PointEntityChunk(ChunkPosition chunkPosition, LayerRegistry layerRegistry, int count)
		{
			_chunkPosition = chunkPosition;
			_layerRegistry = layerRegistry;
			_count = count;
		}

		public LoadStatus Status { get; private set; }

		public List<PointEntity> Points { get; } = new List<PointEntity>();

		public bool IsDependenciesLoaded()
		{
			return _layerRegistry.Get<FloatEntityChunk>().IsLoaded(_chunkPosition.Area);
		}

		public void LoadDependencies()
		{
			_layerRegistry.Get<FloatEntityChunk>().RequestLoad(this, _chunkPosition.Area);
		}

		public void UnloadDependencies()
		{
			_layerRegistry.Get<FloatEntityChunk>().RequestUnload(this, _chunkPosition.Area);
		}

		public void Load(System.Random random)
		{
			Status = LoadStatus.Processing;
			
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

			public Factory(int count)
			{
				_count = count;
			}
			
			public PointEntityChunk Create(ChunkPosition position, LayerRegistry layerRegistry)
			{
				return new PointEntityChunk(position, layerRegistry, _count);
			}
		}
	}
}
