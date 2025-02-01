using System.Globalization;
using RoofEstimation.Models.Material;
using RoofEstimation.Models.TearOff;

namespace RoofEstimation.Models.Estimation;

public class Estimation
{
    private static readonly CultureInfo UsCulture = new CultureInfo("en-US");

    public MaterialResult MaterialResult { get; set; }
    public TearOffResult TearOffResult { get; set; }
    //public ClientEntity Client { get; set; }
    
    public decimal GuttersTotal { get; set; }
    public decimal GuttersProfit { get; set; }
    public decimal LaborTotal { get; set; }
    public decimal PermitFees { get; set; }
    public decimal EstimatedProfit { get; set; }
    public decimal MainTotal { get; set; }
    public decimal Woodcrest { get; set; }
    public decimal Woodmoor { get; set; }

    public decimal Total => MaterialResult.TotalWithTax +
                            TearOffResult.Total +
                            GuttersTotal +
                            LaborTotal +
                            PermitFees;

    // Formatted Properties
    public string MaterialsTotal => FormatCurrency(MaterialResult.TotalWithTax);
    public string TearOffTotal => FormatCurrency(TearOffResult.Total);
    public string Gutters => FormatCurrency(GuttersTotal);
    public string InstallLaborTotalFormatted => FormatCurrency(LaborTotal);
    public string PermitFeesFormatted => FormatCurrency(PermitFees);
    public string ProfitTotal => FormatCurrency(EstimatedProfit);
    public string WoodcrestFormatted => FormatCurrency(Woodcrest);
    public string WoodmoorFormatted => FormatCurrency(Woodmoor);

    public string Duration => FormatCurrency(
        MaterialResult.TotalWithTax +
        PermitFees +
        LaborTotal +
        GuttersTotal +
        TearOffResult.Total +
        TearOffResult.TearOffWithPrices.Sum(x => x.MyProfit)
    );

    public string TearOffHaul => FormatCurrency(
        decimal.Round(TearOffResult.Total + TearOffResult.TearOffWithPrices.Sum(x => x.MyProfit), 2)
    );

    public string InstallRoofMaterials => FormatCurrency(LaborTotal);
    
    public string MaterialCost => FormatCurrency(decimal.Round(MaterialResult.TotalWithTax, 2));

    public string InstallGuttersAnd => FormatCurrency(
        decimal.Round(GuttersTotal + GuttersProfit, 2)
    );

    private static string FormatCurrency(decimal value) => value.ToString("C", UsCulture);
}