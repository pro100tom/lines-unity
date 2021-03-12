using System.Linq;
using Lines.Scripts.TileStuff.Styles;
using UnityEngine;

namespace Lines.Scripts.TileStuff.Models
{
    public class TileFactory
    {
        public GameObject Create(TileStyle tileStyle)
        {
            var tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var tileSize = tileStyle.Size;
            tile.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
            var renderer = tile.GetComponent<Renderer>();
            var materials = tileStyle.MaterialNames.Select(Resources.Load<Material>).ToArray();
            renderer.materials = materials;

            return tile;
        }
    }
}
