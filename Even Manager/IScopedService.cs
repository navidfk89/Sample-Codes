using System.Threading.Tasks;

namespace Framework;

public interface IScopedService : IService
{
	Task Dispose();
}
public interface IScopedService<T> : IService<T>
{
	Task Dispose();
}
public interface IScopedService<T, V> : IService<T, V>
{
	Task Dispose();
}
