namespace Aerozure.Extensions;

internal class Optimizer
{
    private int bestScore = 0;
    private List<OptimizableItem> bestSolution = new();

    public List<OptimizableItem> FindBest(List<OptimizableItem> items, int maxBudget, int itemCount)
    {
        // var sorted = items.OrderByDescending(i => (double)i.Reward / i.Cost).ToList();
        var sorted = items.OrderByDescending(i => (double)i.Reward).ToList();
        Search(sorted, 0, new List<OptimizableItem>(), 0, 0, maxBudget, itemCount);
        return bestSolution;
    }

    private void Search(List<OptimizableItem> items, int index, List<OptimizableItem> current, int starsSum, int pointsSum, int maxBudget, int itemCount)
    {
        if (current.Count == itemCount)
        {
            if (starsSum <= maxBudget && pointsSum > bestScore)
            {
                bestScore = pointsSum;
                bestSolution = new List<OptimizableItem>(current);
            }
            return;
        }

        if (index >= items.Count || current.Count > 15 || starsSum > 40)
            return;

        var remaining = 15 - current.Count;
        var maxPotential = pointsSum + items.Skip(index).Take(remaining).Sum(i => i.Reward);
        if (maxPotential <= bestScore) return; // prune if we can't beat best score

        // Option 1: include current item
        var item = items[index];
        if (starsSum + item.Cost <= 40)
        {
            current.Add(item);
            Search(items, index + 1, current, starsSum + item.Cost, pointsSum + item.Reward, maxBudget, itemCount);
            current.RemoveAt(current.Count - 1);
        }

        // Option 2: skip current item
        Search(items, index + 1, current, starsSum, pointsSum, maxBudget, itemCount );
    }
}