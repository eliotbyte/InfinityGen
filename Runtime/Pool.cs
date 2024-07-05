using System;
using System.Collections.Generic;

namespace EliotByte.InfinityGen
{
	public class Pool<TItem>
	{
		private readonly Stack<TItem> _availableItems = new Stack<TItem>();
		private readonly Func<TItem> _factory;

		public Pool(Func<TItem> factory)
		{
			_factory = factory;
		}

		public Pool(Func<TItem> factory, int prewarm) : this(factory)
		{
			for (int i = 0; i < prewarm; i++)
			{
				TItem item = _factory.Invoke();
				_availableItems.Push(item);
			}
		}

		public TItem Get()
		{
			if (_availableItems.Count > 0)
			{
				return _availableItems.Pop();
			}
			else
			{
				return _factory.Invoke();
			}
		}

		public void Return(TItem item)
		{
			_availableItems.Push(item);
		}
	}
}
