using System.Text.Json;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.Services.ExternalService.AI.Prompts;

public static class FlexibleMeasurementPrompts
{
    public static string GetFlexibleCalculationPrompt(
        MeasurementDiaryDto diary,
        MeasurementCreateDto? currentInput,
        Measurement? lastMeasurement,
        int targetWeek)
    {
        var prompt = $@"
You are an expert AI specializing in pregnancy body measurements and maternity clothing design.
Your task is to calculate comprehensive body measurements for a pregnant woman.

Patient Profile:
- Age: {diary.Age} years
- Height: {diary.Height} cm
- Pre-pregnancy weight: {diary.Weight} kg
- Pre-pregnancy measurements:
  * Bust: {diary.Bust} cm
  * Waist: {diary.Waist} cm
  * Hip: {diary.Hip} cm
- Current pregnancy week: {targetWeek}

";

        if (lastMeasurement != null)
        {
            prompt += $@"
Previous Measurements (Week {lastMeasurement.WeekOfPregnancy}):
- Weight: {lastMeasurement.Weight} kg
- Neck: {lastMeasurement.Neck} cm
- Coat: {lastMeasurement.Coat} cm
- Bust: {lastMeasurement.Bust} cm
- ChestAround: {lastMeasurement.ChestAround} cm
- Stomach: {lastMeasurement.Stomach} cm
- PantsWaist: {lastMeasurement.PantsWaist} cm
- Thigh: {lastMeasurement.Thigh} cm
- DressLength: {lastMeasurement.DressLength} cm
- SleeveLength: {lastMeasurement.SleeveLength} cm
- ShoulderWidth: {lastMeasurement.ShoulderWidth} cm
- Waist: {lastMeasurement.Waist} cm
- LegLength: {lastMeasurement.LegLength} cm
- Hip: {lastMeasurement.Hip} cm

";
        }

        if (currentInput != null && currentInput.Weight > 0)
        {
            prompt += $@"
Current Manual Input:
- Weight: {currentInput.Weight} kg
- Bust: {currentInput.Bust} cm
- Waist: {currentInput.Waist} cm
- Hip: {currentInput.Hip} cm

";
        }

        prompt += $@"
IMPORTANT CONTEXT:
1. This is for maternity clothing design, so measurements need to be practical and comfortable
2. Consider natural body changes during pregnancy:
   - Weight gain patterns based on pre-pregnancy BMI
   - Breast enlargement for milk production
   - Abdominal expansion for growing baby
   - Hip widening for childbirth preparation
   - Potential swelling in extremities
3. Clothing measurements should provide comfort:
   - Coat size needs extra room for layering
   - PantsWaist sits below the belly
   - Stomach measurement is for maternity wear
   - Thigh measurement considers potential swelling

Calculate ALL measurements intelligently based on:
- The woman's body proportions and height
- Natural pregnancy progression at week {targetWeek}
- Realistic body changes and weight distribution
- Comfort requirements for maternity clothing
- Individual variation (not everyone follows the same pattern)

Return ONLY a JSON object with these measurements (all values must be positive numbers):
{{
  ""weight"": [realistic weight for week {targetWeek}],
  ""neck"": [proportional to body frame and potential swelling],
  ""coat"": [bust measurement with comfortable extra room],
  ""bust"": [considering breast changes in pregnancy],
  ""chestAround"": [under-bust measurement for support garments],
  ""stomach"": [maximum abdominal circumference],
  ""pantsWaist"": [below-belly measurement for maternity pants],
  ""thigh"": [considering potential swelling and weight gain],
  ""dressLength"": [appropriate length based on height],
  ""sleeveLength"": [based on arm length and height],
  ""shoulderWidth"": [based on frame size],
  ""waist"": [natural waist, above belly],
  ""legLength"": [inseam measurement],
  ""hip"": [considering pelvic changes]
}}

Think step by step about each measurement and how it relates to the woman's body and pregnancy stage.
";

        return prompt;
    }
}