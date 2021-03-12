using Lines.Scripts.BallStuff.EventArgs;
using Lines.Scripts.BallStuff.Managers;
using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;

namespace Lines.Scripts.BallStuff.Controllers
{
    public class BallLifespanController
    {
        protected readonly BallSpawnManager ballSpawnManager;
        protected readonly BallPopManager ballPopManager;

        public BallLifespanController(
            BallSpawnManager ballSpawnManager,
            BallPopManager ballPopManager
        )
        {
            this.ballSpawnManager = ballSpawnManager;
            this.ballPopManager = ballPopManager;
        }

        public void Enable()
        {
            ballSpawnManager.Board.OnBallMoved += OnBallMoved;
            ballSpawnManager.OnFewBallsSpawned += OnFewBallsSpawned;
        }

        public void Disable()
        {
            ballSpawnManager.Board.OnBallMoved -= OnBallMoved;
            ballSpawnManager.OnFewBallsSpawned -= OnFewBallsSpawned;
        }

        protected void OnBallMoved(object sender, BallMovedEventArgs e)
        {
            if (!ballPopManager.TryPopLines())
                ballSpawnManager.TrySpawnFewBalls();
        }
        
        protected void OnFewBallsSpawned(object sender, BallGroupSpawnedEventArgs e)
        {
            ballPopManager.TryPopLines();
        }
    }
}
