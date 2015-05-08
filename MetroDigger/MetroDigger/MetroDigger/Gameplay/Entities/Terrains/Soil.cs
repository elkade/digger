using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Soil : Terrain
    {
        public Soil()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Soil"));
            _accessibility = Accessibility.Soil;
        }

    }
}
