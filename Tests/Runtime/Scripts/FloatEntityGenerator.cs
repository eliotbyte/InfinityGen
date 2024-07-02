using System.Collections.Generic;
using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class FloatEntityGenerator : IChunkEntityGenerator<FloatEntity>
    {
        private readonly int _count;

        public FloatEntityGenerator(int count)
        {
            _count = count;
        }

        public List<FloatEntity> Generate(System.Random random, Rectangle area)
        {
            List<FloatEntity> entities = new();

            for (int i = 0; i < _count; i++)
            {
                float x = (float)(area.X + random.NextDouble() * area.Width);
                float y = (float)(area.Y + random.NextDouble() * area.Height);
                entities.Add(new FloatEntity(new Vector2(x, y), (float)random.NextDouble()));
            }

            return entities;
        }

        public void Dispose(List<FloatEntity> entities) => throw new System.NotImplementedException();
    }
}
