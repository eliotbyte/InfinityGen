using UnityEngine;

namespace EliotByte.InfinityGen
{
    public struct ChunkPosition
    {
        public Vector2Int Position { get; }
        public Vector2 GlobalPosition { get; }
        public Rectangle Area { get; }
        public int Size { get; }

        public ChunkPosition(Vector2Int position, int chunkSize)
        {
            Position = position;
            Size = chunkSize;
            Area = new Rectangle(Position.x * Size, Position.y * Size, Size);
            GlobalPosition = new Vector2(Area.Position.x + Size / 2f, Area.Position.y + Size / 2f);
        }
    }
}
