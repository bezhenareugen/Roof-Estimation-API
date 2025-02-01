using RoofEstimation.Models.Enums;

namespace RoofEstimation.Entities.Labor;

public class InstallLaborCostEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal LaborPrice { get; set; }
    public decimal Profit { get; set; }
    public decimal MyProfit { get; set; }
    public decimal NumberOfSquares { get; set; }
    public InstallContractorLabor LaborType { get; set; }
}