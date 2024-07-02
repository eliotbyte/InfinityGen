using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class FloatEntity : IChunkEntity
    {
        public FloatEntity(Vector2 position, float floatValue)
        {
            Position = position;
            FloatValue = floatValue;
        }

        public Vector2 Position { get; }
        public float FloatValue { get; }
    }
}
