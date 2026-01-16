using System;

namespace AirGlobeEcoKernel
{
    public static class EcoImpactCalculator
    {
        public static double NetEcoImpactScore(double capturedTonsPerYear, double embodiedTonsCO2e, double gridIntensityGPerKWh, double annualKWh)
        {
            double grossAvoided = capturedTonsPerYear;
            double operationalEmissions = (annualKWh * gridIntensityGPerKWh / 1000000.0);
            double netAvoided = grossAvoided - embodiedTonsCO2e - operationalEmissions;
            return Math.Max(0.0, netAvoided / grossAvoided); // normalized 0–1 score
        }
    }
}
