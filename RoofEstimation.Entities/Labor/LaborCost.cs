namespace RoofEstimation.Entities.Labor;

public record LaborCost(List<InstallLaborCostEntity> LaborCosts, decimal LaborTotal, decimal LaborProfit);