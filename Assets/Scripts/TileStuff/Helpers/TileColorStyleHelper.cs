using Lines.Scripts.General.Helpers;
using Lines.Scripts.TileStuff.Models;
using UnityEngine;

namespace Lines.Scripts.TileStuff.Helpers
{
    public static class TileColorStyleHelper
    {
        public static TileColorGroup CreateVariantColorGroup(Color32 color)
        {
            return new TileColorGroup {
                IdleColor = color,
                MouseOverColor = ColorHelper.GetColorVariation(color),
                MouseDownColor = ColorHelper.GetColorVariation(color, Strength.Strong)
            };
        }
    }
}
