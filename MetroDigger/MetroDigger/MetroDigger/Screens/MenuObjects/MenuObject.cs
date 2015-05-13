using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla kontrolek. Opisuje efekty przejścia kontrolek przy zmianie ekranu.
    /// </summary>
    abstract class MenuObject
    {
        #region Fields

        /// <summary>
        /// Tekst kontrolki
        /// </summary>
        protected string _text;

        /// <summary>
        /// Współczynnik blednięcia kontrolki przy przejściu
        /// </summary>
        float _selectionFade;

        /// <summary>
        /// Położenie kontrolki na ekranie.
        /// </summary>
        Vector2 _position;

        #endregion

        #region Properties


        /// <summary>
        /// Teks kontrolki.
        /// </summary>
        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        /// <summary>
        /// Pozycja kontrolki na ekranie.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// Określa, czy kontrolkę da sie zaznaczyć.
        /// </summary>
        public bool IsSelectable { get; set; }

        #endregion

        #region Initialization


        /// <summary>
        /// Tworzy nową kontrolkę.
        /// </summary>
        protected MenuObject(string text)
        {
            _text = text;
        }


        #endregion

        #region Events

        /// <summary>
        /// Zdarzenie wywoływanie przy wyborze kontrolki.
        /// </summary>
        public event EventHandler<EventArgs> Selected;


        /// <summary>
        /// Metoda do wywoływania zdarzenia wybrania kontrolki.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Aktualizuje kontrolkę
        /// </summary>
        public virtual void Update(bool isSelected, GameTime gameTime)
        {

            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Rysuje kontrolkę
        /// </summary>
        public void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.LightGreen : Color.White;

            color *= screen.TransitionAlpha;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = MediaManager.Instance.Font;

            Vector2 origin = new Vector2(0, (float)font.LineSpacing / 2);

            spriteBatch.DrawString(font, Text, _position, color, 0,
                                   origin, 1, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Pobiera ile miejsca potrzebuje kontrolka.
        /// </summary>
        public int GetHeight()
        {
            return MediaManager.Instance.Font.LineSpacing;
        }


        /// <summary>
        /// Pobiera szerokość kontrolki w celu wycentorwania jej
        /// </summary>
        public int GetWidth()
        {
            return (int)MediaManager.Instance.Font.MeasureString(Text).X;
        }


        #endregion

        public virtual void HandleInput(InputHandler input)
        {
        }
    }
}
