namespace Aerozure.Extensions;

public static class LinqExtensions
{
    public static IEnumerable<T> SelectMaximumItems<T>(this IDictionary<string,T> source, int size,
        Func<T, int> rewardFunction, Func<T, int> costFunction, int maxCost)
    {
        var optimizer = new Optimizer();
        var items = optimizer.FindBest(
            source.Select(s => new OptimizableItem(s.Key, costFunction(s.Value), rewardFunction(s.Value))).ToList(), maxCost,
            size);
        return items.Select(item => source[item.Id]);
    }
}