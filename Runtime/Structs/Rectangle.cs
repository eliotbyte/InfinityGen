using UnityEngine;

namespace EliotByte.InfinityGen
{
	public struct Rectangle
	{
        public Vector2 Min { get; }
        public Vector2 Max { get; }

        public float MinX => Min.x;
        public float MinY => Min.y;
        public float MaxX => Max.x;
        public float MaxY => Max.y;
        public float Width => Max.x - Min.x;
        public float Height => Max.y - Min.y;

        public Vector2 Center => new Vector2((Min.x + Max.x) / 2, (Min.y + Max.y) / 2);

        public Rectangle(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public Rectangle(Vector2 position, float width, float height)
            : this(position, new Vector2(position.x + width, position.y + height))
        {
        }

        public Rectangle(float minX, float minY, float maxX, float maxY)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY))
        {
        }

        public Rectangle(float x, float y, float size)
            : this(new Vector2(x, y), new Vector2(x + size, y + size))
        {
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= Min.x && point.x < Max.x &&
                   point.y >= Min.y && point.y < Max.y;
        }
	}
}
