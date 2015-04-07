#region File Description
//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using MetroDigger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace XNA_GSM.Screens.MenuObjects
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry _text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class MenuEntry : MenuObject
    {

        #region Initialization

        public MenuEntry(string text)
            : base(text)
        {
            IsSelectable = true;
        }

        #endregion


    }
}
