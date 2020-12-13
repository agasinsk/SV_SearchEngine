namespace SearchEngine.Service.Interface
{
    public interface ISearchConfiguration
    {
        (int weight, int transitiveWeight) GetWeightsForProperty(string propertyName);
    }
}