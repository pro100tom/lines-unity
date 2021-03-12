using Lines.Scripts.BallStuff.Notifiers;
using Lines.Scripts.BoardStuff.EventArgs;
using Lines.Scripts.Services;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Controllers
{
    public class BallMoveController
    {
        // Constructor arguments
        protected readonly BallSelectionEventsNotifier ballSelectionEventsNotifier;
        
        // DI
        protected Pathfinder pathfinder;
        
        // Fields
        protected GameObject selectedBall;

        protected BallMoveController(BallSelectionEventsNotifier ballSelectionEventsNotifier)
        {
            this.ballSelectionEventsNotifier = ballSelectionEventsNotifier;
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
            selectedBall = e.Board.GetBall(e.Slot);
        }
        
        protected void OnBallDeselected(object sender, MouseEventArgs e)
        {
            try {
                if (e.Board.HasBall(e.Slot)) {
                    return;
                }  

                if (!e.Board.TryGetSlotFromBall(selectedBall, out var slotFrom))
                    return;

                if (!pathfinder.TryFindShortestPath(e.Board, slotFrom, e.Slot, out var path))
                    return;

                if (path == null)
                    return;

                if (e.Board.TryMoveBall(slotFrom, e.Slot))
                    return;
            } finally {
                selectedBall = null;
            }
        }

        public class BallMoveControllerFactory
        {
            protected readonly Pathfinder pathfinder;
            
            public BallMoveControllerFactory(Pathfinder pathfinder)
            {
                this.pathfinder = pathfinder;
            }
            
            public BallMoveController Create(BallSelectionEventsNotifier ballSelectionEventsNotifier)
            {
                return new BallMoveController(ballSelectionEventsNotifier) {
                    pathfinder = pathfinder
                };
            }
        }
    }
}
