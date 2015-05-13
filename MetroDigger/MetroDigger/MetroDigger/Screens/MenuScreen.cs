using System;
using System.Collections.Generic;
using MetroDigger.Logging;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Bazowa klasa dla ekranów menu. U¿ytkownik mo¿e przesuwaæ wskaŸnik w górê/dó³, wybieraæ enterem i wychodziæ Esc.
    /// </summary>
    public abstract class MenuScreen : GameScreen
    {
        #region Fields

        readonly List<MenuObject> _menuObjects = new List<MenuObject>();
        int _selectedObjectIndex;
        readonly string _menuTitle;
        #endregion

        #region Properties


        /// <summary>
        /// Pobiera listê kontrolek, aby klasa pochodna mog³a j¹ edytowaæ.
        /// </summary>
        protected IList<MenuObject> MenuObjects
        {
            get { return _menuObjects; }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Tworzy now¹ instancjê Ekranu menu.
        /// </summary>
        protected MenuScreen(string menuTitle)
        {
            Logger.Log(menuTitle +" loaded");
            _menuTitle = menuTitle;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Odpowiada na ¿¹dania p³yn¹ce z klawiatury; zaznacza b¹dŸ odznacza kontrolkê
        /// </summary>
        public override void HandleInput(InputHandler input)
        {
            // Move to the previous menu entry?
            if (input.IsUp())
            {
                do
                {
                    _selectedObjectIndex--;
                    if (_selectedObjectIndex < 0)
                        _selectedObjectIndex = _menuObjects.Count - 1;
                } while (!MenuObjects[_selectedObjectIndex].IsSelectable);
            }

            if (input.IsDown())
            {
                do
                {
                    _selectedObjectIndex++;
                    if (_selectedObjectIndex >= _menuObjects.Count)
                        _selectedObjectIndex = 0;
                } while (!MenuObjects[_selectedObjectIndex].IsSelectable);
            }

            MenuObjects[_selectedObjectIndex].HandleInput(input);

            if (input.IsMenuSelect())
            {
                OnSelectEntry(_selectedObjectIndex);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }
        }


        private void OnSelectEntry(int entryIndex)
        {
            _menuObjects[entryIndex].OnSelectEntry();
        }


        /// <summary>
        /// Handler dla zdarzenia wyjœcia z menu
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }


        /// <summary>
        /// Prze³adowanie handlera wyjœcia z menu wzbogacone o argumenty.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }


        #endregion

        #region UpdateMovement and Draw


        private void UpdateMenuEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(0f, 175f);

            foreach (MenuObject menuObject in _menuObjects)
            {
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuObject.GetWidth() / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuObject.Position = position;

                position.Y += menuObject.GetHeight();
            }
        }


        /// <summary>
        /// Aktualizuje menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            while (!MenuObjects[_selectedObjectIndex].IsSelectable)
            {
                _selectedObjectIndex++;
                if (_selectedObjectIndex >= _menuObjects.Count)
                    _selectedObjectIndex = 0;
            }

            for (int i = 0; i < _menuObjects.Count; i++)
            {
                bool isSelected = IsActive && (i == _selectedObjectIndex);

                _menuObjects[i].Update(isSelected, gameTime);
            }
            UpdateMenuEntryLocations();

        }


        /// <summary>
        /// Rysuje menu
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = MediaManager.Instance.Font;

            spriteBatch.Begin();

            for (int i = 0; i < _menuObjects.Count; i++)
            {
                MenuObject menuObject = _menuObjects[i];

                bool isSelected = IsActive && (i == _selectedObjectIndex);

                menuObject.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2((float)graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(_menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}
