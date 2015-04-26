#region File Description
//-----------------------------------------------------------------------------
// MenuScreen.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNA_GSM.Screens.MenuObjects;

#endregion

namespace MetroDigger
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        readonly List<MenuObject> _menuObjects = new List<MenuObject>();
        int _selectedObjectIndex = 0;
        readonly string _menuTitle;
        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuObject> MenuObjects
        {
            get { return _menuObjects; }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this._menuTitle = menuTitle;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputHandler input)
        {
            // Move to the previous menu entry?
            if (input.IsUp())
            {
                do//todo: mo�e si� zap�tli� w niesko�czono�� je�eli �aden nie jest selectable
                {
                    _selectedObjectIndex--;
                    if (_selectedObjectIndex < 0)
                        _selectedObjectIndex = _menuObjects.Count - 1;
                } while (!MenuObjects[_selectedObjectIndex].IsSelectable);
            }

            // Move to the next menu entry?
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

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputHandler helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect())
            {
                OnSelectEntry(_selectedObjectIndex);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            _menuObjects[entryIndex].OnSelectEntry();
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }


        #endregion

        #region UpdateMovement and Draw


        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 175f);

            // update each menu entry's location in turn
            for (int i = 0; i < _menuObjects.Count; i++)
            {
                MenuObject menuObject = _menuObjects[i];
                
                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuObject.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuObject.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuObject.GetHeight(this);
            }
        }


        /// <summary>
        /// Updates the menu.
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

            // UpdateMovement each nested MenuEntry object.
            for (int i = 0; i < _menuObjects.Count; i++)
            {
                bool isSelected = IsActive && (i == _selectedObjectIndex);

                _menuObjects[i].Update(this, isSelected, gameTime);
            }
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = MediaManager.Instance.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < _menuObjects.Count; i++)
            {
                MenuObject menuObject = _menuObjects[i];

                bool isSelected = IsActive && (i == _selectedObjectIndex);

                menuObject.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(_menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}