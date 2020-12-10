using SearchEngine.Model.Interface;

namespace SearchEngine.Service.Interface
{
    public interface ISearchEvaluator
    {
        (int weight, int transitiveWeight) Evaluate(ISearchable searchableItem, string searchString);
    }
}