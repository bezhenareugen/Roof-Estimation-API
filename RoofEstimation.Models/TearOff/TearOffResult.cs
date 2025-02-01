using RoofEstimation.Entities.TearOff;

namespace RoofEstimation.Models.TearOff;

public record TearOffResult(List<TearOffEntity> TearOffWithPrices, decimal Total);