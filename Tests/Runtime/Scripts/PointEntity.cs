using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class PointEntity : IChunkEntity
    {
        public PointEntity(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; }
    }
}
