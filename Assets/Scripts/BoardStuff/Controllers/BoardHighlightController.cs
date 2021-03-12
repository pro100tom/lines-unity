using Lines.Scripts.BoardStuff.EventArgs;
using Lines.Scripts.TileStuff.Managers;
using Lines.Scripts.TileStuff.Styles;
using UnityEngine;
using BoardMouseEventsNotifier = Lines.Scripts.BoardStuff.Notifiers.MouseEventsNotifier;

namespace Lines.Scripts.BoardStuff.Controllers
{
    public class BoardHighlightController
    {
        // Constructor arguments
        protected readonly BoardMouseEventsNotifier boardMouseEventsNotifier;
        public TileColorStyle TileColorStyle { get; }

        // DI
        protected TileHighlightManager tileHighlightManager;

        protected BoardHighlightController(
            BoardMouseEventsNotifier boardMouseEventsNotifier,
            TileColorStyle tileColorStyle
        )
        {
            this.boardMouseEventsNotifier = boardMouseEventsNotifier;
            this.TileColorStyle = tileColorStyle;
        }

        public void Enable()
        {
            boardMouseEventsNotifier.MouseEntered += OnMouseEntered;
            boardMouseEventsNotifier.MousePressedDown += OnMousePressedDown;
            boardMouseEventsNotifier.MouseReleased += OnMouseReleased;
            boardMouseEventsNotifier.MouseExited += OnMouseExited;
        }

        public void Disable()
        {
            boardMouseEventsNotifier.MouseEntered -= OnMouseEntered;
            boardMouseEventsNotifier.MousePressedDown -= OnMousePressedDown;
            boardMouseEventsNotifier.MouseReleased -= OnMouseReleased;
            boardMouseEventsNotifier.MouseExited -= OnMouseExited;
        }

        protected void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!e.Board.TryGetTile(e.Slot, out var tile))
                return;

            OnMouseOver(tile);
        }

        protected void OnMousePressedDown(object sender, MouseEventArgs e)
        {
            if (!e.Board.TryGetTile(e.Slot, out var tile))
                return;

            tileHighlightManager.SetTileColor(
                tile,
                tileColorStyle.ForegroundColorGroup.MouseDownColor,
                tileColorStyle.BorderColorGroup.MouseDownColor
            );
        }

        protected void OnMouseReleased(object sender, MouseEventArgs e)
        {
            if (!e.Board.TryGetTile(e.Slot, out var tile))
                return;

            OnMouseOver(tile);
        }
        
        protected void OnMouseOver(GameObject tile)
        {
            tileHighlightManager.SetTileColor(
                tile,
                tileColorStyle.ForegroundColorGroup.MouseOverColor,
                tileColorStyle.BorderColorGroup.MouseOverColor
            );
        }

        protected void OnMouseExited(object sender, MouseEventArgs e)
        {
            if (!e.Board.TryGetTile(e.Slot, out var tile))
                return;

            tileHighlightManager.SetTileColor(
                tile,
                tileColorStyle.ForegroundColorGroup.IdleColor,
                tileColorStyle.BorderColorGroup.IdleColor
            );
        }

        public class BoardHighlightControllerFactory
        {
            public TileHighlightManager TileHighlightManager { get; set; } = new TileHighlightManager();

            public BoardHighlightController Create(
                BoardMouseEventsNotifier boardMouseEventsNotifier,
                TileColorStyle tileColorStyle
            )
            {
                return new BoardHighlightController(boardMouseEventsNotifier, tileColorStyle) {
                    tileHighlightManager = TileHighlightManager
                };
            }
        }
    }
}
