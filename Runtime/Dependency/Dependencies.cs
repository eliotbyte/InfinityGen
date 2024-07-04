using System.Collections.Generic;
using System.Linq;

namespace EliotByte.InfinityGen
{
	public class Dependencies : IDependency
	{
		private readonly List<IDependency> _dependencies;

		public Dependencies() : this(Enumerable.Empty<IDependency>())
		{
		}

		public Dependencies(IEnumerable<IDependency> dependencies)
		{
			_dependencies = new List<IDependency>(dependencies);
		}

		public void Add(IDependency dependency)
		{
			_dependencies.Add(dependency);
		}

		public bool IsLoaded()
		{
			foreach (var dependency in _dependencies)
			{
				if (!dependency.IsLoaded())
				{
					return false;
				}
			}
			return true;
		}

		public void Load()
		{
			foreach (var dependency in _dependencies)
			{
				dependency.Load();
			}
		}

		public void Unload()
		{
			foreach (var dependency in _dependencies)
			{
				dependency.Unload();
			}
		}
	}
}