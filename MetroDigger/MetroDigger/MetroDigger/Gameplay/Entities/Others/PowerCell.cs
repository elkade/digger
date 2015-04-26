﻿using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class PowerCell : Item
    {
        public PowerCell()
        {
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.PowerCell, 1, false, 300, MediaManager.Instance.Scale) };
            Sprite.PlayAnimation(Animations[0]);
        }

        public override void GetCollected(ICollector collector)
        {
            collector.PowerCellCount++;
            base.GetCollected(collector);
        }
    }
}
