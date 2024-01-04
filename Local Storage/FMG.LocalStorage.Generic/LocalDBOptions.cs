using System.Collections.Generic;

namespace FMG.LocalStorage.Generic;

public class LocalDBOptions<T>
{
	public IFileHandler<List<T>> FileHandler { get; set; }
}
