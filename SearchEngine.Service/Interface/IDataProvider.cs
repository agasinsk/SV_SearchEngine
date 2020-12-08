namespace SearchEngine.Service.Interface
{
    public interface IDataProvider<T>
    {
        T GetData();
    }
}