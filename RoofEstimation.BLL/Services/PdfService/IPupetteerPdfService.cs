namespace RoofEstimation.BLL.Services.PdfService;

public interface IPupetteerPdfService
{
    Task<byte[]> GeneratePdfFromHtmlAsync(EstimationData data);
}