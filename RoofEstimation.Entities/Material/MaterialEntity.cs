using RoofEstimation.Entities.Enums;

namespace RoofEstimation.Entities.Material;

public class MaterialEntity
{
    public int Id { get; set; }
    public string MaterialName { get; set; }
    public decimal MaterialPrice { get; set; }
    public int MaterialUnits { get; set; }
    public decimal CalcPrice { get; set; }
    public MaterialsType MaterialType { get; set; }
}