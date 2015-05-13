namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Teren z ziemią, na który obiekty mogą się dostać przy użyciu wiertła.
    /// </summary>
    class Soil : Terrain
    {
        /// <summary>
        /// Tworzy nowy teren ze skałą.
        /// </summary>
        public Soil()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Soil"));
            _accessibility = Accessibility.Soil;
        }

    }
}
