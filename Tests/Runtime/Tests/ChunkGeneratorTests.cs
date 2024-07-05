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
			int seed = 0;
			ChunkGenerator2D chunkGenerator2D = new();

			chunkGenerator2D.RegisterLayer(7, new PointEntityChunk.Factory(5, seed));
			chunkGenerator2D.RegisterLayer(10, new FloatEntityChunk.Factory(3, seed));

			Viewport viewport = new(Vector2.zero, 5f);

			chunkGenerator2D.RegisterViewport(viewport);

			chunkGenerator2D.Generate();
			chunkGenerator2D.Generate();

			Assert.IsTrue(chunkGenerator2D.LayerRegistry.Get<PointEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius)));
			Assert.IsTrue(chunkGenerator2D.LayerRegistry.Get<FloatEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius)));
			Assert.IsFalse(chunkGenerator2D.LayerRegistry.Get<FloatEntityChunk>().IsLoaded(new Circle(viewport.Position, viewport.Radius + 20)));
		}
	}
}
