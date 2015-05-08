using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Free : Terrain
    {
        public Free()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Free"));
            _accessibility = Accessibility.Free;
        }
    }
}
