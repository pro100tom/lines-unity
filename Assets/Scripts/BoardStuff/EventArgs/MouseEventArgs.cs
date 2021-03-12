using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;

namespace Lines.Scripts.BoardStuff.EventArgs
{
    public class MouseEventArgs : System.EventArgs
    {
        public Board Board { get; }
        public Index2D Slot { get; }

        public MouseEventArgs(Board board, Index2D slot)
        {
            Board = board;
            Slot = slot;
        }
    }
}
