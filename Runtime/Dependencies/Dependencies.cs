using System.Collections.Generic;
using System.Linq;

namespace EliotByte.InfinityGen
{
	public class Dependencies<TDimension> : IDependency<TDimension>
	{
		private readonly List<IDependency<TDimension>> _dependencies;

		public Dependencies() : this(Enumerable.Empty<IDependency<TDimension>>())
		{
		}

		public Dependencies(IEnumerable<IDependency<TDimension>> dependencies)
		{
			_dependencies = new List<IDependency<TDimension>>(dependencies);
		}

		public void Add(IDependency<TDimension> dependency)
		{
			_dependencies.Add(dependency);
		}

		public bool IsLoaded(LayerRegistry<TDimension> layerRegistry)
		{
			foreach (var dependency in _dependencies)
			{
				if (!dependency.IsLoaded(layerRegistry))
				{
					return false;
				}
			}
			return true;
		}

		public void Load(LayerRegistry<TDimension> layerRegistry)
		{
			foreach (var dependency in _dependencies)
			{
				dependency.Load(layerRegistry);
			}
		}

		public void Unload(LayerRegistry<TDimension> layerRegistry)
		{
			foreach (var dependency in _dependencies)
			{
				dependency.Unload(layerRegistry);
			}
		}
	}
}
