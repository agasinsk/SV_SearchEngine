using System.Threading.Tasks;

namespace SearchEngine.Service.Interface
{
    public interface IDataProvider<T>
    {
        Task<T> GetData();
    }
}