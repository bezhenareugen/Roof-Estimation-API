using RoofEstimation.Entities.TearOff;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.Api.UnitTests;

public static class TearOffsRealData
{
    public static List<TearOffEntity> TearOffs { get; } =
    [
        new TearOffEntity
        {
            Id = 1,
            Name = "1- LAYER 4/12 or Torch On",
            Price = 50.00m,
            TearOffPrice = 150.00m,
            Profit = 30.00m,
            MyProfit = 90.00m,
            NumberOfSquares = 3,
            TearOffType = TearOff.Layer4by12orTouchOn // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 2,
            Name = "ADDITIONAL LAYER 4/12",
            Price = 19.00m,
            TearOffPrice = 57.00m,
            Profit = 6.00m,
            MyProfit = 18.00m,
            NumberOfSquares = 3,
            TearOffType = TearOff.AdditionalLayer4by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 3,
            Name = "ADDITIONAL LAYER 4/12",
            Price = 11.00m,
            TearOffPrice = 33.00m,
            Profit = 9.00m,
            MyProfit = 27.00m,
            NumberOfSquares = 3,
            TearOffType = TearOff.AdditionalLayer4by12Second // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 4,
            Name = "1- LAYER 6/12",
            Price = 48.00m,
            TearOffPrice = 0.00m,
            Profit = 32.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.Layer6by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 5,
            Name = "ADDITIONAL LAYER 6/12",
            Price = 22.00m,
            TearOffPrice = 0.00m,
            Profit = 8.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer6by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 6,
            Name = "ADDITIONAL LAYER 6/12",
            Price = 12.00m,
            TearOffPrice = 0.00m,
            Profit = 8.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer6by12Second // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 7,
            Name = "1- LAYER 7/12",
            Price = 66.00m,
            TearOffPrice = 0.00m,
            Profit = 19.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.Layer7by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 8,
            Name = "ADDITIONAL LAYER 7/12",
            Price = 12.00m,
            TearOffPrice = 0.00m,
            Profit = 18.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer7by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 9,
            Name = "ADDITIONAL LAYER 7/12",
            Price = 11.00m,
            TearOffPrice = 0.00m,
            Profit = 9.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer7by12Second // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 10,
            Name = "1- LAYER 8/12",
            Price = 66.00m,
            TearOffPrice = 0.00m,
            Profit = 24.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.Layer8by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 11,
            Name = "ADDITIONAL LAYER 8/12",
            Price = 12.00m,
            TearOffPrice = 0.00m,
            Profit = 18.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer8by12 // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 12,
            Name = "ADDITIONAL LAYER 8/12",
            Price = 11.00m,
            TearOffPrice = 0.00m,
            Profit = 9.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.AdditionalLayer8by12Second // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 13,
            Name = "ADD EXTRA $ Amount OTHER",
            Price = 0.00m,
            TearOffPrice = 0.00m,
            Profit = 0.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.ExtraAmount // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 14,
            Name = "LOW SLOPE",
            Price = 46.00m,
            TearOffPrice = 138.00m,
            Profit = 14.00m,
            MyProfit = 42.00m,
            NumberOfSquares = 3,
            TearOffType = TearOff.LowSlope // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 15,
            Name = "Ground Drop # of SQUARES",
            Price = 22.00m,
            TearOffPrice = 0.00m,
            Profit = 8.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.GroundDropOfSquares // Explicit enum value
        },

        new TearOffEntity
        {
            Id = 16,
            Name = "Ground Drop Addtnl Layer",
            Price = 17.00m,
            TearOffPrice = 0.00m,
            Profit = 13.00m,
            MyProfit = 0.00m,
            NumberOfSquares = 0,
            TearOffType = TearOff.GroundDropAdditionalLayer // Explicit enum value
        }
    ];
}