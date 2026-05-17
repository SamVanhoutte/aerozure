namespace Aerozure.Extensions;

internal class OptimizableItem(string id, int cost, int reward)
{
    public string Id { get; set; } = id;
    public int Cost { get; set; } = cost;
    public int Reward { get; set; } = reward;
}