using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class Viewport : IChunkViewport
    {
        public Vector2 Position { get; }
        public float Radius { get; }
        public bool IsActive { get; }

        public Viewport(Vector2 position, float radius, bool isActive = true)
        {
            Position = position;
            Radius = radius;
            IsActive = isActive;
        }
    }
}
