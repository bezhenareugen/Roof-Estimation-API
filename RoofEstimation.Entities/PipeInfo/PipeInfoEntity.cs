namespace RoofEstimation.Entities.PipeInfo;

public class PipeInfoEntity
{
    public int PipeInfoEntityId { get; set; }
    public int ShakeBaseAndCollars1 { get; set; }
    public int ShakeBaseAndCollars2 { get; set; }
    public int ShakeBaseAndCollars3 { get; set; }
    public int TTop4 { get; set; }
    public int HoodVent2Ppc4 { get; set; }
    public int HoodVent2Ppc6 { get; set; }
    public int TTop6 { get; set; }
    public int TTop7 { get; set; }
    public int HoodVent2Ppc8 { get; set; }
    public int OvalVertical { get; set; }
    public int OvalHorizontal { get; set; }
    public int PipesOther { get; set; }
    public int StepFlashing { get; set; }
    public int RoofToWalls { get; set; }
    public int PeelAndStickBase { get; set; }
    public int PeelAndStickCap { get; set; }
    public bool? Nails34 { get; set; } = false;
    public int Nails14 { get; set; }
    public int SiliconFlexiseal { get; set; }
    public int TackerStaples { get; set; }
    public int Paint { get; set; }
    public int PlyWood716OSB { get; set; }
    public int StaplesN19112 { get; set; }
}