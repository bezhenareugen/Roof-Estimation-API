namespace RoofEstimation.Entities.TearOff;

public class TearOffEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal TearOffPrice { get; set; }
    public decimal Profit { get; set; }
    public decimal MyProfit { get; set; }
    public int NumberOfSquares { get; set; }
    public Models.Enums.TearOff TearOffType { get; set; }
}