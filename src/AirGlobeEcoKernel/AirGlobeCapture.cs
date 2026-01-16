using System;

namespace AirGlobeEcoKernel
{
    public class AirGlobeCapture
    {
        public double FlowRateM3PerHour { get; init; }   // Air volume processed
        public double CO2InPpm { get; init; }            // Inlet concentration
        public double CO2OutPpm { get; init; }           // Outlet concentration
        public double TemperatureC { get; init; } = 25.0;
        public double PressureKPa { get; init; } = 101.325;

        // CO2 mass concentration kg/m³ per ppm at STP, scaled for T/P
        private double CO2MassPerM3PerPpm()
        {
            const double molarMassCO2 = 44.01;
            const double molarMassAir = 28.96;
            const double stdMolarVolume = 22.414; // L/mol at 0°C, 101.325 kPa
            double densityAdjustment = (273.15 / (TemperatureC + 273.15)) * (PressureKPa / 101.325);
            return (molarMassCO2 / molarMassAir) * (1.0 / (stdMolarVolume * densityAdjustment * 1000.0));
        }

        public double CapturedKgPerHour()
        {
            double deltaPpm = CO2InPpm - CO2OutPpm;
            return FlowRateM3PerHour * deltaPpm * CO2MassPerM3PerPpm();
        }

        public double CapturedTonsPerYear()
        {
            return CapturedKgPerHour() * 8760.0 / 1000.0; // metric tons, assuming 100% uptime
        }
    }
}
