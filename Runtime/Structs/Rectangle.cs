using Unity.Mathematics;

namespace EliotByte.InfinityGen
{
    public struct Rectangle
    {
        public float2 Min { get; }
        public float2 Max { get; }

        public float MinX => Min.x;
        public float MinY => Min.y;
        public float MaxX => Max.x;
        public float MaxY => Max.y;
        public float Width => Max.x - Min.x;
        public float Height => Max.y - Min.y;

        public float2 Center => (Min + Max) / 2;

        public Rectangle(float2 min, float2 max)
        {
            Min = min;
            Max = max;
        }

        public Rectangle(float2 position, float width, float height)
            : this(position, position + new float2(width, height))
        {
        }

        public Rectangle(float minX, float minY, float maxX, float maxY)
            : this(new float2(minX, minY), new float2(maxX, maxY))
        {
        }

        public Rectangle(float x, float y, float size)
            : this(new float2(x, y), new float2(x + size, y + size))
        {
        }

        public bool Contains(float2 point)
        {
            return point.x >= Min.x && point.x < Max.x &&
                   point.y >= Min.y && point.y < Max.y;
        }
    }
}