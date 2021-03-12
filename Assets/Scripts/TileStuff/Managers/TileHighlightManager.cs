using UnityEngine;

namespace Lines.Scripts.TileStuff.Managers
{
    public class TileHighlightManager
    {
        public void SetTileColor(GameObject tile, Color32 foregroundColor, Color32 borderColor)
        {
            SetForegroundColor(tile, foregroundColor);
            SetBorderColor(tile, borderColor);
        }
        
        public void SetForegroundColor(GameObject tile, Color32 color)
        {
            var materials = GetMaterials(tile);
            if (materials.Length > 0)
                materials[0].color = color;
        }

        public void SetBorderColor(GameObject tile, Color32 color)
        {
            var materials = GetMaterials(tile);
            if (materials.Length > 1)
                materials[1].color = color;
        }

        protected Material[] GetMaterials(GameObject tile)
        {
            return tile.GetComponent<Renderer>().materials;
        }
    }
}
