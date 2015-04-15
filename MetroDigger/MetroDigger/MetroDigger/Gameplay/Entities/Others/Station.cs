using System;
using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    class Station : Metro
    {
        //public static event Action Created;
        public static event Action Cleared;

        public Station()
        {
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.MetroStation, 1, false) };
            Sprite.PlayAnimation(Animations[0]);
            //if (Created != null)
            //    Created();
        }
        public override void Clear()
        {
            if (Cleared != null && !IsCleared)
                Cleared();
            base.Clear();
        }
    }
}
