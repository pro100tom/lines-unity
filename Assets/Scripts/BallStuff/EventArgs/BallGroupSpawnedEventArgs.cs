using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;

namespace Lines.Scripts.BallStuff.EventArgs
{
    public class BallGroupSpawnedEventArgs : System.EventArgs
    {
        public Board Board { get; }
        public Index2D[] Slots { get; }

        public BallGroupSpawnedEventArgs(Board board, Index2D[] slots)
        {
            Board = board;
            Slots = slots;
        }
    }
}
