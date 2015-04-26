using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    internal class Station : Metro
    {
        //public static event Action Created;
        public Station()
        {
            MediaManager grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.MetroStation, 1, false, 0, MediaManager.Instance.Scale) };
            Sprite.PlayAnimation(Animations[0]);
            //if (Created != null)
            //    Created();
        }

        public override int Clear(ref int stationsCount)
        {
            if (!IsCleared)
            {
                IsCleared = true;
                stationsCount--;
            }
            base.Clear(ref stationsCount);
            return 0;
        }
    }
}