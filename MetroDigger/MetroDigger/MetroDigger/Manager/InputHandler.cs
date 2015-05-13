using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Manager
{
    /// <summary>
    /// Odpowiada za interpretacj� informacji pochodz�cych z klawiatury.
    /// Realizuje wzorzec projektowy Singleton.
    /// </summary>
    public class InputHandler
    {
        #region Singleton
        /// <summary>
        /// Zwraca instancj� klasy.
        /// </summary>
        public static InputHandler Instance { get { return _instance; } }

        private static readonly InputHandler _instance = new InputHandler();
        #endregion

        private InputHandler()
        {
            CurrentKeyboardState = new KeyboardState();

            _lastKeyboardState = new KeyboardState();
        }



        /// <summary>
        /// Obecny stan klawiatury
        /// </summary>
        public KeyboardState CurrentKeyboardState;

        private KeyboardState _lastKeyboardState;




        /// <summary>
        /// Odczytuje ostatni stan klawiatury.
        /// </summary>
        public void Update()
        {
            _lastKeyboardState = CurrentKeyboardState;

            CurrentKeyboardState = Keyboard.GetState(0);

        }


        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty nowy klawisz.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key));
        }

        private bool IsKeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za zaznaczenie opcji w menu.
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
        }


        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za anulowanie menu.
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }


        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za przej�cie do g�ry w menu lub grze.
        /// </summary>
        public bool IsUp(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.W) : IsNewKeyPress(Keys.Up);
        }

        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za przej�cie w d� w menu lub grze.
        /// </summary>
        public bool IsDown(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.S) : IsNewKeyPress(Keys.Down);
        }

        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za przej�cie w prawo w menu lub grze.
        /// </summary>
        public bool IsRight(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.D) : IsNewKeyPress(Keys.Right);
        }

        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za przej�cie w lewo w menu lub grze.
        /// </summary>
        public bool IsLeft(bool wsadEnabled = false)
        {
            return wsadEnabled ? IsNewKeyPress(Keys.A) : IsNewKeyPress(Keys.Left);
        }

        /// <summary>
        /// Sprawdza, czy zosta� wci�ni�ty klawisz odpowiedzialny za zapauzowanie gry.
        /// </summary>
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }
        /// <summary>
        /// Zwraca kierunek poruszania si� gracza zgodnie z wci�ni�tym klawiszem g�ra/d�
        /// </summary>
        /// <param name="wsad">Okre�la czy u�ywane s� strza�ki czy klawisze WSAD</param>
        /// <returns>Kierunek poruszania si� gracza</returns>
        public int Vertical(bool wsad = false)
        {
            int dir = 0;
            if ((wsad && IsKeyPress(Keys.S)) || (!wsad && IsKeyPress(Keys.Down)))
                dir++;
            if ((wsad && IsKeyPress(Keys.W)) || (!wsad && IsKeyPress(Keys.Up)))
                dir--;
            return dir;
        }
        /// <summary>
        /// Zwraca kierunek poruszania si� gracza zgodnie z wci�ni�tym klawiszem prawo/lewo
        /// </summary>
        /// <param name="wsad">Okre�la czy u�ywane s� strza�ki czy klawisze WSAD</param>
        /// <returns>Kierunek poruszania si� gracza</returns>

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
