using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Soil : Terrain
    {
        public Soil()
        {
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.Soil, 1, false, 300) };
            Sprite.PlayAnimation(Animations[0]);
            _accessibility = Accessibility.Soil;
        }

    }
}
