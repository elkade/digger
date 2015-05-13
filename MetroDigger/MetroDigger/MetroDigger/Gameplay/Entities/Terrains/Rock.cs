using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Teren ze skałą, na który nie mają dostępu inne obiekty
    /// </summary>
    class Rock : Terrain
    {
        /// <summary>
        /// Tworzy nowy teren ze skałą.
        /// </summary>
        public Rock()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Rock"));
            _accessibility = Accessibility.Rock;
        }

    }
}
