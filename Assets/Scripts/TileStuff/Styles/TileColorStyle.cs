using System;
using Lines.Scripts.TileStuff.Models;
using UnityEngine;

namespace Lines.Scripts.TileStuff.Styles
{
    [Serializable]
    public class TileColorStyle
    {
        [SerializeField] protected TileColorGroup foregroundColorGroup;
        [SerializeField] protected TileColorGroup borderColorGroup;

        public TileColorGroup ForegroundColorGroup
        {
            get => foregroundColorGroup;
            protected set => foregroundColorGroup = value;
        }

        public TileColorGroup BorderColorGroup
        {
            get => borderColorGroup;
            protected set => borderColorGroup = value;
        }

        public TileColorStyle(TileColorGroup foregroundColorGroup, TileColorGroup borderColorGroup)
        {
            this.foregroundColorGroup = foregroundColorGroup;
            this.borderColorGroup = borderColorGroup;
        }
    }
}
