using System;

namespace AirGlobeEcoKernel
{
    class Program
    {
        static void Main()
        {
            var unit = new AirGlobeCapture
            {
                FlowRateM3PerHour = 100000,  // example realistic flow for building-scale
                CO2InPpm = 420,
                CO2OutPpm = 100
            };

            Console.WriteLine($"Annual capture: {unit.CapturedTonsPerYear():F1} metric tons CO₂");
            double score = EcoImpactCalculator.NetEcoImpactScore(unit.CapturedTonsPerYear(), 50, 50, 800000);
            Console.WriteLine($"Net Eco-Impact Score: {score:F2}");
        }
    }
}
