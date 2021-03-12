using Lines.Scripts.BallStuff.Managers;
using Lines.Scripts.BallStuff.Notifiers;
using Lines.Scripts.BoardStuff.EventArgs;

namespace Lines.Scripts.BallStuff.Controllers
{
    public class BallBounceController
    {
        // Constructor arguments
        protected readonly BallSelectionEventsNotifier ballSelectionEventsNotifier;
        protected readonly BallBounceManager ballBounceManager;

        public BallBounceController(
            BallSelectionEventsNotifier ballSelectionEventsNotifier,
            BallBounceManager ballBounceManager
        )
        {
            this.ballSelectionEventsNotifier = ballSelectionEventsNotifier;
            this.ballBounceManager = ballBounceManager;
        }

        public void Enable()
        {
            ballSelectionEventsNotifier.Enable();
            ballSelectionEventsNotifier.OnBallSelected += OnBallSelected;
            ballSelectionEventsNotifier.OnBallDeselected += OnBallDeselected;
        }

        public void Disable()
        {
            ballSelectionEventsNotifier.OnBallSelected -= OnBallSelected;
            ballSelectionEventsNotifier.OnBallDeselected -= OnBallDeselected;
        }

        protected void OnBallSelected(object sender, MouseEventArgs e)
        {
            if (!e.Board.HasBall(e.Slot))
                return;

            ballBounceManager.StopBounceAll();
            ballBounceManager.TryStartBounce(e.Slot);
        }
        
        protected void OnBallDeselected(object sender, MouseEventArgs e)
        {
            if (!e.Board.HasBall(e.Slot))
                return;
            
            ballBounceManager.TryStopBounce(e.Slot);
        }
    }
}
