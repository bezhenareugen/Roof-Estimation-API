using RoofEstimation.Entities.Material;

namespace RoofEstimation.Models.Material;

public record MaterialResult(List<MaterialEntity> Materials, decimal PreTotal, decimal Tax, decimal TotalWithTax);