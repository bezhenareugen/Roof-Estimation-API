namespace RoofEstimation.Models.Files;

public abstract class BaseFile
{
    public string FileName { get; set; }
    public string FileSize { get; set; }
    public string ContentType { get; set; }
    public DateTime? ModifiedOn { get; set; }
}