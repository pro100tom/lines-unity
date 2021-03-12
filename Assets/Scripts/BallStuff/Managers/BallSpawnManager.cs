using System;
using UnityEngine;
using Lines.Scripts.BallStuff.Settings;
using System.Collections.Generic;
using System.Linq;
using Lines.Scripts.BallStuff.EventArgs;
using Lines.Scripts.BallStuff.Models;
using Lines.Scripts.BallStuff.Styles;
using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;
using Random = System.Random;

namespace Lines.Scripts.BallStuff.Managers
{
    public class BallSpawnManager
    {
        // Constructor arguments
        public Board Board { get; protected set; }
        public BallColorPool BallColorPool { get; protected set; }

        // DI
        protected BallLifetimeManagerSettings ballLifetimeManagerSettings;
        protected BallStyleRepository ballStyleRepository;
        protected BallFactory ballFactory;
        protected Random random;

        // Events
        protected EventHandler<BallGroupSpawnedEventArgs> onFewBallsSpawned;

        protected BallSpawnManager(Board board, BallColorPool ballColorPool)
        {
            Board = board;
            BallColorPool = ballColorPool;
        }

        public bool TrySpawnFewBalls()
        {
            var slots = new List<Index2D>();

            void OnBallSpawned(object sender, BallSpawnedEventArgs e) => slots.Add(e.Slot);
            Board.OnBallSpawned += OnBallSpawned;
            for (int numberOfBalls = 0; numberOfBalls < random.Next(GetMin(Board), GetMax(Board)); numberOfBalls++)
                TrySpawnBall();
            Board.OnBallSpawned -= OnBallSpawned;
            
            var isSuccessful = slots.Count > 1;
            if (isSuccessful)
                onFewBallsSpawned?.Invoke(this, new BallGroupSpawnedEventArgs(Board, slots.ToArray()));
            
            return isSuccessful;
        }

        public bool TrySpawnBall(Index2D slot = null, Color32? color = null)
        {
            if (slot == null)
                if (!TryGetRandomAvailableSlot(Board, out slot))
                    return false;

            if (!Board.TryGetTile(slot, out var tile))
                return false;

            color ??= BallColorPool.GetRandomColor();

            var ballStyle = ballStyleRepository.ObtainDefault();
            var tilePosition = tile.transform.position;
            var ballPosition = new Vector3(
                tilePosition.x,
                (Board.TileStyle.Size + ballStyle.UniformScale) * 0.5f,
                tilePosition.z
            );
            var ball = ballFactory.Create(ballPosition, color.Value, ballStyle);
            ball.name = "Ball_" + slot.ColumnIndex + "_" + slot.RowIndex;
            ball.AddComponent<Rigidbody>();

            return Board.TrySpawnBall(ball, slot);
        }

        protected bool TryGetRandomAvailableSlot(Board board, out Index2D availableRandomSlot)
        {
            availableRandomSlot = null;
            if (!TryGetAvailableSlots(board, out Index2D[] slots))
                return false;

            availableRandomSlot = slots[random.Next(slots.Length)];

            return true;
        }

        protected bool TryGetAvailableSlots(Board board, out Index2D[] availableSlots)
        {
            var slots = new List<Index2D>();
            var dimension = board.BoardSettings.GetDimension();
            for (int columnIndex = 0; columnIndex < dimension.GetColumnCount(); columnIndex++) {
                for (int rowIndex = 0; rowIndex < dimension.GetRowCount(); rowIndex++) {
                    var slot = new Index2D(columnIndex, rowIndex);
                    if (!board.HasBall(slot))
                        slots.Add(slot);
                }
            }

            availableSlots = slots.ToArray();

            return slots.Any();
        }

        protected int GetMin(Board board)
        {
            int min = ballLifetimeManagerSettings.GetSpawnQtyMin();
            int max = GetMax(board);

            if (min >= max)
                min = max - 1;

            return min;
        }

        protected int GetMax(Board board)
        {
            int max = ballLifetimeManagerSettings.GetSpawnQtyMax() + 1;
            int numberOfAvailableSlots = board.GetEmptySlotCount();
            if (numberOfAvailableSlots < max)
                max = numberOfAvailableSlots + 1;

            return max;
        }

        public event EventHandler<BallGroupSpawnedEventArgs> OnFewBallsSpawned
        {
            add {
                if (onFewBallsSpawned == null || !onFewBallsSpawned.GetInvocationList().Contains(value))
                    onFewBallsSpawned += value;
            }
            remove => onFewBallsSpawned -= value;
        }

        public class BallSpawnManagerFactory
        {
            protected readonly BallLifetimeManagerSettings ballLifetimeManagerSettings;
            protected readonly BallStyleRepository ballStyleRepository;
            protected readonly BallFactory ballFactory;
            protected readonly Random random;

            public BallSpawnManagerFactory(
                BallLifetimeManagerSettings ballLifetimeManagerSettings,
                BallStyleRepository ballStyleRepository,
                BallFactory ballFactory,
                Random random
            )
            {
                this.ballLifetimeManagerSettings = ballLifetimeManagerSettings;
                this.ballStyleRepository = ballStyleRepository;
                this.ballFactory = ballFactory;
                this.random = random;
            }

            public BallSpawnManager Create(Board board, BallColorPool ballColorPool)
            {
                return new BallSpawnManager(board, ballColorPool) {
                    ballLifetimeManagerSettings = ballLifetimeManagerSettings,
                    ballStyleRepository = ballStyleRepository,
                    ballFactory = ballFactory,
                    random = random,
                };
            }
        }
    }
}
