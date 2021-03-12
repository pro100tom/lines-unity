using Lines.Scripts.BallStuff.Behaviour;
using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Managers
{
    public class BallBounceManager
    {
        // Constructor arguments
        public Board Board { get; protected set; }

        public BallBounceManager(Board board)
        {
            Board = board;
        }

        public bool TryStartBounce(Index2D slot)
        {
            if (!Board.TryGetBall(slot, out var ball))
                return false;

            GetBounceComponent(ball).BounceStart();

            return true;
        }

        public bool TryStopBounce(Index2D slot)
        {
            if (!Board.TryGetBall(slot, out var ball))
                return false;

            GetBounceComponent(ball).BounceStop();

            return true;
        }

        public bool TryToggleBounce(Index2D slot)
        {
            if (!Board.TryGetBall(slot, out var ball))
                return false;

            GetBounceComponent(ball).BounceToggle();

            return true;
        }

        public bool IsBouncing(Index2D slot)
        {
            if (!Board.TryGetBall(slot, out var ball))
                return false;

            return GetBounceComponent(ball).IsBouncing;
        }

        public void StopBounceAll()
        {
            foreach (var ball in Board.GetBalls())
                GetBounceComponent(ball).BounceStop();
        }

        protected Bounce GetBounceComponent(GameObject ball)
        {
            var bounce = ball.GetComponent<Bounce>();
            bounce ??= ball.AddComponent<Bounce>();

            return bounce;
        }
    }
}
