using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMG.LocalStorage.Generic;

public class LocalDB<T> : ILocalDB<T>
{
	protected readonly List<T> _collection;

	private readonly LocalDBOptions<T> _options;

	private readonly object _lock;

	public string FilePath => _options.FileHandler.FilePath;

	public virtual IEnumerable<T> Collection
	{
		get
		{
			lock (_lock)
			{
				return _collection;
			}
		}
	}

	public LocalDB(Action<FileOptionsBuilder<T>> setupAction = null)
	{
		FileOptionsBuilder<T> optionsBuilder = new FileOptionsBuilder<T>();
		setupAction?.Invoke(optionsBuilder);
		_options = optionsBuilder.Options;
		if (_options.FileHandler == null)
		{
			optionsBuilder.UseDefaultFileHandler();
		}
		_collection = new List<T>();
		_lock = ((ICollection)_collection).SyncRoot;
	}

	public T LatestItem()
	{
		lock (_lock)
		{
			return _collection[^1];
		}
	}

	public virtual void Insert(T newItem)
	{
		lock (_lock)
		{
			_collection.Add(newItem);
		}
	}

	public virtual void InsertMany(IEnumerable<T> newItems)
	{
		lock (_lock)
		{
			_collection.AddRange(newItems);
		}
	}

	public virtual bool Delete(T item)
	{
		lock (_lock)
		{
			return _collection.Remove(item);
		}
	}

	public virtual int DeleteMany(IEnumerable<T> items)
	{
		lock (_lock)
		{
			int removedItemsCount = 0;
			foreach (T item in items)
			{
				if (Delete(item))
				{
					removedItemsCount++;
				}
			}
			return removedItemsCount;
		}
	}

	public virtual void DeleteAll()
	{
		lock (_lock)
		{
			_collection.Clear();
		}
	}

	public virtual bool Update(T item)
	{
		lock (_lock)
		{
			int itemHashCode = item.GetHashCode();
			for (int i = 0; i < _collection.Count; i++)
			{
				if (_collection[i].GetHashCode() == itemHashCode)
				{
					_collection[i] = item;
					return true;
				}
			}
			return false;
		}
	}

	public virtual int UpdateMany(IEnumerable<T> items)
	{
		lock (_lock)
		{
			int updatedItemsCount = 0;
			foreach (T item in items)
			{
				if (Update(item))
				{
					updatedItemsCount++;
				}
			}
			return updatedItemsCount;
		}
	}

	public virtual IEnumerable<T> Refresh()
	{
		lock (_lock)
		{
			List<T> collection = _options.FileHandler.Load();
			_collection.Clear();
			if (collection != null && collection.Count > 0)
			{
				_collection.AddRange(collection);
			}
			return _collection;
		}
	}

	public virtual bool Save()
	{
		lock (_lock)
		{
			return _options.FileHandler.Save(_collection);
		}
	}

	public virtual async Task<IEnumerable<T>> RefreshAsync()
	{
		List<T> collection = await _options.FileHandler.LoadAsync();
		lock (_lock)
		{
			_collection.Clear();
			if (collection != null && collection.Count > 0)
			{
				_collection.AddRange(collection);
			}
			return _collection;
		}
	}

	public virtual async Task<bool> SaveAsync()
	{
		lock (_lock)
		{
			_ = _collection;
		}
		return await _options.FileHandler.SaveAsync(_collection);
	}
}
