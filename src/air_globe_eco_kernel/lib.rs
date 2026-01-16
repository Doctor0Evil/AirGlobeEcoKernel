pub struct AirGlobeCapture {
    pub flow_rate_m3_per_h: f64, // Q
    pub co2_in_ppm: f64,         // Cin
    pub co2_out_ppm: f64,        // Cout
    pub temperature_c: f64,      // T
    pub pressure_kpa: f64,       // P
}

impl AirGlobeCapture {
    pub fn captured_kg_per_h(&self) -> f64 {
        let delta_ppm = (self.co2_in_ppm - self.co2_out_ppm).max(0.0);
        let t_k = self.temperature_c + 273.15;
        let p_pa = self.pressure_kpa * 1000.0;
        let r = 8.314_462_618_f64;
        // dry air density kg/m³
        let rho_air = (p_pa * 0.028_96) / (r * t_k);
        let mass_per_m3_per_ppm = rho_air * (44.01 / 28.96) * 1.0e-6;
        self.flow_rate_m3_per_h * delta_ppm * mass_per_m3_per_ppm
    }

    pub fn captured_t_per_year(&self, uptime_fraction: f64) -> f64 {
        let uptime = uptime_fraction.clamp(0.0, 1.0);
        self.captured_kg_per_h() * 8760.0 * uptime / 1000.0
    }
}

pub struct EcoImpactInputs {
    pub captured_t_per_year: f64,
    pub embodied_t_co2e: f64,
    pub grid_intensity_g_per_kwh: f64,
    pub annual_kwh: f64,
}

pub fn net_eco_impact_score(input: &EcoImpactInputs) -> f64 {
    let gross = input.captured_t_per_year.max(0.0);
    if gross == 0.0 {
        return 0.0;
    }
    let operational_t = input.annual_kwh * input.grid_intensity_g_per_kwh / 1_000_000.0;
    let net = gross - input.embodied_t_co2e - operational_t;
    (net / gross).clamp(0.0, 1.0)
}
