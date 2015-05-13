using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Opisuje stan przejœcia miêdzy kolejnymi menu.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }


    /// <summary>
    /// Klasa bazowa dla ekranów. Ekran to pojedyncza warstwa,
    /// która ma logikê aktualizacji i rysowania oraz mo¿e byæ ³¹czona z innyni
    /// warstwami menu w celu stworzenia z³o¿onego systemu.
    /// Klasa ta definiuje wszelkie animacje towarzysz¹ce przejœciom miêdzy ekranami.
    /// </summary>
    public abstract class GameScreen
    {
        #region Properties

        /// <summary>
        /// Wskazuje na to, czy ekran jest popupem - czyli wyœwietla siê jako okienko przed
        /// ekranem macierzystym.
        /// </summary>
        public bool IsPopup { get; protected set; }


        /// <summary>
        /// Ustawia jak d³ugo zajmuje ekranowi przejœcie z innego ekranu.
        /// </summary>
        protected TimeSpan TransitionOnTime
        {
            set { _transitionOnTime = value; }
        }

        TimeSpan _transitionOnTime = TimeSpan.Zero;


        /// <summary>
        /// Ustawia jak d³ugo zajmuje ekranowi przejœciew inny ekran.
        /// </summary>
        protected TimeSpan TransitionOffTime
        {
            private get { return _transitionOffTime; }
            set { _transitionOffTime = value; }
        }

        TimeSpan _transitionOffTime = TimeSpan.Zero;

        protected float TransitionPosition
        {
            get { return _transitionPosition; }
        }

        float _transitionPosition = 1;


        /// <summary>
        /// Pobiera parametr zaciemnienia ekranu przy przejœciu.
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Zwraca obecny stan przejœcia.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _screenState; }
        }

        ScreenState _screenState = ScreenState.TransitionOn;


        /// <summary>
        /// Zwraca, czy ekran jest w trakcie zamykania siê.
        /// </summary>
        public bool IsExiting
        {
            set { _isExiting = value; }
        }

        bool _isExiting;


        /// <summary>
        /// Sprawdza, czy menu jest aktywne i powinno reagowaæ na input u¿ytkownika.
        /// </summary>
        protected bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus &&
                       (_screenState == ScreenState.TransitionOn ||
                        _screenState == ScreenState.Active);
            }
        }

        bool _otherScreenHasFocus;


        /// <summary>
        /// Zwraca ScreenManager, który zarz¹dza tym ekranem.
        /// </summary>
        public ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Tworzy now¹ instancjê klasy <see cref="GameScreen"/>.
        /// </summary>
        protected GameScreen()
        {
            IsPopup = false;
        }



        /// <summary>
        /// Aktualizuje przejœcie i stan ekranu.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (_isExiting)
            {
                _screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, _transitionOffTime, 1))
                    ScreenManager.RemoveScreen(this);
            }
            else if (coveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, _transitionOffTime, 1))
                    _screenState = ScreenState.TransitionOff;
                else
                    _screenState = ScreenState.Hidden;
            }
            else
            {
                if (UpdateTransition(gameTime, _transitionOnTime, -1))
                    _screenState = ScreenState.TransitionOn;
                else
                    _screenState = ScreenState.Active;
            }
        }


        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            _transitionPosition += transitionDelta * direction;

            if (((direction < 0) && (_transitionPosition <= 0)) ||
                ((direction > 0) && (_transitionPosition >= 1)))
            {
                _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Metoda wirtualna wywo³ywana, gdy nale¿y sprawdziæ stan klawiatury.
        /// </summary>
        public virtual void HandleInput(InputHandler input) { }


        /// <summary>
        /// Metoda wirtualna wywo³ywana, gdy ekran powinien siê odrysowaæ
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }


        #endregion

        #region Public Methods


        /// <summary>
        /// Powoduje rozpoczêcie przejœcia zamykaj¹cego ekran.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(this);
            else
                _isExiting = true;
        }


        #endregion
    }
}
