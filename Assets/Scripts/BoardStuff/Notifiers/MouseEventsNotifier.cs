using System;
using System.Linq;
using Lines.Scripts.BallStuff.EventArgs;
using GeneralMouseEventArgs = Lines.Scripts.General.EventArgs.MouseEventArgs;
using BoardMouseEventArgs = Lines.Scripts.BoardStuff.EventArgs.MouseEventArgs;
using Lines.Scripts.BoardStuff.Models;

namespace Lines.Scripts.BoardStuff.Notifiers
{
    public class MouseEventsNotifier
    {
        // Constructor arguments
        public Board Board { get; protected set; }
        
        // Events
        protected EventHandler<BoardMouseEventArgs> mouseEntered, mousePressedDown, mouseReleased, mouseExited;

        public MouseEventsNotifier(Board board)
        {
            Board = board;
        }

        public void Enable()
        {
            AddTileMouseInteraction();
            AddBallMouseInteraction();
        }

        public void Disable()
        {
            RemoveTileMouseInteraction();
            RemoveBallMouseInteraction();
        }
        
        protected void AddTileMouseInteraction()
        {
            foreach (var tile in Board.GetTiles()) {
                var mouseInteraction = tile.GetComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction ??= tile.AddComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction.MouseEnter += InvokeOnTileMouseEntered;
                mouseInteraction.MouseDown += InvokeOnTileMousePressedDown;
                mouseInteraction.MouseUp += InvokeOnTileMouseReleased;
                mouseInteraction.MouseExit += InvokeOnTileMouseExited;
            }
        }

        protected void AddBallMouseInteraction()
        {
            foreach (var ball in Board.GetBalls()) {
                var mouseInteraction = ball.GetComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction ??= ball.AddComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction.MouseEnter += InvokeOnBallMouseEntered;
                mouseInteraction.MouseDown += InvokeOnBallMousePressedDown;
                mouseInteraction.MouseUp += InvokeOnBallMouseReleased;
                mouseInteraction.MouseExit += InvokeOnBallMouseExited;
            }

            Board.OnBallSpawned += AddBallMouseInteractionWrapper;
        }

        protected void AddBallMouseInteractionWrapper(object sender, BallSpawnedEventArgs e)
        {
            AddBallMouseInteraction();
        }

        protected void RemoveTileMouseInteraction()
        {
            foreach (var tile in Board.GetTiles()) {
                var mouseInteraction = tile.GetComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction ??= tile.AddComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction.MouseEnter -= InvokeOnTileMouseEntered;
                mouseInteraction.MouseDown -= InvokeOnTileMousePressedDown;
                mouseInteraction.MouseUp -= InvokeOnTileMouseReleased;
                mouseInteraction.MouseExit -= InvokeOnTileMouseExited;
            }
        }

        protected void RemoveBallMouseInteraction()
        {
            foreach (var ball in Board.GetBalls()) {
                var mouseInteraction = ball.GetComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction ??= ball.AddComponent<General.Notifiers.MouseEventsNotifier>();
                mouseInteraction.MouseEnter -= InvokeOnBallMouseEntered;
                mouseInteraction.MouseDown -= InvokeOnBallMousePressedDown;
                mouseInteraction.MouseUp -= InvokeOnBallMouseReleased;
                mouseInteraction.MouseExit -= InvokeOnBallMouseExited;
            }

            Board.OnBallSpawned -= RemoveBallMouseInteractionWrapper;
        }
        
        protected void RemoveBallMouseInteractionWrapper(object sender, BallSpawnedEventArgs e)
        {
            RemoveBallMouseInteraction();
        }

        protected void InvokeOnTileMouseEntered(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromTile(e.GameObject, out var slot))
                return;
         
            mouseEntered?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnTileMousePressedDown(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromTile(e.GameObject, out var slot))
                return;
         
            mousePressedDown?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnTileMouseReleased(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromTile(e.GameObject, out var slot))
                return;
         
            mouseReleased?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnTileMouseExited(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromTile(e.GameObject, out var slot))
                return;
         
            mouseExited?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnBallMouseEntered(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromBall(e.GameObject, out var slot))
                return;

            mouseEntered?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }

        protected void InvokeOnBallMousePressedDown(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromBall(e.GameObject, out var slot))
                return;

            mousePressedDown?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnBallMouseReleased(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromBall(e.GameObject, out var slot))
                return;

            mouseReleased?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }
        
        protected void InvokeOnBallMouseExited(object sender, GeneralMouseEventArgs e)
        {
            if (!Board.TryGetSlotFromBall(e.GameObject, out var slot))
                return;

            mouseExited?.Invoke(this, new BoardMouseEventArgs(Board, slot));
        }

        public event EventHandler<BoardMouseEventArgs> MouseEntered
        {
            add {
                if (mouseEntered == null || !mouseEntered.GetInvocationList().Contains(value))
                    mouseEntered += value;
            }
            remove => mouseEntered -= value;
        }
        
        public event EventHandler<BoardMouseEventArgs> MousePressedDown
        {
            add {
                if (mousePressedDown == null || !mousePressedDown.GetInvocationList().Contains(value))
                    mousePressedDown += value;
            }
            remove => mousePressedDown -= value;
        }

        public event EventHandler<BoardMouseEventArgs> MouseReleased
        {
            add {
                if (mouseReleased == null || !mouseReleased.GetInvocationList().Contains(value))
                    mouseReleased += value;
            }
            remove => mouseReleased -= value;
        }
        
        public event EventHandler<BoardMouseEventArgs> MouseExited
        {
            add {
                if (mouseExited == null || !mouseExited.GetInvocationList().Contains(value))
                    mouseExited += value;
            }
            remove => mouseExited -= value;
        }
    }
}
