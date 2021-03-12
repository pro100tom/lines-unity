using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;
using UnityEngine;

namespace Lines.Scripts.BallStuff.EventArgs
{
    public class BallMovedEventArgs : System.EventArgs
    {
        public Board Board { get; }
        public Index2D SlotFrom { get; }
        public Index2D SlotTo { get; }
        public GameObject Ball { get; }

        public BallMovedEventArgs(Board board, Index2D slotFrom, Index2D slotTo, GameObject ball)
        {
            Board = board;
            SlotFrom = slotFrom;
            SlotTo = slotTo;
            Ball = ball;
        }
    }
}
