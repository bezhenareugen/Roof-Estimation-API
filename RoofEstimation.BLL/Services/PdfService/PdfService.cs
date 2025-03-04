using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace RoofEstimation.BLL.Services.PdfService;

public class PdfService : IPdfService
{
       public byte[] GeneratePdfAsync()
       {
              QuestPDF.Settings.License = LicenseType.Community; 
              Document.Create(container =>
              {
                     container.Page(page =>
                     {
                            page.Size(PageSizes.A4);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(20));

                            page.Header()
                                   .Height(200)
                                   .Background("#111928")
                                   .PaddingHorizontal(2) // Ensuring space on both sides
                                   .Row(row =>
                                   {
                                          row.RelativeItem() // Left side (LIC Number)
                                                 .AlignLeft()
                                                 .Text("LIC #1007021")
                                                 .FontColor(Colors.White)
                                                 .FontSize(16)
                                                 .SemiBold();

                                          row.RelativeItem() // Right side (Company Details)
                                                 .AlignRight()
                                                 .Column(column =>
                                                 {
                                                        column.Item().Text("White River Roofing, Inc").FontSize(18).FontColor(Colors.White).SemiBold();
                                                        column.Item().Text("1342 Ascote Ave").FontColor(Colors.White);
                                                        column.Item().Text("Sacramento, CA 95673").FontColor(Colors.White);
                                                        column.Item().Text("(916) 813-ROOF (7663)").FontColor(Colors.White);
                                                        column.Item().Text("info@whiteriverroofing.com").FontColor(Colors.White);
                                                        column.Item().Text("www.whiteriverroofing.com").FontColor(Colors.White);
                                                 });
                                   });

                            page.Footer()
                                   .AlignCenter()
                                   .Text(x =>
                                   {
                                          x.Span("Page ");
                                          x.CurrentPageNumber();
                                          x.Span(" of ");
                                          x.TotalPages();
                                   });
                     });
              }).ShowInCompanion();

              return Array.Empty<byte>();
       }
}