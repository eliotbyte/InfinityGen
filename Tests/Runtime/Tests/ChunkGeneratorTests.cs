using NUnit.Framework;
using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
	[TestFixture]
	public class ChunkGeneratorTests
	{
		[Test]
		public void TestChunkCreation()
		{
			ChunkGenerator chunkGenerator = new(new RandomFactory(0));
			
			chunkGenerator.RegisterLayer(7, new PointEntityChunk.Factory(5));
			chunkGenerator.RegisterLayer(10, new FloatEntityChunk.Factory(3));

			Viewport viewport = new(Vector2.zero, 5f);

			chunkGenerator.RegisterViewport(viewport);

			chunkGenerator.Generate();
			chunkGenerator.Generate();

			Assert.IsTrue(chunkGenerator.LayerRegistry.Get<PointEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius)));
			Assert.IsTrue(chunkGenerator.LayerRegistry.Get<FloatEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius)));
			Assert.IsFalse(chunkGenerator.LayerRegistry.Get<FloatEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius + 20)));
		}
	}
}
