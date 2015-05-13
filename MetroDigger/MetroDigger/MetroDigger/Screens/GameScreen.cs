using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Opisuje stan przej�cia mi�dzy kolejnymi menu.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }


    /// <summary>
    /// Klasa bazowa dla ekran�w. Ekran to pojedyncza warstwa,
    /// kt�ra ma logik� aktualizacji i rysowania oraz mo�e by� ��czona z innyni
    /// warstwami menu w celu stworzenia z�o�onego systemu.
    /// Klasa ta definiuje wszelkie animacje towarzysz�ce przej�ciom mi�dzy ekranami.
    /// </summary>
    public abstract class GameScreen
    {
        #region Properties

        /// <summary>
        /// Wskazuje na to, czy ekran jest popupem - czyli wy�wietla si� jako okienko przed
        /// ekranem macierzystym.
        /// </summary>
        public bool IsPopup { get; protected set; }


        /// <summary>
        /// Ustawia jak d�ugo zajmuje ekranowi przej�cie z innego ekranu.
        /// </summary>
        protected TimeSpan TransitionOnTime
        {
            set { _transitionOnTime = value; }
        }

        TimeSpan _transitionOnTime = TimeSpan.Zero;


        /// <summary>
        /// Ustawia jak d�ugo zajmuje ekranowi przej�ciew inny ekran.
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
        /// Pobiera parametr zaciemnienia ekranu przy przej�ciu.
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Zwraca obecny stan przej�cia.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _screenState; }
        }

        ScreenState _screenState = ScreenState.TransitionOn;


        /// <summary>
        /// Zwraca, czy ekran jest w trakcie zamykania si�.
        /// </summary>
        public bool IsExiting
        {
            set { _isExiting = value; }
        }

        bool _isExiting;


        /// <summary>
        /// Sprawdza, czy menu jest aktywne i powinno reagowa� na input u�ytkownika.
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
        /// Zwraca ScreenManager, kt�ry zarz�dza tym ekranem.
        /// </summary>
        public ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Tworzy now� instancj� klasy <see cref="GameScreen"/>.
        /// </summary>
        protected GameScreen()
        {
            IsPopup = false;
        }



        /// <summary>
        /// Aktualizuje przej�cie i stan ekranu.
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
        /// Metoda wirtualna wywo�ywana, gdy nale�y sprawdzi� stan klawiatury.
        /// </summary>
        public virtual void HandleInput(InputHandler input) { }


        /// <summary>
        /// Metoda wirtualna wywo�ywana, gdy ekran powinien si� odrysowa�
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }


        #endregion

        #region Public Methods


        /// <summary>
        /// Powoduje rozpocz�cie przej�cia zamykaj�cego ekran.
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
