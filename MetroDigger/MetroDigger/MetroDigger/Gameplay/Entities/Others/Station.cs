using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    internal class Station : Metro
    {
        public Station()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Station"));
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