using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace EliotByte.InfinityGen
{
    public class Chunk<T> : IChunk where T : IChunkEntity
    {
        public ChunkPosition Position { get; }
        public bool Loaded { get; private set; }

        private readonly List<ChunkDependencies> _dependencies = new();
        private List<T> _entities = new();
        private readonly IChunkEntityGenerator<T> _generator;

        public Chunk(ChunkPosition position, IChunkEntityGenerator<T> generator)
        {
            Position = position;
            _generator = generator;
        }

        public Chunk(ChunkPosition position, IChunkEntityGenerator<T> generator, List<ChunkDependencies> dependencies) : this(position, generator)
        {
            _dependencies = dependencies;
        }

        public bool DependenciesLoaded()
        {
            foreach (ChunkDependencies dependency in _dependencies)
                foreach (IChunk chunk in dependency.Chunks)
                    if (!chunk.Loaded)
                        return false;

            return true;
        }

        public IList GetEntities()
        {
            return _entities;
        }

        public void Load(IRandomFactory random)
        {
            if (!DependenciesLoaded())
                throw new Exception("Cannot load chunks when dependent chunks are not yet loaded");

            if (_dependencies.Count != 0)
                InjectGenerator();

            _entities = _generator.Generate(random.WorldPointRandom(Position.Position), Position.Area);
            Loaded = true;
        }

        public void Unload()
        {
            _generator.Dispose(_entities);
            _entities.Clear();
            Loaded = false;
        }

        private void InjectGenerator()
        {
            IEnumerable<FieldInfo> fields = GetGeneratorFields();

            foreach (FieldInfo field in fields)
            {
                Type type = field.FieldType;
                ChunkDependencies dependencies = GetDependenciesByType(type);
                InjectGeneratorField(field, dependencies);
            }
        }

        private ChunkDependencies GetDependenciesByType(Type type)
        {
            if (!type.IsGenericType)
                throw new InvalidOperationException("The provided type is not a generic type.");

            Type targetType = type.GetGenericArguments()[0];

            foreach (ChunkDependencies dependency in _dependencies)
                if (dependency.EntityType == targetType)
                    return dependency;

            throw new Exception($"No dependencies found for the type {type.Name}");
        }

        private void InjectGeneratorField(FieldInfo field, ChunkDependencies dependencies)
        {
            if (dependencies.Chunks.Count == 0)
                return;

            // Add: limit entities by region

            Type elementType = field.FieldType.GetGenericArguments()[0];
            Type listType = typeof(List<>).MakeGenericType(elementType);
            IList combinedData = (IList)Activator.CreateInstance(listType);

            foreach (IChunk chunk in dependencies.Chunks)
            {
                foreach (object item in chunk.GetEntities())
                {
                    combinedData.Add(item);
                }
            }

            field.SetValue(_generator, combinedData);
        }

        private IEnumerable<FieldInfo> GetGeneratorFields()
        {
            Type type = _generator.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            List<FieldInfo> matchingFields = new();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsGenericType &&
                    field.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                    typeof(IChunkEntity).IsAssignableFrom(field.FieldType.GetGenericArguments()[0]))
                {
                    matchingFields.Add(field);
                }
            }

            return matchingFields;
        }
    }
}
