using System.Threading.Tasks;

namespace Framework;

public interface IService
{
	Task Initialize();
}
public interface IService<T>
{
	Task Initialize(T tValue);
}
public interface IService<T, V>
{
	Task Initialize(T tValue, V vValue);
}
