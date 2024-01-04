using System.Threading.Tasks;

namespace FMG.LocalStorage.Generic;

public interface IFileHandler<T>
{
	string FileName { get; }

	string FilePath { get; }

	FileOptions Options { get; }

	T Load();

	Task<T> LoadAsync();

	bool Save(T content);

	Task<bool> SaveAsync(T content);
}
