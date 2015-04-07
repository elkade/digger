using System;
using MetroDigger;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_GSM.Screens.MenuObjects
{
    abstract class MenuObject
    {
        #region Fields

        /// <summary>
        /// The _text rendered for this entry.
        /// </summary>
        protected string _text;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        float selectionFade;

        /// <summary>
        /// The position at which the entry is drawn. This is set by the MenuScreen
        /// each frame in Update.
        /// </summary>
        Vector2 position;

        private bool isSelectable;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the _text of this menu entry.
        /// </summary>
        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsSelectable
        {
            get { return isSelectable; }
            set { isSelectable = value; }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified _text.
        /// </summary>
        public MenuObject(string text)
        {
            this._text = text;
        }


        #endregion

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade _text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw _text, centered on the middle of each line.
            GameManager _gameManager = screen.GameManager;
            SpriteBatch spriteBatch = _gameManager.SpriteBatch;
            SpriteFont font = GraphicResourceContainer.Instance.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, Text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return GraphicResourceContainer.Instance.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)GraphicResourceContainer.Instance.Font.MeasureString(Text).X;
        }


        #endregion

        public virtual void HandleInput(InputHandler input)
        {
        }
    }
}
