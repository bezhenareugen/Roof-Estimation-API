using RoofEstimation.Models.Enums;

namespace RoofEstimation.Models.Prices;

public class PriceUpdateRequest
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public GuttersAndFeesType Type { get; set; }
}