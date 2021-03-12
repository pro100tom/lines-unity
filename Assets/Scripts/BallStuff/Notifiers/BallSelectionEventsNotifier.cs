using System;
using System.Linq;
using Lines.Scripts.BoardStuff.EventArgs;
using Lines.Scripts.BoardStuff.Notifiers;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Notifiers
{
    public class BallSelectionEventsNotifier
    {
        // Constructor arguments
        protected readonly MouseEventsNotifier mouseEventsNotifier;
        
        // Events
        protected EventHandler<MouseEventArgs> onBallSelected, onBallDeselected;

        // Fields
        protected GameObject selectedBall;
        
        public BallSelectionEventsNotifier(MouseEventsNotifier mouseEventsNotifier)
        {
            this.mouseEventsNotifier = mouseEventsNotifier;
        }

        public void Enable()
        {
            mouseEventsNotifier.Enable();
            mouseEventsNotifier.MouseReleased += MouseEventsReleased;
        }

        public void Disable()
        {
            mouseEventsNotifier.MouseReleased -= MouseEventsReleased;
        }
        
        protected void MouseEventsReleased(object sender, MouseEventArgs e)
        {
            if (!e.Board.TryGetBall(e.Slot, out var ball) || ball.Equals(selectedBall)) {
                selectedBall = null;
                onBallDeselected?.Invoke(this, e);
            } else {
                selectedBall = ball;
                onBallSelected?.Invoke(this, e);
            }
        }

        public event EventHandler<MouseEventArgs> OnBallSelected
        {
            add {
                if (onBallSelected == null || !onBallSelected.GetInvocationList().Contains(value))
                    onBallSelected += value;
            }
            remove => onBallSelected -= value;
        }
        
        public event EventHandler<MouseEventArgs> OnBallDeselected
        {
            add {
                if (onBallDeselected == null || !onBallDeselected.GetInvocationList().Contains(value))
                    onBallDeselected += value;
            }
            remove => onBallDeselected -= value;
        }
    }
}
