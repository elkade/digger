using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    class Tunnel : Metro
    {
        public Tunnel()
        {
            var grc = MediaManager.Instance;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Tunnel"));
        }
        public override int Clear(ref int stationsCount)
        {
            int points = 0;
            if (!IsCleared)
                points = 50;
            base.Clear(ref stationsCount);
            return points;
        }
    }
}
