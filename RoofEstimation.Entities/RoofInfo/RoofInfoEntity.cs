using RoofEstimation.Models.Enums;

namespace RoofEstimation.Entities.RoofInfo;

public class RoofInfoEntity
{
    public int RoofInfoEntityId { get; set; }
    public int Squares { get; set; }
    public int LowSlope { get; set; }
    public int ValleysLF { get; set; }
    public int RidgeLF { get; set; }
    public int GaldesLF { get; set; }
    public int GutterLF { get; set; }
    public int GutterSize { get; set; }
    public int DownSpots { get; set; }
    public RoofPitchTypes RoofPitchType { get; set; }
    public int Vents { get; set; }
    public int VentsLF { get; set; }
    public bool VentsCutIn { get; set; }
    public bool VentsRidge { get; set; }
    public int StepFlashing { get; set; }
    public bool PlyWood { get; set; }
    public int RoofSlope { get; set; }
    public int Stories { get; set; }
    public int Layers { get; set; }
    public int UnderShots { get; set; }
    public int Chimneys { get; set; }
    public int ACUnits { get; set; }
    public int SkyLights { get; set; }
    public int StepFlashingLF { get; set; }
    public int Dishes { get; set; }
    public int ACCurb { get; set; }
    public int ACFrame { get; set; }
}