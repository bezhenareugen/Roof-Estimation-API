using System.Text;
using System.Text.Json;

namespace RoofEstimation.BLL.Services.PdfService;

public class PupetteerPdfService : IPupetteerPdfService
{
    private readonly HttpClient _httpClient;

    public PupetteerPdfService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<byte[]> GeneratePdfFromHtmlAsync(EstimationData data)
    {
        var htmlTemplate = await File.ReadAllTextAsync("../RoofEstimation.BLL/PdfTemplates/EstimationTemplate.html");

        // Escape the HTML content *before* creating the PdfRequest
        var escapedHtml = EscapeHtmlForJson(htmlTemplate);

        // Create request object for the external PDF service
        var pdfRequest = new PdfRequest { html = escapedHtml };

        // Serialize the request to JSON
        var json = JsonSerializer.Serialize(pdfRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send the request to the external service
        var response = await _httpClient.PostAsync("http://localhost:3897/api/pdf/generate", content); // Replace with your endpoint

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }
        else
        {
            // Handle errors appropriately (e.g., log, throw exception)
            throw new Exception($"Error generating PDF: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
    }

    private string ReplacePlaceholders(string html, EstimationData data)
    {
        // Implement your placeholder replacement logic here.
        // This is a basic example, you might want to use a templating engine for more complex scenarios.
        return html
            .Replace("Sun Run", data.PropertyAddress.Name)
            .Replace("99 Oak Ave", data.PropertyAddress.Street)
            // ... and so on for all your placeholders
            .Replace("$9,200.69", data.Total.ToString("C"));
    }

    // Helper method to escape HTML for JSON
    private string EscapeHtmlForJson(string html)
    {
        return html
            .Replace("\\", "\\\\")  // Escape backslashes
            .Replace("\"", "\\\"")  // Escape double quotes
            .Replace("\r", "")      // Remove carriage returns
            .Replace("\n", "\\n");  // Escape newlines
    }
}

// Example model (adjust to match your actual data structure)
public class EstimationData
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime ExpirationDate { get; set; }
    public PropertyAddress PropertyAddress { get; set; }
    public decimal TearOffCost { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal InstallCost { get; set; }
    public decimal GuttersCost { get; set; }
    public decimal PermitFees { get; set; }
    public decimal DurationTotal { get; set; }
    public decimal WoodcrestTotal { get; set; }
    public decimal WoodmoorTotal { get; set; }
    public decimal Total { get; set; }
}

public class PropertyAddress
{
    public string Name { get; set; }
    public string Street { get; set; }
    public string CityStateZip { get; set; }
}

// Request model for the external PDF service
public class PdfRequest
{
    public string html { get; set; }
}