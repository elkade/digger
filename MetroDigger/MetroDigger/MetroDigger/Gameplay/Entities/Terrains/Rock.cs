using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Rock : Terrain
    {
        public Rock()
        {
            var grc = MediaManager.Instance;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Rock"));
            _accessibility = Accessibility.Rock;
        }

    }
}
