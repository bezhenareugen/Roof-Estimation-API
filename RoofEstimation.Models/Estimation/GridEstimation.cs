using RoofEstimation.Entities.Enums;

namespace RoofEstimation.Models.Estimation;

public class GridEstimation
{
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public EstimationStatus EstimationStatus { get; set; }
}