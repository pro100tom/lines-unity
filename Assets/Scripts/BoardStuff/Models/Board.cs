using System;
using System.Collections.Generic;
using System.Linq;
using Lines.Scripts.BallStuff.EventArgs;
using UnityEngine;
using Lines.Scripts.BoardStuff.Settings;
using Lines.Scripts.General.Models;
using Lines.Scripts.TileStuff.Models;
using Lines.Scripts.TileStuff.Styles;
using Object = UnityEngine.Object;

namespace Lines.Scripts.BoardStuff.Models
{
    public class Board
    {
        // Constructor arguments
        public BoardSettings BoardSettings { get; protected set; }
        public TileStyle TileStyle { get; protected set; }

        // Fields
        protected GameObject[,] tiles;
        protected Dictionary<Index2D, GameObject> ballMap;

        // Events
        protected EventHandler<BallSpawnedEventArgs> onBallSpawned;
        protected EventHandler<BallMovedEventArgs> onBallMoved;

        protected Board(BoardSettings boardSettings, TileStyle tileStyle)
        {
            BoardSettings = boardSettings;
            TileStyle = tileStyle;
        }

        public bool TrySpawnBall(GameObject ball, Index2D slot)
        {
            if (!IsSlotValid(slot))
                return false;

            ballMap[slot] = ball;
            onBallSpawned?.Invoke(this, new BallSpawnedEventArgs(this, slot));

            return true;
        }

        public bool TryGetBall(GameObject tile, out GameObject ball)
        {
            ball = null;
            if (!TryGetSlotFromTile(tile, out var slot))
                return false;

            return TryGetBall(slot, out ball);
        }

        public bool TryGetBall(Index2D slot, out GameObject ball)
        {
            ball = null;
            if (!ballMap.ContainsKey(slot))
                return false;
            
            ball = ballMap[slot];

            return ball != null;
        }

        public GameObject GetBall(Index2D slot)
        {
            if (!TryGetBall(slot, out var ball))
                throw new ArgumentException();
            
            return ball;
        }

        public IEnumerable<GameObject> GetTiles()
        {
            return tiles.Cast<GameObject>();
        }

        public bool TryGetTile(Index2D slot, out GameObject tile)
        {
            tile = null;
            if (!IsSlotValid(slot))
                return false;

            tile = tiles[slot.ColumnIndex, slot.RowIndex];

            return true;
        }

        public bool TryGetTile(GameObject ball, out GameObject tile)
        {
            tile = null;
            if (!TryGetSlotFromBall(ball, out var slot))
                return false;

            if (!TryGetTile(slot, out tile))
                return false;

            return true;
        }

        public bool TryMoveBall(Index2D slotFrom, Index2D slotTo)
        {
            if (!TryGetBall(slotFrom, out var ballToMove))
                return false;

            if (TryGetBall(slotTo, out _))
                return false;

            ballMap[slotFrom] = null;
            ballMap[slotTo] = ballToMove;

            if (!TryGetTile(slotTo, out var tile))
                return false;

            ballToMove.transform.position = ObtainNewBallPosition(tile, ballToMove.transform.position.y);
            onBallMoved?.Invoke(this, new BallMovedEventArgs(this, slotFrom, slotTo, ballToMove));

            return true;
        }

        public IEnumerable<GameObject> GetBalls()
        {
            return from item in ballMap where item.Value != null select item.Value;
        }

        public bool TryGetSlotFromBall(GameObject ball, out Index2D slot)
        {
            slot = ballMap.FirstOrDefault(x => x.Value == ball).Key;

            return ballMap.ContainsValue(ball);
        }

        public bool TryGetSlotFromTile(GameObject tile, out Index2D slot)
        {
            var columnCount = BoardSettings.GetDimension().GetColumnCount();
            var rowCount = BoardSettings.GetDimension().GetRowCount();
            slot = null;

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++) {
                    if (tiles[columnIndex, rowIndex] != tile)
                        continue;

                    slot = new Index2D(columnIndex, rowIndex);

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Index2D> GetEmptySlots()
        {
            return from item in ballMap where item.Value == null select item.Key;
        }
        
        public IEnumerable<Index2D> GetSlots()
        {
            return from item in ballMap select item.Key;
        }

        public int GetEmptySlotCount()
        {
            return GetEmptySlots().Count();
        }

        public bool IsSlotValid(Index2D slot)
        {
            if (slot.ColumnIndex < 0 || slot.ColumnIndex > BoardSettings.GetDimension().GetColumnCount())
                return false;

            if (slot.RowIndex < 0 || slot.RowIndex > BoardSettings.GetDimension().GetRowCount())
                return false;

            return true;
        }

        public bool TryPopBall(Index2D slot)
        {
            if (!TryGetBall(slot, out var ball))
                return false;

            ballMap[slot] = null;
            Object.DestroyImmediate(ball);
            
            return true;
        }

        public bool TryPopBall(GameObject ball)
        {
            if (!TryGetSlotFromBall(ball, out var slot))
                return false;

            return TryPopBall(slot);
        }

        public bool HasBall(Index2D slot)
        {
            return ballMap.ContainsKey(slot) && ballMap[slot] != null;
        }

        public bool HasBall(GameObject tile)
        {
            if (!TryGetSlotFromTile(tile, out var slot))
                return false;

            return HasBall(slot);
        }

        protected Vector3 ObtainNewBallPosition(GameObject tile, float ballPositionY)
        {
            var tilePosition = tile.transform.position;

            return new Vector3(tilePosition.x, ballPositionY, tilePosition.z);
        }

        public event EventHandler<BallSpawnedEventArgs> OnBallSpawned
        {
            add {
                if (onBallSpawned == null || !onBallSpawned.GetInvocationList().Contains(value))
                    onBallSpawned += value;
            }
            remove => onBallSpawned -= value;
        }

        public event EventHandler<BallMovedEventArgs> OnBallMoved
        {
            add {
                if (onBallMoved == null || !onBallMoved.GetInvocationList().Contains(value))
                    onBallMoved += value;
            }
            remove => onBallMoved -= value;
        }
        
        public class BoardFactory
        {
            protected readonly TileFactory tileFactory;

            public BoardFactory(TileFactory tileFactory)
            {
                this.tileFactory = tileFactory;
            }

            public Board Create(BoardSettings boardSettings, TileStyle tileStyle)
            {
                var board = new Board(boardSettings, tileStyle);

                var columnCount = boardSettings.GetDimension().GetColumnCount();
                var rowCount = boardSettings.GetDimension().GetRowCount();
                float xOffset = tileStyle.Size * columnCount * 0.5f;
                float zOffset = tileStyle.Size * rowCount * 0.5f;
                board.tiles = new GameObject[columnCount, rowCount];
                board.ballMap = new Dictionary<Index2D, GameObject>();

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++) {
                        var tile = tileFactory.Create(tileStyle);
                        float posX = columnIndex * tileStyle.Size - xOffset + 0.5f;
                        float posZ = -(rowIndex * tileStyle.Size - zOffset + 0.5f);

                        tile.transform.position = new Vector3(posX, 0, posZ);
                        board.tiles[columnIndex, rowIndex] = tile;

                        board.ballMap[new Index2D(columnIndex, rowIndex)] = null;
                    }
                }
                
                return board;
            }
        }
    }
}
