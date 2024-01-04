using System.Threading.Tasks;

namespace FMG.LocalStorage;

public interface IFileHandler
{
	string FileName { get; }

	string FilePath { get; }

	string Load();

	byte[] LoadBinary();

	Task<string> LoadAsync();

	Task<byte[]> LoadBinaryAsync();

	bool Save(string content);

	bool Save(byte[] data);

	Task<bool> SaveAsync(string content);

	Task<bool> SaveAsynce(byte[] data);
}
