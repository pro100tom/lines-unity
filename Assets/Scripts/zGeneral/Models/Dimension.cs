using System;
using UnityEngine;

namespace Lines.Scripts.General.Models
{
    [Serializable]
    public class Dimension
    {
        [SerializeField] protected int columnCount;
        [SerializeField] protected int rowCount;

        public Dimension(int columnCount, int rowCount)
        {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
        }

        public Dimension(Dimension dimension)
        {
            columnCount = dimension.GetColumnCount();
            rowCount = dimension.GetRowCount();
        }

        public int GetColumnCount()
        {
            return columnCount;
        }

        public int GetRowCount()
        {
            return rowCount;
        }
    }
}
