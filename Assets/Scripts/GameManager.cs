using Lines.Scripts.BallStuff.Controllers;
using Lines.Scripts.BallStuff.Managers;
using Lines.Scripts.BallStuff.Models;
using Lines.Scripts.BallStuff.Notifiers;
using Lines.Scripts.BallStuff.Settings;
using Lines.Scripts.BoardStuff.Controllers;
using Lines.Scripts.BoardStuff.Notifiers;
using Lines.Scripts.BoardStuff.Settings;
using Lines.Scripts.Services;
using Lines.Scripts.TileStuff.Helpers;
using Lines.Scripts.TileStuff.Models;
using Lines.Scripts.TileStuff.Styles;
using UnityEngine;
using static Lines.Scripts.BallStuff.Controllers.BallMoveController;
using static Lines.Scripts.BallStuff.Managers.BallSpawnManager;
using static Lines.Scripts.BallStuff.Styles.BallStyleRepository;
using static Lines.Scripts.BoardStuff.Models.Board;

namespace Lines.Scripts
{
    public class GameManager : MonoBehaviour
    {
        protected BallSpawnManager ballSpawnManager;

        public void Awake()
        {
            var tileStyleRepository = new TileStyleRepository();
            var tileStyle = tileStyleRepository.ObtainDefault();
            var tileFactory = new TileFactory();
            var boardFactory = new BoardFactory(tileFactory);

            var boardSettingsRepository = new BoardSettingsRepository();
            var boardSettings = boardSettingsRepository.ObtainDefault();
            var board = boardFactory.Create(boardSettings, tileStyle);

            var boardMouseEventsNotifier = new MouseEventsNotifier(board);
            boardMouseEventsNotifier.Enable();

            var ballSelectionEventsNotifier = new BallSelectionEventsNotifier(boardMouseEventsNotifier);
            ballSelectionEventsNotifier.Enable();

            var pathfinder = new Pathfinder();
            var ballMoveControllerFactory = new BallMoveControllerFactory(pathfinder);
            var ballMoveController = ballMoveControllerFactory.Create(ballSelectionEventsNotifier);
            ballMoveController.Enable();

            var ballBounceManager = new BallBounceManager(board);
            var ballBounceController = new BallBounceController(ballSelectionEventsNotifier, ballBounceManager);
            ballBounceController.Enable();

            var ballMaterialSettingsRepository = new BallMaterialSettingsRepository();
            var ballStyleRepository = new BallStyleRepositoryFactory(ballMaterialSettingsRepository).Create();
            var ballLifetimeManagerSettingsRepository = new BallLifetimeManagerSettingsRepository();
            var ballLifetimeManagerSettings = ballLifetimeManagerSettingsRepository.ObtainDefault();
            var ballFactory = new BallFactory();
            var random = new System.Random();
            var ballSpawnManagerFactory = new BallSpawnManagerFactory(
                ballLifetimeManagerSettings,
                ballStyleRepository,
                ballFactory,
                random
            );
            var ballColorPool = new BallColorPoolRepository().ObtainDefault();
            ballSpawnManager = ballSpawnManagerFactory.Create(board, ballColorPool);
            var ballPopManager = new BallPopManager(board);
            var ballLifespanController = new BallLifespanController(ballSpawnManager, ballPopManager);
            ballLifespanController.Enable();

            var tileColorStyle = new TileColorStyle(
                TileColorStyleHelper.CreateVariantColorGroup(
                    tileStyle.ColorStyle.ForegroundColorGroup.IdleColor
                ),
                tileStyle.ColorStyle.BorderColorGroup
            );
            var boardHighlightController = new BoardHighlightController(boardMouseEventsNotifier, tileColorStyle);
            boardHighlightController.Enable();
        }

        public void Start()
        {
            ballSpawnManager.TrySpawnFewBalls();
        }
    }
}
