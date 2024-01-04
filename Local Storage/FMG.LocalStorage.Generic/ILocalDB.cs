using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMG.LocalStorage.Generic;

public interface ILocalDB<T>
{
	string FilePath { get; }

	IEnumerable<T> Collection { get; }

	T LatestItem();

	void Insert(T newItem);

	void InsertMany(IEnumerable<T> newItems);

	bool Delete(T item);

	int DeleteMany(IEnumerable<T> items);

	void DeleteAll();

	bool Update(T item);

	int UpdateMany(IEnumerable<T> items);

	IEnumerable<T> Refresh();

	bool Save();

	Task<IEnumerable<T>> RefreshAsync();

	Task<bool> SaveAsync();
}
