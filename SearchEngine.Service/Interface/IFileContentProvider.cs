using System.Threading.Tasks;

namespace SearchEngine.Service.Interface
{
    public interface IFileContentProvider
    {
        Task<string> GetFileContent();
    }
}