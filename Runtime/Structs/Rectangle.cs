using UnityEngine;

namespace EliotByte.InfinityGen
{
    public struct Rectangle
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public float X => Position.x;
        public float Y => Position.y;
        public float Width => Size.x;
        public float Height => Size.y;

        public Vector2 Center
        {
            get
            {
                if (!_isCenterDirty)
                {
                    return _center;
                }

                _center = new Vector2(Position.x + Size.x / 2, Position.y + Size.y / 2);
                _isCenterDirty = false;
                return _center;
            }
        }

        private Vector2 _center;
        private bool _isCenterDirty;

        public Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            _center = default;
            _isCenterDirty = true;
        }

        public Rectangle(Vector2 position, float size) : this(position: position, size: new Vector2(size, size)){}

        public Rectangle(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height)) {}

        public Rectangle(float x, float y, float size) : this(new Vector2(x, y), new Vector2(size, size)) {}

        public bool Contains(Vector2 point)
        {
            return point.x >= Position.x && point.x < Position.x + Size.x &&
                   point.y >= Position.y && point.y < Position.y + Size.y;
        }
    }
}
