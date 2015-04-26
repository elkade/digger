using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Manager
{

    public class InputHandler
    {
        #region Singleton
        public static InputHandler Instance { get { return _instance; } }

        private static readonly InputHandler _instance = new InputHandler();
        #endregion

        private InputHandler()
        {
            CurrentKeyboardState = new KeyboardState();

            LastKeyboardState = new KeyboardState();
        }



        #region Fields

        public KeyboardState CurrentKeyboardState;

        public KeyboardState LastKeyboardState;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new input state.
        /// </summary>


        #endregion

        #region Public Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;

            CurrentKeyboardState = Keyboard.GetState(0);

        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
                return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool IsKeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsUp(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.W) : IsNewKeyPress(Keys.Up);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsDown(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.S) : IsNewKeyPress(Keys.Down);
        }

        public bool IsRight(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.D) : IsNewKeyPress(Keys.Right);
        }

        public bool IsLeft(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.A) : IsNewKeyPress(Keys.Left);
        }

        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }


        #endregion

        public int Vertical(bool wsad = false)
        {
            int dir = 0;
            if ((wsad && IsKeyPress(Keys.S)) || (!wsad && IsKeyPress(Keys.Down)))
                dir++;
            if ((wsad && IsKeyPress(Keys.W)) || (!wsad && IsKeyPress(Keys.Up)))
                dir--;
            return dir;
        }
        public int Horizontal(bool wsad = false)
        {
            int dir = 0;
            if ((wsad && IsKeyPress(Keys.A)) || (!wsad && IsKeyPress(Keys.Left)))
                dir--;
            if ((wsad && IsKeyPress(Keys.D)) || (!wsad && IsKeyPress(Keys.Right)))
                dir++;
            return dir;
        }
    }
}
