using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace EliotByte.InfinityGen.Tests
{
    public class WorldLayersContainerTests
    {
        [Test]
        public void TestInjectionBetweenLayers()
        {
            PointEntityGenerator pointEntityGenerator = new PointEntityGenerator(5);
            ChunkLayer<PointEntity> pointLayer = new(7, pointEntityGenerator);
            ChunkLayer<FloatEntity> floatLayer = new ChunkLayer<FloatEntity>(10, new FloatEntityGenerator(3));

            Viewport viewport = new(Vector2.zero, 5f);

            ChunkGenerator layersContainer = new(new RandomFactory(0));

            layersContainer.RegisterLayer(pointLayer);
            layersContainer.RegisterLayer(floatLayer);

            layersContainer.RegisterViewport(viewport);

            layersContainer.RegisterDependency(pointLayer, floatLayer, 3);

            layersContainer.Generate();
            layersContainer.Generate();

            Assert.IsTrue(pointEntityGenerator.HasInjectedData(), "The points layer did not receive injected data from the float layer.");
        }
    }
}
