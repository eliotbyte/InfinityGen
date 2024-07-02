using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class PointEntityGenerator : IChunkEntityGenerator<PointEntity>
    {
        [ChunkInject] private readonly List<FloatEntity> _injectFloat;
        private readonly int _count;

        public PointEntityGenerator(int count)
        {
            _count = count;
        }

        public List<PointEntity> Generate(System.Random random, Rectangle area)
        {
            List<PointEntity> points = new();

            for (int i = 0; i < _count; i++)
            {
                float x = (float)(random.NextDouble() * area.Width) + area.X;
                float y = (float)(random.NextDouble() * area.Height) + area.Y;
                points.Add(new PointEntity(new Vector2(x, y)));
            }

            return points;
        }

        public void Dispose(List<PointEntity> entities) => throw new System.NotImplementedException();

        public bool HasInjectedData()
        {
            return _injectFloat != null;
        }
    }
}
