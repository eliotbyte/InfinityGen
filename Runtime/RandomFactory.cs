using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen
{
	public class RandomFactory : IRandomFactory
	{
		private readonly int _seed;

		public RandomFactory(int seed)
		{
			_seed = seed;
		}

		public Random WorldRandom() => new(_seed);

		public Random WorldPointRandom(Vector2Int point) => new(PointSeed(point, _seed));

		private static int PointSeed(Vector2Int point, int seed) => (point.x * 73856093) ^ (point.y * 19349663) ^ seed;
	}
}
