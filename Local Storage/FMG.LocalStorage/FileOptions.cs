using FMG.Security.Cryptography;

namespace FMG.LocalStorage;

public class FileOptions
{
	public string Path { get; set; } = null;


	public ICryptographer Cryptographer { get; set; }

	public bool HashFileName { get; set; }
}
