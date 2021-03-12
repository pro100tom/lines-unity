using System;

namespace Lines.Scripts.General.Models
{
    public class Index2D : IEquatable<Index2D>, IComparable
    {
        public int ColumnIndex { get; }
        public int RowIndex { get; }

        public Index2D() : this(0, 0)
        {
        }

        public Index2D(int columnIndex, int rowIndex)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }

        public bool Equals(Index2D other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ColumnIndex == other.ColumnIndex && RowIndex == other.RowIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (ReferenceEquals(this, obj)) 
                return true;
            
            if (obj.GetType() != GetType()) 
                return false;
            
            return Equals((Index2D) obj);
        }

        public override int GetHashCode()
        {
            unchecked {
                return (ColumnIndex * 397) ^ RowIndex;
            }
        }

        public int CompareTo(object obj)
        {
            if (ColumnIndex < ((Index2D)obj).ColumnIndex)
                return -1;

            if (ColumnIndex > ((Index2D)obj).ColumnIndex)
                return 1;
            
            if (RowIndex < ((Index2D)obj).RowIndex)
                return -1;

            if (RowIndex > ((Index2D)obj).RowIndex)
                return 1;

            return 0;
        }
    }
}
