using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    /// <summary>
    /// MessageBox u¿ywany do wyœwietlania krótkich informacji
    /// </summary>


    public class MessageBoxScreen : GameScreen
    {
        /// <summary>
        /// Tworzy message box
        /// </summary>
        public event Action Accept;

        #region Fields

        private readonly Texture2D _gradientTexture;
        private readonly string _message;

        #endregion

        #region Events

        #endregion

        #region Initialization

        /// <summary>
        ///     Constructor lets the caller specify whether to include the standard
        ///     "A=ok, B=cancel" usage _text prompt.
        /// </summary>
        public MessageBoxScreen(string message)
        {
            _message = message;
            _gradientTexture = MediaManager.Instance.GetStaticAnimation("Water").Texture;
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        #endregion

        #region Handle Input

        /// <summary>
        ///     Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputHandler input)
        {
            if (input.IsMenuSelect() || input.IsMenuCancel())
            {
                ExitScreen();
                if (Accept != null)
                    Accept();
            }
        }
        #endregion

        #region Draw

        /// <summary>
        ///     Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = MediaManager.Instance.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha*2/3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(_message);
            Vector2 textPosition = (viewportSize - textSize)/2;

            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle(
                (int) textPosition.X - hPad,
                (int) textPosition.Y - vPad,
                (int) textSize.X + hPad*2,
                (int) textSize.Y + vPad*2);

            Color color = Color.White*TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);

            spriteBatch.DrawString(font, _message, textPosition, color);

            spriteBatch.End();
        }

        #endregion
    }
}