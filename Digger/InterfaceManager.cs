using Digger.Data;

namespace Digger
{
    class InterfaceManager
    {
        private GameSettings _gameSettings;
        private GameView _recentGameView;
        private MenuView _recentMenuView;



        public InterfaceManager(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public GameView GetNewGame()
        {
            _recentGameView = new GameView();
            return _recentGameView;
        }

        public GameView GetPausedGame()
        {
            return _recentGameView;
        }

        public MenuView GetNewMenu()
        {
            _recentMenuView = new MenuView();
            return _recentMenuView;
        }
    }
}
