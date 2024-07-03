namespace EliotByte.InfinityGen
{
    public interface IChunk
    {
        ChunkStatus Status { get; }

        void Load(IRandomFactory randomFactory);

        void Unload();
    }

    public enum ChunkStatus
    {
        Unloaded,
        Processing,
        Loaded
    }
    
    public enum Request
    {
        Unload,
        Load
    }
}
