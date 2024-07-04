using UnityEngine;
using Random = System.Random;

namespace EliotByte.InfinityGen
{
	public interface IRandomFactory
	{
		Random WorldRandom();
		Random WorldPointRandom(Vector2Int point);
	}
}
