using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;

namespace Lines.Scripts.BallStuff.EventArgs
{
    public class BallSpawnedEventArgs : System.EventArgs
    {
        public Board Board { get; }
        public Index2D Slot { get; }

        public BallSpawnedEventArgs(Board board, Index2D slot)
        {
            Board = board;
            Slot = slot;
        }
    }
}
