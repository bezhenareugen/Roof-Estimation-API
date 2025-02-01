namespace RoofEstimation.Entities.Gutters;

public class GuttersEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Units { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Units * Price;
    public decimal Profit { get; set; }
    public decimal MyProfit => Units * Profit;
}