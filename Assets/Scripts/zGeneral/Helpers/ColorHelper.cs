using System;
using UnityEngine;

namespace Lines.Scripts.General.Helpers
{
    public enum Strength
    {
        Slight, Strong,
    }

    public enum ColorVariationType
    {
        Lighter, Darker, Auto
    }

    public static class ColorHelper
    {
        public static bool IsDark(Color color)
        {
            return Math.Sqrt(color.r * color.r + color.g * color.g + color.b * color.b) < Math.Sqrt(3) * 0.5;
        }

        public static Color32 GetColorVariation(
            Color32 color,
            Strength strength = Strength.Slight,
            ColorVariationType colorVariationType = ColorVariationType.Auto
        )
        {
            var difference = GetCoefficient(strength);
            var coefficient = colorVariationType switch {
                ColorVariationType.Lighter => 1 - difference,
                ColorVariationType.Darker => 1 + difference,
                ColorVariationType.Auto => IsDark(color) ? 1 - difference : 1 + difference,
                _ => 1.0f
            };

            return new Color32(
                (byte) (color.r * coefficient),
                (byte) (color.g * coefficient),
                (byte) (color.b * coefficient),
                color.a
            );
        }

        public static Color GetLighterColor(Color32 color, Strength strength)
        {
            return GetColorVariation(color, strength, ColorVariationType.Lighter);
        }

        public static Color GetDarkerColor(Color32 color, Strength strength)
        {
            return GetColorVariation(color, strength, ColorVariationType.Darker);
        }

        private static float GetCoefficient(Strength strength)
        {
            return strength switch {
                Strength.Slight => 0.33f, Strength.Strong => 0.67f, _ => 0.0f
            };
        }
    }
}
